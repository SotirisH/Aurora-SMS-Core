using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;
using Amazon.SQS.Model;
using Aurora.SMS.AWS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aurora.SMS.AWS
{
    /// <summary>
    /// Interface for AWS
    /// </summary>
    public interface ISQSsmsServices
    {
        /// <summary>
        /// Pushes SMS messages into the SQS
        /// </summary>
        /// <param name="sMSMessage"></param>
        Task PushMessagesAsync(IEnumerable<SMSMessage> sMSMessages, Guid sessionId);

        /// <summary>
        /// Retrieves messages from the SQS
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SMSMessage>> GetMessagesAsync();

        /// <summary>
        /// Deletes a message from the QUEUE
        /// </summary>
        /// <returns></returns>
        Task DeleteMessageAsync(string messageRecieptHandle);

        Task<int> GetVisibilityTimeOut();
    }

    public class SQSsmsServices : ISQSsmsServices
    {
        private readonly AWSOptions _aWSOptions;
        private readonly string _queueName;

        /// <summary>
        /// Primary constructor where the AWSOptions are passed
        /// </summary>
        /// <param name="aWSOptions"></param>
        public SQSsmsServices(SQSsmsServicesOptions sQSsmsServicesOptions)
        {
            if(sQSsmsServicesOptions==null)
            {
               throw new  ArgumentNullException(nameof(sQSsmsServicesOptions)); ;
            }
            _aWSOptions = sQSsmsServicesOptions.AWSOptions ?? throw new ArgumentNullException(nameof(sQSsmsServicesOptions.AWSOptions));
            _queueName = sQSsmsServicesOptions.QueueName ?? throw new ArgumentNullException(nameof(sQSsmsServicesOptions.QueueName)); ;
        }

        public async Task DeleteMessageAsync(string messageRecieptHandle)
        {
            try
            {
                using (var sqs = _aWSOptions.CreateServiceClient<IAmazonSQS>())
                {
                    var queueUrlResponse = await sqs.GetQueueUrlAsync(_queueName);
                    string myQueueUrl = queueUrlResponse.QueueUrl;

                    var deleteRequest = new DeleteMessageRequest { QueueUrl = myQueueUrl, ReceiptHandle = messageRecieptHandle };
                    await sqs.DeleteMessageAsync(deleteRequest);
                }
            }
            catch (AmazonSQSException ex)
            {
                Console.WriteLine("Caught Exception: " + ex.Message);
                Console.WriteLine("Response Status Code: " + ex.StatusCode);
                Console.WriteLine("Error Code: " + ex.ErrorCode);
                Console.WriteLine("Error Type: " + ex.ErrorType);
                Console.WriteLine("Request ID: " + ex.RequestId);
                throw;
            }
        }

        public async Task<IEnumerable<SMSMessage>> GetMessagesAsync()
        {
            var returnMessages = new List<SMSMessage>();
            try
            {
                using (var sqs = _aWSOptions.CreateServiceClient<IAmazonSQS>())
                {
                    var queueUrlResponse = await sqs.GetQueueUrlAsync(_queueName);
                    string myQueueUrl = queueUrlResponse.QueueUrl;
                    //Receiving a message
                    var receiveMessageRequest = new ReceiveMessageRequest { QueueUrl = myQueueUrl, MaxNumberOfMessages = 5 };
                    // The message attributes are not returned by default
                    receiveMessageRequest.MessageAttributeNames.Add("*");
                    var receiveMessageResponse = await sqs.ReceiveMessageAsync(receiveMessageRequest);

                    if (receiveMessageResponse.Messages != null)
                    {
                        foreach (var message in receiveMessageResponse.Messages)
                        {
                            var newSMSMessage = new SMSMessage()
                            {
                                Id = Convert.ToInt64(message.MessageAttributes[SMSMessage.SMSMessageAttributes.IdName].StringValue),
                                Message = message.Body,
                                MobileNumber = message.MessageAttributes[SMSMessage.SMSMessageAttributes.MobileNumberName].StringValue,
                                ProviderName = message.MessageAttributes[SMSMessage.SMSMessageAttributes.ProviderName].StringValue,
                                ReceiptHandle = message.ReceiptHandle
                            };
                            returnMessages.Add(newSMSMessage);
                        }
                    }
                }
                return returnMessages;
            }
            catch (AmazonSQSException ex)
            {
                Console.WriteLine("Caught Exception: " + ex.Message);
                Console.WriteLine("Response Status Code: " + ex.StatusCode);
                Console.WriteLine("Error Code: " + ex.ErrorCode);
                Console.WriteLine("Error Type: " + ex.ErrorType);
                Console.WriteLine("Request ID: " + ex.RequestId);
                throw;
            }
        }

        public async Task<int> GetVisibilityTimeOut()
        {
            using (var sqs = _aWSOptions.CreateServiceClient<IAmazonSQS>())
            {
                var queueUrlResponse = await sqs.GetQueueUrlAsync(_queueName);
                string myQueueUrl = queueUrlResponse.QueueUrl;

                var queueAttributes = await sqs.GetQueueAttributesAsync(myQueueUrl, new List<string>() { "VisibilityTimeout" });
                return queueAttributes.VisibilityTimeout;
            }
        }

        public async Task PushMessagesAsync(IEnumerable<SMSMessage> sMSMessages, Guid sessionId)
        {
            try
            {
                using (var sqs = _aWSOptions.CreateServiceClient<IAmazonSQS>())
                {
                    //Create a queue
                    var sqsRequest = new CreateQueueRequest { QueueName = _queueName };
                    var createQueueResponse = await sqs.CreateQueueAsync(sqsRequest);
                    string myQueueUrl = createQueueResponse.QueueUrl;

                    // Construct a message and send to the queue
                    foreach (var sMSMessage in sMSMessages)
                    {
                        var sendMessageRequest = new SendMessageRequest
                        {
                            QueueUrl = myQueueUrl, //URL from initial queue creation
                            MessageBody = sMSMessage.Message
                        };
                        sendMessageRequest.MessageAttributes.Add(SMSMessage.SMSMessageAttributes.IdName, new MessageAttributeValue() { DataType = "String", StringValue = sMSMessage.Id.ToString() });
                        sendMessageRequest.MessageAttributes.Add(SMSMessage.SMSMessageAttributes.MobileNumberName, new MessageAttributeValue() { DataType = "String", StringValue = string.IsNullOrWhiteSpace(sMSMessage.MobileNumber) ? "N/A" : sMSMessage.MobileNumber });
                        sendMessageRequest.MessageAttributes.Add(SMSMessage.SMSMessageAttributes.ProviderName, new MessageAttributeValue() { DataType = "String", StringValue = sMSMessage.ProviderName });
                        await sqs.SendMessageAsync(sendMessageRequest);
                    }
                }
            }
            catch (AmazonSQSException ex)
            {
                Console.WriteLine("Caught Exception: " + ex.Message);
                Console.WriteLine("Response Status Code: " + ex.StatusCode);
                Console.WriteLine("Error Code: " + ex.ErrorCode);
                Console.WriteLine("Error Type: " + ex.ErrorType);
                Console.WriteLine("Request ID: " + ex.RequestId);
                throw;
            }
        }
    }
}