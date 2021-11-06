
using Aurora.Insurance.Server.Entity.Application.Services;
using Aurora.Insurance.Server.Entity.Domain.Interfaces.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.Insurance.Server.Entity.Application
{
    public static class DependencyInjection
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IOrganizationServices, OrganizationServices>();
        }
    }
}
