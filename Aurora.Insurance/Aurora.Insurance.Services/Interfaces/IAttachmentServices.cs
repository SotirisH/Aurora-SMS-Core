using Aurora.Insurance.EFModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aurora.Insurance.Services.Interfaces
{
    public interface IAttachmentServices
    {
        Task<Attachment> GetOne(Guid id);

        /// <summary>
        ///     Returns all the companies ordered by Description
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Attachment>> GetByIds(IList<Guid> ids);

        Task<Attachment> CreateOne(Attachment attachment);

        Task<Attachment> UpdateOne(Attachment attachment);

        Task DeleteOne(Guid id);
    }
}
