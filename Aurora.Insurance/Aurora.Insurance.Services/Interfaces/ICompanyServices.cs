using Aurora.Insurance.EFModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aurora.Insurance.Services.Interfaces
{
    public interface ICompanyServices
    {
        /// <summary>
        ///     Returns all the companies ordered by Description
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Company>> GetAll();
        Task<Company> CreateOne(Company company);

        Task<Company> UpdateOne(Company company);

        Task DeleteOne(string id);
    }
}
