using Aurora.Insurance.Data;
using Aurora.Insurance.EFModel;
using Aurora.Insurance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aurora.Insurance.Services
{
    public class AttachmentServices : IAttachmentServices
    {
        private readonly InsuranceDb _db;

        /// <summary>
        ///     Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public AttachmentServices(InsuranceDb db)
        {
            _db = db;
        }

        public Task<Attachment> CreateOne(Attachment attachment)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOne(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Attachment>> GetByIds(IList<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<Attachment> GetOne(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Attachment> UpdateOne(Attachment attachment)
        {
            throw new NotImplementedException();
        }
    }

 
}