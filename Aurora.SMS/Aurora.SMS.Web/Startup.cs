using Amazon.SQS;
using Aurora.Core.Data;
using Aurora.SMS.AWS;
using Aurora.SMS.Data;
using AutoMapper;
using FluentValidation.AspNetCore;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Aurora.SMS.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            //https://github.com/aspnet/Mvc/issues/4842  MVC now serializes JSON with camel case names by default
            services.AddMvc()
                 .AddFluentValidation(fv =>
                 {
                     fv.ValidatorFactoryType = typeof(AttributedValidatorFactory);
                 })
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;

            services.AddAutoMapper();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.Cookie.HttpOnly = true;
            });

            var insuranceDbconnection = @"Server =.\SQL16; Database = InsuranceCore; Trusted_Connection = True;";
            var sMSDbconnection = @"Server =.\SQL16; Database = SMSDbCore; Trusted_Connection = True;";

            // This registration is used for the CurrentUserService class
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<CookieHelper>();
            services.AddSingleton<SessionHelper>();

            services.AddDbContext<SMSDb>(options => options.UseSqlServer(sMSDbconnection));
            services.AddDbContext<Insurance.Data.InsuranceDb>(options => options.UseSqlServer(insuranceDbconnection));

            //AWS
            // More details: https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-netcore.html
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSQS>();

            services.AddTransient<Service.ITemplateServices, Service.TemplateServices>();
            services.AddTransient<Service.ITemplateFieldServices, Service.TemplateFieldServices>();
            services.AddTransient<Insurance.Services.ICompanyServices, Insurance.Services.CompanyServices>();
            services.AddTransient<Insurance.Services.IContractServices, Insurance.Services.ContractServices>();
            services.AddTransient<Service.ISMSServices, Service.SMSServices>();
            services.AddTransient<IAWSServices, AWSServices>();

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Migrations take place here
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<SMSDb>().Database.Migrate();
                var insuranceDb = serviceScope.ServiceProvider.GetService<Insurance.Data.InsuranceDb>();
                insuranceDb.Database.Migrate();
                var seed = new DbInsuranceSeed();
                seed.Seed(insuranceDb);
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=AuroraSMS}/{action=Index}/{id?}");
            });

        }
    }
}
