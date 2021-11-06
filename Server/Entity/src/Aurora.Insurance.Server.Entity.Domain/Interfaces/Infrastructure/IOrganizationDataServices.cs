using System.Threading.Tasks;
using Aurora.Insurance.Server.Entity.Domain.Models.Dtos;
using Aurora.Insurance.Server.Entity.Domain.Models.Entities;

namespace Aurora.Insurance.Server.Entity.Domain.Interfaces.Infrastructure
{
    public interface IOrganizationDataServices
    {
        /// <summary>
        ///     Creates a new organization, a broker and links them
        /// </summary>
        /// <param name="organizationRequest"></param>
        /// <returns></returns>
        Task<Organization> CreateOne(NewOrganizationRequest organizationRequest);
    }
}
