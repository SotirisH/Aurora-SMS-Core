using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Aurora.SMS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .Build();
    }
}