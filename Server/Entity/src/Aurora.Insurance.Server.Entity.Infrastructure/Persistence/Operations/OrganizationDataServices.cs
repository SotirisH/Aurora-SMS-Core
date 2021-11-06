using System.Threading.Tasks;
using Aurora.Insurance.Server.Entity.Domain.Interfaces.Infrastructure;
using Aurora.Insurance.Server.Entity.Domain.Models.Dtos;
using Aurora.Insurance.Server.Entity.Domain.Models.Entities;

namespace Aurora.Insurance.Server.Entity.Infrastructure.Persistence.Operations
{
    public class OrganizationDataServices : IOrganizationDataServices
    {
        private readonly LocalDbContext _dbContext;

        public OrganizationDataServices(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Organization> CreateOne(NewOrganizationRequest organizationRequest)
        {
            var broker = new Agent
            {
                LastName = organizationRequest.Title,
                EmailAddress = organizationRequest.EmailAddress,
                TaxId = organizationRequest.TaxId
            };
            _dbContext.Agents.Add(broker);
            var newOrganization = new Organization
            {
                Broker = broker
            };
            _dbContext.Organizations.Add(newOrganization);
            await _dbContext.SaveChangesAsync();
            return newOrganization;
        }
    }
}
