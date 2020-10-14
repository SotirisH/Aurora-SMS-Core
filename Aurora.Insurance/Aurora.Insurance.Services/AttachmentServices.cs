using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aurora.Core.Aws.S3;
using Aurora.Insurance.Data;
using Aurora.Insurance.EFModel;
using Aurora.Insurance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aurora.Insurance.Services
{
    public class AttachmentServices : IAttachmentServices
    {
        private readonly IS3BucketStorageClient _bucketStorageClient;
        private readonly InsuranceDb _db;

        /// <summary>
        ///     Primary constructor.
        /// </summary>
        /// <param name="db">It is fine to pass the dbcontext here</param>
        public AttachmentServices(InsuranceDb db,
            IS3BucketStorageClient bucketStorageClient)
        {
            _db = db;
            _bucketStorageClient = bucketStorageClient;
        }

        public async Task<Attachment> CreateOne(Attachment attachment,
            Stream file)
        {
            attachment.AttachmentId = Guid.NewGuid();
            string url = await _bucketStorageClient.Upload(new UploadRequest
            {
                Content = file,
                MimeType = attachment.MimeType,
                Path = "attachments/customer/",
                FileName = $"{attachment.AttachmentId}-{attachment.FileName}"
            });
            attachment.Url = url;
            _db.Attachments.Add(attachment);
            await _db.SaveChangesAsync();
            return attachment;
        }

        public Task DeleteOne(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Attachment>> GetAll()
        {
            return await _db.Attachments.ToListAsync();
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
