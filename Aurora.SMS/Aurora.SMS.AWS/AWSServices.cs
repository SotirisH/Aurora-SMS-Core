using Aurora.SMS.AWS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Amazon.Extensions.NETCore.Setup;

namespace Aurora.SMS.AWS
{
    /// <summary>
    /// Interface for AWS
    /// </summary>
    public interface IAWSServices
    {
        /// <summary>
        /// Pushes SMS messages into the SQS
        /// </summary>
        /// <param name="sMSMessage"></param>
        Task PushMessagesAsync(IEnumerable<SMSMessage> sMSMessages, Guid sessionId);

    }

    public class AWSServices : IAWSServices
    {
        private readonly AWSOptions _aWSOptions;
        /// <summary>
        /// Primary constructor where the AWSOptions are passed
        /// </summary>
        /// <param name="aWSOptions"></param>
        public AWSServices(AWSOptions aWSOptions)
        {
            _aWSOptions = aWSOptions;
        }
        public async Task PushMessagesAsync(IEnumerable<SMSMessage> sMSMessages, Guid sessionId)
        {
            try
            {
                using (var sqs = _aWSOptions.CreateServiceClient<IAmazonSQS>())
                {
                    //Create a queue
                    var sqsRequest = new CreateQueueRequest { QueueName = "SMS" };
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
                        sendMessageRequest.MessageAttributes.Add("Id", new MessageAttributeValue() { DataType = "String", StringValue = sMSMessage.Id.ToString() });
                        sendMessageRequest.MessageAttributes.Add("MobileNumber", new MessageAttributeValue() { DataType = "String", StringValue = sMSMessage.MobileNumber });
                        sendMessageRequest.MessageAttributes.Add("ProviderName", new MessageAttributeValue() { DataType = "String", StringValue = sMSMessage.ProviderName });
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
