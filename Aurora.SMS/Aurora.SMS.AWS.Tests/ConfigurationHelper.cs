using Amazon.Extensions.NETCore.Setup;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Aurora.SMS.AWS.Tests
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Gets the default configuration for testing.
        /// The profile is set in the appsettings.json
        /// </summary>
        /// <returns></returns>
        public static AWSOptions GetAWSConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json"); //Microsoft.Extensions.Configuration.Json
            var configuration = builder.Build();
            return configuration.GetAWSOptions();
        }
    }
}