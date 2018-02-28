using Aurora.SMS.AWS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aurora.SMS.AWS.Interfaces
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
}