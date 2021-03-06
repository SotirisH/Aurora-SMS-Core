using Aurora.Core.Aws.S3;
using Aurora.Core.Data;
using Aurora.Insurance.Data;
using Aurora.Insurance.Services;
using Aurora.Insurance.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aurora.Insurance.WebAPI
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddControllers();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IS3BucketStorageClient, S3BucketStorageClient>();

            services.AddScoped<ICompanyServices, CompanyServices>();
            services.AddScoped<IPersonServices, PersonServices>();
            services.AddScoped<IAttachmentServices, AttachmentServices>();


            services.AddDbContext<InsuranceDb>(options => options.UseSqlServer(Configuration.GetConnectionString("InsuranceDbConnection"),
                x => x.MigrationsAssembly("Aurora.Insurance.Data")));
            
           
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        

    }
}
