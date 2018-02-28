using Aurora.SMS.Data;
using Aurora.SMS.EFModel.Enumerators;
using Aurora.SMS.Worker.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Aurora.SMS.Worker.Consumer
{
    public class DatabaseCommands : IDatabaseCommands
    {
        private readonly SMSDb _dbContext;

        public DatabaseCommands(SMSDb dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task UpdateSMSHistoryAsync(long id,
            string providerFeedback,
            DateTime providerFeedBackTimeStamp,
            string providerMsgId,
            bool delivered)
        {
            //Since you're using named parameters, you have to specify the matching name for the parameter you're passing.
            return _dbContext.Database.ExecuteSqlCommandAsync(
                "exec dbo.usp_update_sms_history @Id, @ProviderFeedBackDateTime, @ProviderFeedback, @ProviderMsgId, @Status",
                new SqlParameter("@Id", id),
                new SqlParameter("@ProviderFeedBackDateTime", providerFeedBackTimeStamp),
                new SqlParameter("@ProviderFeedback", providerFeedback),
                new SqlParameter("@ProviderMsgId", providerMsgId),
                new SqlParameter("@Status", delivered ? MessageStatus.Delivered : MessageStatus.Error)
            );
        }
    }
}