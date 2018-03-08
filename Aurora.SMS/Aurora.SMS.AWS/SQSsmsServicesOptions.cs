using Amazon.Extensions.NETCore.Setup;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurora.SMS.AWS
{
    public class SQSsmsServicesOptions
    {
        public AWSOptions AWSOptions { get; set; }
        public string QueueName { get; set; }
    }
}
