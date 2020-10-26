using System;
using System.Net.Http;
using System.Threading.Tasks;
using Aurora.Insurance.EFModel;
using Aurora.Insurance.Validation.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.Insurance.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            //In order to authenticate to IS4:
            // https://github.com/dotnet/aspnetcore/issues/21327
            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Local", options.ProviderOptions);
            });
            
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("http://localhost:49906")
            });
            builder.Services.AddTransient<IValidator<Company>, CompanyValidator>();

            await builder.Build().RunAsync();
        }
    }
}
