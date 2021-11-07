using System.Threading;
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

        public async Task<Organization> CreateOne(NewOrganizationRequest organizationRequest,
            CancellationToken cancellationToken = default)
        {
            var broker = new Agent
            {
                LastName = organizationRequest.Title,
                EmailAddress = organizationRequest.EmailAddress,
                TaxId = organizationRequest.TaxId,
                IsBroker = true
            };
            _dbContext.Agents.Add(broker);
            var newOrganization = new Organization
            {
                BrokerId = broker.AgentId
            };
 
            _dbContext.Organizations.Add(newOrganization);
            broker.OrganizationId = newOrganization.OrganizationId;
           
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newOrganization;
        }
    }
}
