using Aurora.Core.Data;
using Aurora.SMS.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services.AddMvc();

            var insuranceDbconnection = @"Server =.\SQL16; Database = InsuranceCore; Trusted_Connection = True;";
            var sMSDbconnection = @"Server =.\SQL16; Database = SMSDbCore; Trusted_Connection = True;";

            // This registration is used for the CurrentUserService class
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddDbContext<SMSDb>(options => options.UseSqlServer(sMSDbconnection));
            services.AddDbContext<Insurance.Data.InsuranceDb>(options => options.UseSqlServer(insuranceDbconnection));


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
                serviceScope.ServiceProvider.GetService<Insurance.Data.InsuranceDb>().Database.Migrate();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
