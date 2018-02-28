using Aurora.SMS.AWS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Aurora.SMS.AWS.Tests
{
    public class AWSServicesTest
    {
        [Fact]
        public async Task PushMessagesAsyncTest()
        {
            var target = new SQSsmsServices(ConfigurationHelper.GetAWSConfiguration(), "SMS");
            var mockMessages = new List<SMSMessage>();
            var faker = new Bogus.Faker();
            for (int i = 0; i < 50; i++)
            {
                mockMessages.Add(new SMSMessage()
                {
                    Id = i,
                    MobileNumber = faker.Phone.PhoneNumber(),
                    ProviderName = "TestSMS",
                    Message = $"Test message with id: {i}"
                });
            }
            await target.PushMessagesAsync(mockMessages, Guid.NewGuid());
        }

        [Fact]
        public void GetVisibilityTimeOutTest()
        {
            var target = new SQSsmsServices(ConfigurationHelper.GetAWSConfiguration(), "SMS");
            Assert.Equal(30, target.GetVisibilityTimeOut().Result);
        }
    }
}