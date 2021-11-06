using System.Threading.Tasks;
using Aurora.Insurance.Server.Entity.Domain.Interfaces.Application;
using Aurora.Insurance.Server.Entity.Domain.Interfaces.Infrastructure;
using Aurora.Insurance.Server.Entity.Domain.Models.Dtos;
using Aurora.Insurance.Server.Entity.Domain.Models.Entities;

namespace Aurora.Insurance.Server.Entity.Application.Services
{
    public class OrganizationServices : IOrganizationServices
    {
        private readonly IOrganizationDataServices _organizationDataServices;

        public OrganizationServices(IOrganizationDataServices organizationDataServices)
        {
            _organizationDataServices = organizationDataServices;
        }

        public async Task<Organization> CreateOne(NewOrganizationRequest organizationRequest)
        {
            return await _organizationDataServices.CreateOne(organizationRequest);
        }
    }
}
