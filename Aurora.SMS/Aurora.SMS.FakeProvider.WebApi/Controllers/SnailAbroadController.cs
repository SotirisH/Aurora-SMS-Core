
using Aurora.SMS.FakeProvider.WebApi;
using Aurora.SMS.FakeProvider.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;

namespace Aurora.SMS.FakeProvider.Controllers
{
    /// <summary>
    /// The SMS GateWay.
    /// </summary>
    /// <remarks>
    /// For demostration purposes the gateway is slow and creates random errors
    /// WEB API 2 EXPLORING PARAMETER BINDING
    /// https://damienbod.com/2014/08/22/web-api-2-exploring-parameter-binding/
    /// </remarks>
    public class SnailAbroadController : Controller
    {
        private readonly CreditCounterSnail _creditCounter;
        public SnailAbroadController(CreditCounterSnail creditCounter)
        {
            _creditCounter = creditCounter;
        }


        /// <summary>
        /// The credit decrease is created as public function in order to be able to Moq it
        /// </summary>
        [NonAction]
        public virtual void DecreaseCredit()
        {
            _creditCounter.Credits--;
        }
        [NonAction]
        public virtual void ApplyDelay()
        {
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Default get action
        /// http://localhost:8080/api/SnailAbroad/EchoTest?echo=sdsd
        /// </summary>
        /// <param name="echo"></param>
        /// <returns></returns>
        public IActionResult EchoTest(string echo = "Service Is OK!")
        {
            return Ok(echo);
        }

        /// <summary>
        /// Sends an SMS to a mobile phone
        /// </summary>
        /// <param name="username">Login user name</param>
        /// <param name="password"></param>
        /// <param name="message">The body of the message</param>
        /// <param name="mobileNumber">The mobile number that will be sent</param>
        /// <param name="messageExternalId">The external ID will be returned with the result. It is used to track the incoming message</param>
        /// <returns></returns>
        /// <remarks>The [FromBody] is needed to read the json content from the body of the message</remarks>
        [HttpPost]
        public IActionResult SendSMS([FromBody] Models.SmsRequest smsRequest)
        {
            return Ok(SendSmsHelper(smsRequest));
        }

        /// <summary>
        /// This method simulates resending an SMS message with a given Id
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="messageId"></param>
        /// <param name="messageExternalId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ReSendSMS(Models.SmsRequest smsRequest)
        {
            throw new NotImplementedException();
        }

     
        public SMSResult GetMessageStatus(Guid smsId)
        {
            throw new NotImplementedException();
        }

        
        public int GetAvailableCredits(string username,
                 string password)
        {
            ApplyDelay();
            // Remove one credit from the application variable
            return _creditCounter.Credits;
        }

        private SMSResult SendSmsHelper(Models.SmsRequest smsRequest)
        {
            Bogus.Faker faker = new Bogus.Faker();
            SMSResult result = new SMSResult() { Id = Guid.NewGuid(), ExternalId = smsRequest.messageExternalId };
            //try
            //{
            ApplyDelay();
            // simulate a random result behaviour
            // 50% to be ok
            if (faker.Random.Number(100) < 50)
            {
                result.Status = MessageStatus.OK;
            }
            else
            {
                // exclude ok
                result.Status = faker.Random.Enum(MessageStatus.OK);
            }

            switch (result.Status)
            {
                case MessageStatus.Error:
                    result.ReturnedMessage = faker.System.Exception().Message;
                    break;
                case MessageStatus.InvalidCredentials:
                    result.ReturnedMessage = "Your credentials are invalid!";
                    break;
                case MessageStatus.InvalidNumber:
                    result.ReturnedMessage = "This mobile number is invalid!";
                    break;
                case MessageStatus.MessageTooLong:
                    result.ReturnedMessage = "This message is too long!";
                    break;
                case MessageStatus.NotEnoughCredits:
                    result.ReturnedMessage = "NotEnoughCredits";
                    break;
                case MessageStatus.OK:
                    DecreaseCredit();
                    break;
                case MessageStatus.Pending:
                    result.ReturnedMessage = "Pending....";
                    break;
            }
            result.TimeStamp = DateTime.Now;
            return result;
        }

    }



}
