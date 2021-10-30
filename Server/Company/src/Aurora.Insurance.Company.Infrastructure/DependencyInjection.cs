using Aurora.Insurance.Company.Domain.Interfaces.Infrastructure;
using Aurora.Insurance.Company.Infrastructure.Persistence;
using Aurora.Insurance.Company.Infrastructure.Persistence.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.Insurance.Company.Infrastructure
{
    public static class DependencyInjection
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<ICompanyDataServices, CompanyDataServices>();
            services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("LocalDbContext"),
                x => x.MigrationsAssembly("Aurora.Insurance.Company.Infrastructure")));
        }
    }
}
