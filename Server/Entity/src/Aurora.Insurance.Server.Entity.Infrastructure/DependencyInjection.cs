
using Aurora.Insurance.Server.Entity.Domain.Interfaces.Infrastructure;
using Aurora.Insurance.Server.Entity.Infrastructure.Persistence;
using Aurora.Insurance.Server.Entity.Infrastructure.Persistence.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.Insurance.Server.Entity.Infrastructure
{
    public static class DependencyInjection
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IOrganizationDataServices, OrganizationDataServices>();
            services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("LocalDbContext"),
                x => x.MigrationsAssembly("Aurora.Insurance.Server.Entity.Application")));
        }
    }
}
