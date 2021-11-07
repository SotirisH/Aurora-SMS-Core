using System.Threading;
using System.Threading.Tasks;
using Aurora.Insurance.Server.Entity.Domain.Models.Dtos;
using Aurora.Insurance.Server.Entity.Domain.Models.Entities;

namespace Aurora.Insurance.Server.Entity.Domain.Interfaces.Application
{
    public interface IOrganizationServices
    {
        /// <summary>
        ///     Creates a new organization, a broker and links them
        /// </summary>
        /// <param name="organizationRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NewOrganizationResponse> CreateOne(NewOrganizationRequest organizationRequest,
            CancellationToken cancellationToken = default);
    }
}
