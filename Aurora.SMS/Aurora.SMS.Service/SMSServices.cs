using Aurora.Core.Data;
using Aurora.Insurance.Services.DTO;
using Aurora.SMS.Data;
using Aurora.SMS.EFModel;
using Aurora.SMS.EFModel.Enumerators;
using Aurora.SMS.Providers;
using Aurora.SMS.Service.DTO;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Aurora.SMS.Service
{
    public interface ISMSServices
    {
        Guid SendBulkSMS(IEnumerable<SMSMessageDTO> messagesToSent,
            string providerName);

        IEnumerable<SMSMessageDTO> ConstructSMSMessages(IEnumerable<ContractDTO> recepients,
            int templateId);

        string GetAvailableCredits(string smsGateWayName);

        /// <summary>
        ///     Gets the available credits by suplying the credentials.
        ///     It is mainly used for test purposes
        /// </summary>
        /// <param name="smsGateWayName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        string GetAvailableCredits(string smsGateWayName,
            string userName,
            string password);

        IEnumerable<Provider> GetAllProviders();
        IEnumerable<SMSHistory> GetHistory(SmsHistoryCriteriaDTO smsHistoryCriteriaDTO);
        void SaveProxy(Provider smsProxy);
    }


    public class SMSServices : DbServiceBase<SMSDb>, ISMSServices
    {
        /// <summary>
        ///     Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public SMSServices(SMSDb db) : base(db)
        {
        }

        /// <summary>
        ///     Sends messages to the provider, creates a session in the history table and saves the messages as history
        ///     under this session
        /// </summary>
        /// <param name="messagesToSent"></param>
        /// <param name="providerId"></param>
        /// <returns>Returns the session ID</returns>
        public Guid SendBulkSMS(IEnumerable<SMSMessageDTO> messagesToSent,
            string providerName)
        {
            // get the provider data
            Provider provider = DbContext.Providers.Single(x => x.Name == providerName);
            var sessionId = Guid.NewGuid();
            // First job is to save the messages into the history table in order to populate the id
            foreach (SMSMessageDTO msg in messagesToSent)
            {
                var smsHistory = new SMSHistory();
                smsHistory.ContractId = msg.ContractId;
                smsHistory.Message = msg.Message;
                smsHistory.MobileNumber = msg.MobileNumber;
                smsHistory.PersonId = msg.PersonId;
                smsHistory.ProviderName = providerName;
                smsHistory.TemplateId = msg.TemplateId;
                smsHistory.Status = MessageStatus.Pending;
                smsHistory.SessionId = sessionId;
                smsHistory.SessionName = providerName + "|" + DateTime.Now;
                smsHistory.SendDateTime = DateTime.Now;
                if (string.IsNullOrWhiteSpace(msg.MobileNumber))
                {
                    smsHistory.ProviderFeedback = "The mobile number is not present!";
                    smsHistory.Status = MessageStatus.Skipped;
                }

                DbContext.Add(smsHistory);
            }

            DbContext.SaveChanges();

            var serverRequests = new List<Task>();
            // TODO:Need to abstract the ClientProviderFactory
            ISMSClientProxy smsProviderProxy = ClientProviderFactory.CreateClient(providerName, provider.UserName, provider.PassWord);
            //LINQ to Entities does not recognize the method 'Boolean IsNullOrWhiteSpace(System.String)'                  
            //http://stackoverflow.com/questions/9606979/string-isnullorwhitespace-in-linq-expression
            foreach (SMSHistory historysms in DbContext.SMSHistoryRecords
                .Where(m => m.SessionId == sessionId && m.MobileNumber != null && m.MobileNumber.Trim() != string.Empty)
                .ToArray())
            {
                // Collect all tasks in an array
                serverRequests.Add(SendSMSToProvider(smsProviderProxy, provider, historysms));
            }

            Task.WaitAll(serverRequests.ToArray());
            DbContext.SaveChanges();
            return sessionId;
        }


        /// <summary>
        ///     Contrsucts an SMS Message for every Recepient
        /// </summary>
        /// <param name="recepients"></param>
        /// <param name="templateId">The templateId that will be used</param>
        /// <returns></returns>
        public IEnumerable<SMSMessageDTO> ConstructSMSMessages(IEnumerable<ContractDTO> recepients,
            int templateId)
        {
            var template = DbContext.Find<Template>(templateId);
            TemplateField[] templateFields = DbContext.TemplateFields.ToArray();

            var smsList = new List<SMSMessageDTO>();
            if (template == null)
            {
                throw new NullReferenceException(string.Format("The template with id:{0} cannot be found in the db!", templateId));
            }

            foreach (ContractDTO recepient in recepients)
            {
                string smsText = template.Text;
                // God bless regular expressions :)
                // Nice tester:https://regex101.com/r/cU5lC2/1
                /* The rule is that the injected fields inside the template text have the format {templateField.Name}
                 * Here, i loop through all the templateFields(there are only few so iloop them all even if don't exist in thetemplate text )
                 * and i repace with a value that i extract from the recepient Object using reflection
                 * */


                //Sample"CreateEdit&nbsp;<div class="alert alert-dismissible alert-success" contenteditable="false" style="display:inline-block"><button type="button" class="close" data-dismiss="alert">×</button><span>CompanyDescription</span></div>
                // Decode the string using HttpUtility.HtmlDecode in order to replace htm elements such as
                // &nbsp; with their real representaion
                smsText = HttpUtility.HtmlDecode(smsText);
                foreach (TemplateField templateField in templateFields)
                {
                    string regExp =
                        "<div class=\"alert alert-dismissible alert-success\" contenteditable=\"false\" style=\"display:inline-block\"><button type=\"button\" class=\"close\" alert-dismiss=\"alert\">×</button><span>" +
                        templateField.Name +
                        "</span></div>";
                    //"<div class=\"alert alert-dismissible alert-success\" contenteditable=\"false\" style=\"display:inline-block\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button><span>" + templateField.Name + "</span></div>";
                    smsText = Regex.Replace(smsText, regExp, GetFormattedValue(recepient, templateField));
                }

                smsList.Add(new SMSMessageDTO
                {
                    ContractId = recepient.Contractid,
                    Message = smsText,
                    MobileNumber = recepient.MobileNumber,
                    PersonId = recepient.PersonId,
                    TemplateId = templateId
                });
            }

            return smsList;
        }

        public string GetAvailableCredits(string smsGateWayName)
        {
            Provider provider = DbContext.Providers.Find(smsGateWayName);
            return GetAvailableCredits(smsGateWayName, provider.UserName, provider.PassWord);
        }

        public string GetAvailableCredits(string smsGateWayName,
            string userName,
            string password)
        {
            // TODO:Need to abstract the ClientProviderFactory
            ISMSClientProxy smsProviderProxy = ClientProviderFactory.CreateClient(smsGateWayName, userName, userName);
            return smsProviderProxy.GetAvailableCreditsAsync().Result;
        }

        public IEnumerable<Provider> GetAllProviders()
        {
            return DbContext.Providers.ToArray();
        }

        public IEnumerable<SMSHistory> GetHistory(SmsHistoryCriteriaDTO smsHistoryCriteriaDTO)
        {
            IQueryable<SMSHistory> query = DbContext.SMSHistoryRecords.AsQueryable().AsExpandable();
            ExpressionStarter<SMSHistory> finalExpression = PredicateBuilder.New<SMSHistory>(true);
            if (smsHistoryCriteriaDTO.SessionId.HasValue)
            {
                finalExpression = finalExpression.And(c => c.SessionId == smsHistoryCriteriaDTO.SessionId);
            }

            if (smsHistoryCriteriaDTO.SendDateFrom.HasValue)
            {
                finalExpression = finalExpression.And(c => c.SendDateTime >= smsHistoryCriteriaDTO.SendDateFrom);
            }

            if (smsHistoryCriteriaDTO.SendDateTo.HasValue)
            {
                finalExpression = finalExpression.And(c => c.SendDateTime <= smsHistoryCriteriaDTO.SendDateFrom);
            }

            ExpressionStarter<SMSHistory> messageStatusExpression = PredicateBuilder.New<SMSHistory>(true);
            if (smsHistoryCriteriaDTO.MessageStatusList != null)
            {
                foreach (MessageStatus messageStatus in smsHistoryCriteriaDTO.MessageStatusList)
                {
                    messageStatusExpression = messageStatusExpression.Or(c => c.Status == messageStatus);
                }
            }

            finalExpression = finalExpression.And(messageStatusExpression);

            return query.Where(finalExpression).ToArray();
        }

        public void SaveProxy(Provider smsProxy)
        {
            DbContext.Update(smsProxy);
            DbContext.SaveChanges();
        }


        private string GetFormattedValue(ContractDTO recepient,
            TemplateField templateField)
        {
            return string.Format("{0:" + (templateField.DataFormat ?? string.Empty) + "}",
                recepient.GetType().GetProperty(templateField.Name).GetValue(recepient, null));
        }


        /// <summary>
        ///     Sends a message to the SMS gateway
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="smsHistory"></param>
        /// <returns></returns>
        private async Task SendSMSToProvider(ISMSClientProxy smsClient,
            Provider provider,
            SMSHistory smsHistory)
        {
            // send the sms to the provider and return the control to the 
            SMSResult result = await smsClient.SendSMSAsync(smsHistory.Id,
                    smsHistory.MobileNumber,
                    smsHistory.Message,
                    null,
                    null)
                .ConfigureAwait(false);
            // when a task finish the history record is updated
            smsHistory.ProviderFeedback = result.ReturnedMessage;
            smsHistory.ProviderFeedBackDateTime = result.TimeStamp;
            smsHistory.ProviderMsgId = result.ProviderId;
            if (result.MessageStatus == MessageStatus.Delivered)
            {
                smsHistory.Status = MessageStatus.Delivered;
            }
            else
            {
                smsHistory.Status = MessageStatus.Error;
            }

            // _smsHistoryRepository.Update(smsHistory);
        }
    }
}
