using System;
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
        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
