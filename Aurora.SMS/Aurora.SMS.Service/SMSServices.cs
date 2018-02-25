using Aurora.Core.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aurora.SMS.EFModel;
using Aurora.SMS.Service.DTO;
using System.Web;
using Aurora.Insurance.Services.DTO;
using Aurora.SMS.Data;
using Aurora.SMS.EFModel.Enumerators;
using Aurora.SMS.Providers;
using LinqKit;
using Aurora.SMS.AWS;
using Aurora.SMS.AWS.Models;

namespace Aurora.SMS.Service
{
    public interface ISMSServices
    {
        Task<Guid> SendBulkSMS(IEnumerable<SMSMessageDTO> messagesToSent, string providerName);
        IEnumerable<DTO.SMSMessageDTO> ConstructSMSMessages(IEnumerable<ContractDTO> recepients, int templateId);
        string GetAvailableCredits(string smsGateWayName);
        /// <summary>
        /// Gets the available credits by suplying the credentials.
        /// It is mainly used for test purposes
        /// </summary>
        /// <param name="smsGateWayName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        string GetAvailableCredits(string smsGateWayName,string userName,string password);
        IEnumerable<Provider> GetAllProviders();
        IEnumerable<SMSHistory> GetHistory(SmsHistoryCriteriaDTO smsHistoryCriteriaDTO);
        void SaveProxy(Provider smsProxy);
    }


    public class SMSServices : DbServiceBase<SMSDb>, ISMSServices
    {
        private readonly IAWSServices _AWSServices;
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public SMSServices(SMSDb db, IAWSServices AWSServices) :base(db)
        {
            _AWSServices = AWSServices;
        }

    /// <summary>
    /// Sends messages to the provider, creates a session in the history table and saves the messages as history
    /// under this session
    /// </summary>
    /// <param name="messagesToSent"></param>
    /// <param name="providerId"></param>
    /// <returns>Returns the session ID</returns>
    public async Task<Guid> SendBulkSMS(IEnumerable<SMSMessageDTO> messagesToSent, string providerName)
        {
           
            // get the provider data
            Provider provider =  DbContext.Providers.Single(x=>x.Name==providerName);
            Guid sessionId = Guid.NewGuid();
            // First job is to save the messages into the history table in order to populate the id
            var smsToSent = new List<SMSHistory>();
            foreach (var msg in messagesToSent)
            {
                SMSHistory smsHistory = new SMSHistory();
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
                smsToSent.Add(smsHistory);
          
            }
            await DbContext.SMSHistoryRecords.AddRangeAsync(smsToSent);
            await DbContext.SaveChangesAsync();
            await _AWSServices.PushMessagesAsync(AutoMapper.Mapper.Map<IEnumerable<SMSMessage>>(smsToSent), sessionId);

            //List<Task> serverRequests = new List<Task>();
            //// TODO:Need to abstract the ClientProviderFactory
            //var smsProviderProxy = ClientProviderFactory.CreateClient(providerName, provider.UserName, provider.PassWord);
            ////LINQ to Entities does not recognize the method 'Boolean IsNullOrWhiteSpace(System.String)'                  
            ////http://stackoverflow.com/questions/9606979/string-isnullorwhitespace-in-linq-expression
            //foreach (var historysms in DbContext.SMSHistoryRecords.Where(m=> (m.SessionId== sessionId) && (m.MobileNumber!=null) && m.MobileNumber.Trim()!=string.Empty).ToArray())
            //{
            //    // Collect all tasks in an array
            //    serverRequests.Add(SendSMSToProvider(smsProviderProxy, provider, historysms));

            //}
            //Task.WaitAll(serverRequests.ToArray());
            
            return sessionId;
        }


        /// <summary>
        /// Contrsucts an SMS Message for every Recepient
        /// </summary>
        /// <param name="recepients"></param>
        /// <param name="templateId">The templateId that will be used</param>
        /// <returns></returns>
        public IEnumerable<SMSMessageDTO> ConstructSMSMessages(IEnumerable<ContractDTO> recepients, int templateId)
        {
            var template = DbContext.Find<Template>(templateId);
            var templateFields = DbContext.TemplateFields.ToArray();

            var smsList = new List<SMSMessageDTO>();
            if (template == null)
            {
                throw new NullReferenceException(string.Format("The template with id:{0} cannot be found in the db!",templateId));
            }

            foreach (var recepient in recepients)
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
                foreach (var templateField in templateFields)
                {
                    var regExp = "<div class=\"alert alert-dismissible alert-success\" contenteditable=\"false\" style=\"display:inline-block\"><button type=\"button\" class=\"close\" alert-dismiss=\"alert\">×</button><span>" + templateField.Name + "</span></div>";
                        //"<div class=\"alert alert-dismissible alert-success\" contenteditable=\"false\" style=\"display:inline-block\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button><span>" + templateField.Name + "</span></div>";
                    smsText = Regex.Replace(smsText, regExp, GetFormattedValue(recepient, templateField));
                }
                smsList.Add(new DTO.SMSMessageDTO(){
                    ContractId= recepient.Contractid,
                    Message=smsText,
                    MobileNumber=recepient.MobileNumber,
                    PersonId=recepient.PersonId,
                    TemplateId= templateId
                });

            }
            return smsList;
        }


        private string GetFormattedValue(ContractDTO recepient,
                                            EFModel.TemplateField templateField)
        {
            return  string.Format("{0:" + (templateField.DataFormat ?? string.Empty) + "}", recepient.GetType().GetProperty(templateField.Name).GetValue(recepient, null));
        }


        /// <summary>
        /// Sends a message to the SMS gateway
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="smsHistory"></param>
        /// <returns></returns>
        private async Task SendSMSToProvider(ISMSClientProxy smsClient,
                                                    EFModel.Provider provider,
                                                    EFModel.SMSHistory smsHistory)
        {
            // send the sms to the provider and return the control to the 
            var result = await smsClient.SendSMSAsync(smsHistory.Id,
                                                    smsHistory.MobileNumber,
                                                    smsHistory.Message,
                                                    null,
                                                    null).ConfigureAwait(false);
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
            return;
        }

        public string GetAvailableCredits(string smsGateWayName)
        {
            Provider provider = DbContext.Providers.Find(smsGateWayName);
            return GetAvailableCredits(smsGateWayName, provider.UserName, provider.PassWord);
        }

        public string GetAvailableCredits(string smsGateWayName, string userName, string password)
        {
            // TODO:Need to abstract the ClientProviderFactory
            var smsProviderProxy = ClientProviderFactory.CreateClient(smsGateWayName, userName, userName);
            return  smsProviderProxy.GetAvailableCreditsAsync().Result;

        }

        public IEnumerable<Provider> GetAllProviders()
        {
            return DbContext.Providers.ToArray();
        }

        public IEnumerable<SMSHistory> GetHistory(SmsHistoryCriteriaDTO smsHistoryCriteriaDTO)
        {
            var query = DbContext.SMSHistoryRecords.AsQueryable().AsExpandable();
            var finalExpression = PredicateBuilder.New<SMSHistory>(true);
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
            var messageStatusExpression = PredicateBuilder.New<SMSHistory>(true);
            if (smsHistoryCriteriaDTO.MessageStatusList!=null)
            { 
                foreach (var messageStatus in smsHistoryCriteriaDTO.MessageStatusList)
                {
                    messageStatusExpression = messageStatusExpression.Or (c => c.Status == messageStatus);
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
    }
}
