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
            var target = new AWSServices(ConfigurationHelper.GetAWSConfiguration());
            var mockMessages = new List<SMSMessage>();
            var faker = new Bogus.Faker();
            for (int i = 0; i < 50; i++)
            {
                mockMessages.Add(new SMSMessage() {Id=i,
                                    MobileNumber = faker.Phone.PhoneNumber(),
                                    ProviderName ="TestSMS",
                                    Message=$"Test message with id: {i}"});
            }
            await target.PushMessagesAsync(mockMessages, Guid.NewGuid());
        }
    }
}