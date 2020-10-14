using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aurora.Insurance.Data;
using Aurora.Insurance.EFModel;
using Aurora.Insurance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aurora.Insurance.Services
{
    public class CompanyServices : ICompanyServices
    {
        private readonly InsuranceDb _db;

        /// <summary>
        ///     Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public CompanyServices(InsuranceDb db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            return await _db.Companies.OrderBy(m => m.Description).ToListAsync();
        }

        public async Task<Company> CreateOne(Company company)
        {
            await _db.Companies.AddAsync(company);
            await _db.SaveChangesAsync();

            return company;
        }

        public async Task<Company> UpdateOne(Company company)
        {
            _db.Companies.Update(company);
            await _db.SaveChangesAsync();
            return company;
        }

        public async Task DeleteOne(string id)
        {
            Company resourceToDelete = await GetOne(id);
            _db.Remove(resourceToDelete);
            await _db.SaveChangesAsync();
        }

        public async Task<Company> GetOne(string id)
        {
            return await _db.Companies.FindAsync(id);
        }
    }
}
