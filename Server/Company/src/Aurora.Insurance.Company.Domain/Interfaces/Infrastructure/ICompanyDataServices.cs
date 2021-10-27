using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aurora.Insurance.Company.Domain.Interfaces.Infrastructure
{
    public interface ICompanyDataServices
    {
        Task<Models.Entities.Company> GetOne(string id);

        /// <summary>
        ///     Returns all the companies ordered by Description
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Models.Entities.Company>> GetAll();

        Task<Models.Entities.Company> CreateOne(Models.Entities.Company company);

        Task<Models.Entities.Company> UpdateOne(Models.Entities.Company company);

        Task DeleteOne(string id);
    }
}
