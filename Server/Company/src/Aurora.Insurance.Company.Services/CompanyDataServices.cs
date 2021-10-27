using System.Collections.Generic;
using System.Threading.Tasks;
using Aurora.Insurance.Company.Domain.Interfaces.Application;
using Aurora.Insurance.Company.Domain.Interfaces.Infrastructure;

namespace Aurora.Insurance.Company.Services
{
    public class CompanyServices : ICompanyServices
    {
        private readonly ICompanyDataServices _companyDataServices;

        public CompanyServices(ICompanyDataServices companyDataServices)
        {
            _companyDataServices = companyDataServices;
        }

        public async Task<IEnumerable<Domain.Models.Entities.Company>> GetAll()
        {
            return await _companyDataServices.GetAll();
        }

        public async Task<Domain.Models.Entities.Company> CreateOne(Domain.Models.Entities.Company company)
        {
            return await _companyDataServices.CreateOne(company);
        }

        public async Task<Domain.Models.Entities.Company> UpdateOne(Domain.Models.Entities.Company company)
        {
            return await _companyDataServices.UpdateOne(company);
        }

        public async Task DeleteOne(string id)
        {
            await _companyDataServices.DeleteOne(id);
        }

        public async Task<Domain.Models.Entities.Company> GetOne(string id)
        {
            return await _companyDataServices.GetOne(id);
        }
    }
}
