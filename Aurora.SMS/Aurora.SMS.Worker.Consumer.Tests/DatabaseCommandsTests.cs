using Aurora.Core.Data;
using Aurora.SMS.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Aurora.SMS.Worker.Consumer.Tests
{
    public class DatabaseCommandsTests
    {
        [Fact]
        public async Task UpdateSMSHistoryAsync()
        {
            var sMSDbconnection = @"Server =.\SQL16; Database = SMSDbCore; Trusted_Connection = True;";
            var optionsBuilder = new DbContextOptionsBuilder<SMSDb>();

            var mockICurrentUserService = new Mock<ICurrentUserService>();
            mockICurrentUserService.Setup(p => p.GetCurrentUser()).Returns("TestUser");

            optionsBuilder.UseSqlServer(sMSDbconnection);
            var db = new SMSDb(optionsBuilder.Options, mockICurrentUserService.Object);
            var target = new DatabaseCommands(db);
            await target.UpdateSMSHistoryAsync(33, "new", DateTime.Now, "RET:11", true);
        }
    }
}