using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aurora.Insurance.Company.Domain.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Aurora.Insurance.Company.Infrastructure.Persistence.Operations
{
    public class CompanyDataServices : ICompanyDataServices
    {
        private readonly LocalDbContext _db;

        public CompanyDataServices(LocalDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Domain.Models.Entities.Company>> GetAll()
        {
            return await _db.Companies.OrderBy(m => m.Description).ToListAsync();
        }

        public async Task<Domain.Models.Entities.Company> CreateOne(Domain.Models.Entities.Company company)
        {
            await _db.Companies.AddAsync(company);
            await _db.SaveChangesAsync();

            return company;
        }

        public async Task<Domain.Models.Entities.Company> UpdateOne(Domain.Models.Entities.Company company)
        {
            _db.Companies.Update(company);
            await _db.SaveChangesAsync();
            return company;
        }

        public async Task DeleteOne(string id)
        {
            Domain.Models.Entities.Company resourceToDelete = await GetOne(id);
            _db.Remove(resourceToDelete);
            await _db.SaveChangesAsync();
        }

        public async Task<Domain.Models.Entities.Company> GetOne(string id)
        {
            return await _db.Companies.FindAsync(id);
        }
    }
}
