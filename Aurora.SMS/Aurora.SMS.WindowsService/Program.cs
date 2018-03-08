using Amazon.Extensions.NETCore.Setup;
using Aurora.SMS.AWS;
using Aurora.SMS.Data;
using Aurora.SMS.Providers;
using Aurora.SMS.Worker;
using Aurora.SMS.Worker.Consumer;
using Aurora.SMS.Worker.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.ServiceProcess;

namespace Aurora.SMS.WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SQSConsumerService()
            };
            ServiceBase.Run(ServicesToRun);
            Console.WriteLine("Service registered!");
            Console.ReadKey();

        }
    }

    public partial class SQSConsumerService : ServiceBase
    {
        private readonly JobScheduler _jobScheduler;
        public SQSConsumerService()
        {
            SQSsmsServicesOptions sQSsmsServicesOptions = new SQSsmsServicesOptions();
            sQSsmsServicesOptions.AWSOptions = GetAWSConfiguration();
            sQSsmsServicesOptions.QueueName = "SMS";
            ISQSsmsServices sqs = new SQSsmsServices(sQSsmsServicesOptions);

            var sMSDbconnection = @"Server =SOTO-LAPTOP\SQLEXPRESS; Database = SMSDbCore; Trusted_Connection = True;";
            var dbOptions = new DbContextOptionsBuilder<SMSDb>()
                 .UseSqlServer(sMSDbconnection)
                 .Options;
            var windowsUserService = new WindowsCurrentUserService();
            var db = new SMSDb(dbOptions, windowsUserService);
            IConsumerSMS consumer = new ConsumeSMS(db, sqs,new DatabaseCommands(db),new ClientProviderFactory());
            _jobScheduler = new JobScheduler(sqs, consumer);
        }
        protected override void OnStart(string[] args)
        {
            _jobScheduler.Start();  
        }

        protected override void OnStop()
        {
            _jobScheduler.Stop();
        }

        /// <summary>
        /// Gets the default configuration for testing.
        /// The profile is set in the appsettings.json
        /// </summary>
        /// <returns></returns>
        private  AWSOptions GetAWSConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json"); //Microsoft.Extensions.Configuration.Json
            var configuration = builder.Build();
            return configuration.GetAWSOptions();
        }
    }
}
