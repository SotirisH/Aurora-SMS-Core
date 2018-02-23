using Aurora.Core.Data;
using Aurora.SMS.Data;
using AutoMapper;
using FluentValidation.AspNetCore;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Aurora.SMS.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddFluentValidation(fv =>
                {
                    fv.ValidatorFactoryType = typeof(AttributedValidatorFactory);
                })
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddAutoMapper();
            var insuranceDbconnection = @"Server =.\SQL16; Database = InsuranceCore; Trusted_Connection = True;";
            var sMSDbconnection = @"Server =.\SQL16; Database = SMSDbCore; Trusted_Connection = True;";

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddDbContext<SMSDb>(options => options.UseSqlServer(sMSDbconnection));
            services.AddDbContext<Insurance.Data.InsuranceDb>(options => options.UseSqlServer(insuranceDbconnection));
            services.AddTransient<Service.ITemplateServices, Service.TemplateServices>();
            services.AddTransient<Service.ITemplateFieldServices, Service.TemplateFieldServices>();
            services.AddTransient<Insurance.Services.ICompanyServices, Insurance.Services.CompanyServices>();
            services.AddTransient<Insurance.Services.IContractServices, Insurance.Services.ContractServices>();
            services.AddTransient<Service.ISMSServices, Service.SMSServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes => routes.MapRoute("default", "{controller}/{action}/{id?}"));
        }
    }
}
