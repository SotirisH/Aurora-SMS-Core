using Aurora.Insurance.EFModel;
using Aurora.Insurance.Validation.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aurora.Insurance.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:49906") });
            builder.Services.AddTransient<IValidator<Company>, CompanyValidator>();

            await builder.Build().RunAsync();
        }
    }
}