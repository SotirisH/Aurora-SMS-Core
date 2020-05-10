using System.Collections.Generic;
using System.Linq;
using Aurora.Core.Data;
using Aurora.Insurance.Data;
using Aurora.Insurance.EFModel;

namespace Aurora.Insurance.Services
{
    public interface ICompanyServices
    {
        /// <summary>
        ///     Returns all the companies ordered by Description
        /// </summary>
        /// <returns></returns>
        IEnumerable<Company> GetAll();
    }

    public class CompanyServices : DbServiceBase<InsuranceDb>, ICompanyServices
    {
        /// <summary>
        ///     Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public CompanyServices(InsuranceDb db) : base(db)
        {
        }

        public IEnumerable<Company> GetAll()
        {
            return DbContext.Companies.OrderBy(m => m.Description).ToArray();
        }
    }
}
