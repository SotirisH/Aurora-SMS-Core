using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aurora.Core.Aws.S3
{
    public interface IS3BucketStorageClient
    {
        /// <summary>
        /// Uploads an object into the s3
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> Upload(UploadRequest request, CancellationToken cancellationToken = default);
    }
}
