using Aurora.Core.Data;
using Aurora.SMS.AWS;
using Aurora.SMS.AWS.Tests;
using Aurora.SMS.Data;
using Aurora.SMS.Providers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Aurora.SMS.Worker.Consumer.Tests
{
    public class ConsumeSMSTests
    {
        [Fact]
        public async Task ExecuteTest()
        {
            var sMSDbconnection = @"Server =.\SQL16; Database = SMSDbCore; Trusted_Connection = True;";
            var optionsBuilder = new DbContextOptionsBuilder<SMSDb>();

            var mockICurrentUserService = new Mock<ICurrentUserService>();
            mockICurrentUserService.Setup(p => p.GetCurrentUser()).Returns("TestUser");
            optionsBuilder.UseSqlServer(sMSDbconnection);
            var db = new SMSDb(optionsBuilder.Options, mockICurrentUserService.Object);
            var sQSsmsServices = new SQSsmsServices(ConfigurationHelper.GetAWSConfiguration(), "SMS");
            var databaseCommands = new DatabaseCommands(db);
            var mockSMSResult = new SMSResult()
            {
                ExternalId = Guid.NewGuid().ToString(),
                MessageStatus = EFModel.Enumerators.MessageStatus.Delivered,
                ProviderId = Guid.NewGuid().ToString(),
                ReturnedMessage = "Success",
                TimeStamp = DateTime.Now
            };
            var mockISMSClientProxy = new Mock<ISMSClientProxy>();
            mockISMSClientProxy.Setup(m => m.SendSMSAsync(It.IsAny<long>(),
                                    It.IsAny<string>(),
                                    It.IsAny<string>(),
                                    It.IsAny<string>(),
                                    It.IsAny<DateTime?>())).Returns(Task.FromResult<SMSResult>(mockSMSResult));

            var mockIClientProviderFactory = new Mock<IClientProviderFactory>();
            mockIClientProviderFactory.Setup(m => m.CreateClient(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(mockISMSClientProxy.Object);

            var target = new ConsumeSMS(db, sQSsmsServices, databaseCommands, mockIClientProviderFactory.Object);
            await target.Execute();
        }
    }
}