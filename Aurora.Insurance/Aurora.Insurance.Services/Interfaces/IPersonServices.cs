using System.Collections.Generic;
using System.Threading.Tasks;
using Aurora.Insurance.EFModel;

namespace Aurora.Insurance.Services.Interfaces
{
    public interface IPersonServices
    {
        Task<Person> GetOne(int id);

        /// <summary>
        ///     Returns all the companies ordered by Description
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Person>> GetAll();

        Task<Person> CreateOne(Person person);

        Task<Person> UpdateOne(Person person);

        Task DeleteOne(int id);
    }
}
