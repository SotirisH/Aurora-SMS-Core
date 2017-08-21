﻿using Aurora.SMS.EFModel.Enumerators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.SMS.Providers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Example:https://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client
    /// </remarks>
    class SnailAbroadProxy : ISMSClientProxy
    {

        enum AuroraFakeMessageStatus
        {
            OK,
            Pending,
            InvalidNumber,
            InvalidCredentials,
            NotEnoughCredits,
            MessageTooLong,
            Error
        }

        class AuroraFakeSMSResult
        {
            /// <summary>
            /// Id generated by the server for this message
            /// </summary>
            public Guid Id { get; set; }
            /// <summary>
            /// The message returned by the server.
            /// If everything is ok then an empty string is retirned
            /// </summary>
            public string ReturnedMessage { get; set; }
            public DateTime TimeStamp { get; set; }
            public AuroraFakeMessageStatus Status { get; set; }
            /// <summary>
            /// The external ID that is passed by the client and returned back to it
            /// in order to make easy to track the message and correlate it with the Guid ID
            /// </summary>
            public string ExternalId { get; set; }
        }


        private  HttpClient client = new HttpClient();

        private readonly string _userName,_password;

        public SnailAbroadProxy(string userName,
                                    string password)
        {
            _userName = userName;
            _password = password;
            client.BaseAddress = new Uri("http://localhost:8080");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
 

        public async Task<string> GetAvailableCreditsAsync()
        {
            string path = @"snailabroad/GetAvailableCredits?username=" + _userName + "&password=" + _password;
            HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            {
                return "Error!" + response.ReasonPhrase;
            }
        }

        public async Task<SMSResult> SendSMSAsync(long smsMessageId, 
                                    string mobileNumber, 
                                    string smsMessage, 
                                    string sender, 
                                    DateTime? scheduledDate)
        {
            string path = @"/snailabroad/SendSMS";
            // Create Parameters obj
            var param = new
            {
                username= _userName,
                password= _password,
                message= smsMessage,
                mobileNumber= mobileNumber,
                messageExternalId= smsMessageId.ToString()
            };

            //var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
            //var r = client.PostAsync(path, content).Result;
            //http://stackoverflow.com/questions/10343632/httpclient-getasync-never-returns-when-using-await-async
            //OLD 4.5: HttpResponseMessage response = await client.PostAsJsonAsync(path, param).ConfigureAwait(false);

            //https://stackoverflow.com/questions/37750451/send-http-post-message-in-asp-net-core-using-httpclient-postasjsonasync
            var jsonInString = JsonConvert.SerializeObject(param);
            HttpResponseMessage response = await client.PostAsync(path, new StringContent(jsonInString, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                // Because each provider could return a different response object the Deserialization needs to be manualy
                var result= JsonConvert.DeserializeObject<AuroraFakeSMSResult>(responseBody);
                return new SMSResult()
                {
                    ExternalId = result.ExternalId,
                    MessageStatus=(result.Status==AuroraFakeMessageStatus.OK? MessageStatus.Delivered:MessageStatus.Error),
                    ProviderId=result.Id.ToString(),
                    ReturnedMessage=result.ReturnedMessage,
                    TimeStamp=result.TimeStamp
                };

            }
            return null;

        }
    }
}
