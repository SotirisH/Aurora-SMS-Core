using Aurora.Core.Data;
using Aurora.SMS.AWS.Models;
using Aurora.SMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aurora.SMS.AWS
{
    /// <summary>
    /// An interface the data actions
    /// </summary>
    public interface IDataServices
    {
        /// <summary>
        /// Returns all the pending messages that under a specific sessionId
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        IEnumerable<SMSMessage> GetPendingMessagesBySessionId(Guid sessionId);
    }

    public class DataServices : DbServiceBase<SMSDb>, IDataServices
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public DataServices(SMSDb db) : base(db)
        {
        }

        public IEnumerable<SMSMessage> GetPendingMessagesBySessionId(Guid sessionId)
        {
            return AutoMapper.Mapper.Map<IEnumerable<SMSMessage>>(DbContext.SMSHistoryRecords
                .Where(x => x.SessionId == sessionId && x.Status == EFModel.Enumerators.MessageStatus.Pending).ToArray());
        }
    }
}