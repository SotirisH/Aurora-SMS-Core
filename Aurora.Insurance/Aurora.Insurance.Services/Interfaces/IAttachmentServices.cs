﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aurora.Insurance.EFModel;

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

        Task<Attachment> CreateOne(Attachment attachment,
            Stream file);

        Task<Attachment> UpdateOne(Attachment attachment);

        Task DeleteOne(Guid id);

        Task<IEnumerable<Attachment>> GetAll();
    }
}
