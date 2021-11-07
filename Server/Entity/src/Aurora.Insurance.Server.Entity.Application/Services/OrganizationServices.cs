using System.Threading;
using System.Threading.Tasks;
using Aurora.Insurance.Server.Entity.Domain.Interfaces.Application;
using Aurora.Insurance.Server.Entity.Domain.Interfaces.Infrastructure;
using Aurora.Insurance.Server.Entity.Domain.Models.Dtos;
using Aurora.Insurance.Server.Entity.Domain.Models.Entities;
using AutoMapper;

namespace Aurora.Insurance.Server.Entity.Application.Services
{
    public class OrganizationServices : IOrganizationServices
    {
        private readonly IOrganizationDataServices _organizationDataServices;
        private readonly IMapper _mapper;

        public OrganizationServices(IMapper mapper,
            IOrganizationDataServices organizationDataServices)
        {
            _organizationDataServices = organizationDataServices;
            _mapper = mapper;
        }

        public async Task<NewOrganizationResponse> CreateOne(NewOrganizationRequest organizationRequest,
            CancellationToken cancellationToken = default)
        {
            Organization result= await _organizationDataServices.CreateOne(organizationRequest,cancellationToken);
            return _mapper.Map<NewOrganizationResponse>(result);
        }
    }
}
