using System;
using System.Threading.Tasks;

namespace Aurora.SMS.Worker.Interfaces
{
    /// <summary>
    /// Invokes raw SQL against the database
    /// </summary>
    public interface IDatabaseCommands
    {
        /// <summary>
        /// Updates a recond in the SMSHistory.
        /// The redord normally is updated after is sent to the provider.
        /// </summary>
        /// <param name="id">Message id</param>
        /// <param name="providerFeedback">The response message from the provider</param>
        /// <param name="ProviderFeedBackTimeStamp">The timestamp of the response</param>
        /// <param name="providerMsgId">The unique Id that the provider has given to the message</param>
        /// <param name="delivered">True/false</param>
        /// <returns></returns>
        Task UpdateSMSHistoryAsync(long id,
            string providerFeedback,
            DateTime providerFeedBackTimeStamp,
            string providerMsgId,
            bool delivered);
    }
}