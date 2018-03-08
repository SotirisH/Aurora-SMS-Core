using Aurora.SMS.AWS;
using Aurora.SMS.AWS.Models;
using Aurora.SMS.Data;
using Aurora.SMS.EFModel;
using Aurora.SMS.EFModel.Enumerators;
using Aurora.SMS.Providers;
using Aurora.SMS.Worker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aurora.SMS.Worker.Consumer
{
    public class ConsumeSMS : IConsumerSMS
    {
        private readonly ISQSsmsServices _sQSsmsServices;
        private readonly SMSDb _dbContext;
        private readonly IDatabaseCommands _databaseCommands;
        private readonly IClientProviderFactory _clientProviderFactory;

        public ConsumeSMS(SMSDb dbContext,
            ISQSsmsServices sQSsmsServices,
            IDatabaseCommands databaseCommands,
            IClientProviderFactory clientProviderFactory)
        {
            _sQSsmsServices = sQSsmsServices ?? throw new ArgumentNullException(nameof(sQSsmsServices));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _databaseCommands = databaseCommands ?? throw new ArgumentNullException(nameof(databaseCommands));
            _clientProviderFactory = clientProviderFactory ?? throw new ArgumentNullException(nameof(clientProviderFactory));
        }

        public async Task<bool> Execute()
        {
            var messages = await _sQSsmsServices.GetMessagesAsync();
            List<Task> serverRequests = new List<Task>();
            foreach (var message in messages)
            {
                // get the provider data
                Provider provider = _dbContext.Providers.Single(x => x.Name == message.ProviderName);
                var smsClient = _clientProviderFactory.CreateClient(message.ProviderName, provider.UserName, provider.PassWord);

                // Collect all tasks in an array
                serverRequests.Add(SendSMSToProvider(smsClient, message));
            }
            await Task.WhenAll(serverRequests.ToArray());
            return true;
        }

        /// <summary>
        /// Sends a message to the SMS gateway
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="smsHistory"></param>
        /// <returns></returns>
        private async Task SendSMSToProvider(ISMSClientProxy smsClient,
                                            SMSMessage smsMessage)
        {
            // send the sms to the provider and return the control to the
            var result = await smsClient.SendSMSAsync(smsMessage.Id,
                                                    smsMessage.MobileNumber,
                                                    smsMessage.Message,
                                                    null,
                                                    null);
            //Delete the message from the queue
            await _sQSsmsServices.DeleteMessageAsync(smsMessage.ReceiptHandle);

            //Update the status of the message
            await _databaseCommands.UpdateSMSHistoryAsync(smsMessage.Id,
                result.ReturnedMessage,
                result.TimeStamp,
                result.ProviderId,
                result.MessageStatus == MessageStatus.Delivered);
            return;
        }
    }
}