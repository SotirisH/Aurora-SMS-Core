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

        /// <summary>
        /// The receipt handle is the identifier you must provide when deleting the message
        /// </summary>
        public string ReceiptHandle { get; set; }

        /// <summary>
        /// Contains the names of the attributes
        /// that are embedded into the SMS message
        /// </summary>
        public class SMSMessageAttributes
        {
            public const string IdName = "Id";
            public const string ProviderName = "ProviderName";
            public const string MobileNumberName = "MobileNumber";
        }
    }
}