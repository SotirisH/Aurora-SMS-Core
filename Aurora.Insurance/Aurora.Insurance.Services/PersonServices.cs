using Aurora.Insurance.Data;
using Aurora.Insurance.EFModel;
using Aurora.Insurance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aurora.Insurance.Services
{
    public class PersonServices : IPersonServices
    {
        private readonly InsuranceDb _db;

        /// <summary>
        ///     Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public PersonServices(InsuranceDb db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _db.Persons.OrderBy(m => m.RowVersion).ToListAsync();
        }

        public async Task<Person> CreateOne(Person person)
        {
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<Person> UpdateOne(Person person)
        {
            _db.Persons.Update(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task DeleteOne(int id)
        {
            var resourceToDelete = await GetOne(id);
            _db.Remove(resourceToDelete);
            await _db.SaveChangesAsync();
        }

        public async Task<Person> GetOne(int id)
        {
            return await _db.Persons.FindAsync(id);
        }
    }
}