using System;
using System.Threading;
using Aurora.SMS.FakeProvider.Models;
using Aurora.SMS.FakeProvider.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.SMS.FakeProvider.Controllers
{
    /// <summary>
    ///     The SMS GateWay.
    /// </summary>
    /// <remarks>
    ///     For demostration purposes the gateway is slow and creates random errors
    /// </remarks>
    public class FlashSmsController : Controller
    {
        /// <summary>
        ///     The credit decrease is created as public function in order to be able to Moq it
        /// </summary>
        public virtual void DecreaseCredit()
        {
            throw new NotImplementedException();
        }

        public virtual void ApplyDelay()
        {
            Thread.Sleep(100);
        }

        /// <summary>
        ///     Test method
        ///     http://localhost:8080/api/SnailAbroad/EchoTest?echo=sdsd
        /// </summary>
        /// <param name="echo"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EchoTest(string echo)
        {
            return Ok(echo);
        }

        /// <summary>
        ///     Test method for post
        /// </summary>
        /// <param name="echo"></param>
        /// <returns>
        /// </returns>
        /// <remarks>
        ///     After many, many, many hours i discovered this
        ///     http://stackoverflow.com/questions/24625303/why-do-we-have-to-specify-frombody-and-fromuri-in-asp-net-web-api
        ///     In few words: without [FromBody]
        ///     If you use HttpClient.PostAsync the method expects the parameters to be passed from URI
        /// </remarks>
        [HttpPost]
        public string TestPost([FromBody] string echo)
        {
            return echo + " Hello";
        }

        /// <summary>
        ///     Sends an SMS to a mobile phone
        /// </summary>
        /// <param name="username">Login user name</param>
        /// <param name="password"></param>
        /// <param name="message">The body of the message</param>
        /// <param name="mobileNumber">The mobile number that will be sent</param>
        /// <param name="messageExternalId">The external ID will be returned with the result. It is used to track the incoming message</param>
        /// <returns></returns>
        [HttpPost]
        public SMSResult SendSMS(SmsRequest smsRequest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     This method simulates resending an SMS message with a given Id
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="messageId"></param>
        /// <param name="messageExternalId"></param>
        /// <returns></returns>
        [HttpPost]
        public SMSResult ReSendSMS(SmsRequest smsRequest)
        {
            // Suppose the SMS message is retrieved from the providers DB

            return SendSMS(smsRequest);
        }

        [HttpGet]
        public SMSResult GetMessageStatus(Guid smsId)
        {
            throw new NotImplementedException();
        }


        [HttpGet]
        public int GetAvailableCredits(string username,
            string password)
        {
            return 0;
        }
    }
}
