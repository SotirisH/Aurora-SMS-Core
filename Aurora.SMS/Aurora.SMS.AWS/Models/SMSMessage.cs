using System;
using System.Collections.Generic;
using System.Text;

namespace Aurora.SMS.AWS.Models
{
    /// <summary>
    /// Represents an SMS message to stored in the SQS
    /// </summary>
    public class SMSMessage
    {
        public long Id { get; set; }
        public string ProviderName { get; set; }
        public string Message { get; set; }
        public string MobileNumber { get; set; }
    }
}
