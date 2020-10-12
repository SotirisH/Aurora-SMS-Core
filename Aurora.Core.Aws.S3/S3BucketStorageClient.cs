using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Aurora.Core.Aws.S3
{
    public class S3BucketStorageClient
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AWSOptions _aWSOptions;
        private const string bucketName = "aurora-insurance";

        public S3BucketStorageClient(IConfiguration configuration)
        {
            _aWSOptions = configuration.GetAWSOptions();
            var options = _aWSOptions;
            _s3Client = options.CreateServiceClient<IAmazonS3>();
        }

        public async Task<string> Upload(UploadRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var utility = new TransferUtility(_s3Client))
                {
                    var transferUtilityUploadRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = bucketName,
                        Key = GetPath(request),
                        InputStream = request.Content,
                        AutoResetStreamPosition = true
                    };

                    if (!string.IsNullOrEmpty(request.ContentType))
                    {
                        request.ContentType = request.ContentType;
                    }

                    await utility.UploadAsync(transferUtilityUploadRequest, cancellationToken).ConfigureAwait(false);

                    return BuildFileUrl(request);
                }
            }
            catch (Exception exception)
            {
                if (!(exception is AmazonS3Exception))
                {
                    throw;
                }

                throw GetHandledAwsS3Exception((AmazonS3Exception)exception);
            }
        }

        private Exception GetHandledAwsS3Exception(AmazonS3Exception s3Exception)
        {
            string errorCode = s3Exception.ErrorCode;

            var s3ErrorDetails = new
            {
                s3Exception.AmazonCloudFrontId,
                s3Exception.AmazonId2,
                s3Exception.RequestId,
                s3Exception.StatusCode,
                s3Exception.ErrorCode,
                s3Exception.ErrorType
            };

            if (errorCode != null && (errorCode.Equals("InvalidAccessKeyId") || errorCode.Equals("InvalidSecurity")))
            {
                const string errorMessage = "Invalid AWS credentials, please check the provided AWS Credentials.";
                throw new Exception(errorMessage, s3Exception);
            }

            return s3Exception;
        }

        private string BuildFileUrl(UploadRequest request)
        {
            var hostUri = new Uri(GetS3HostUrlForRegion(_s3Client.Config.RegionEndpoint));
            var builder = new UriBuilder(hostUri);
            builder.Path = !string.IsNullOrWhiteSpace(request.Path) ? Path.Combine(request.Path, request.FileName) : request.FileName;
            builder.Path = Path.Combine(bucketName, builder.Path);
            return builder.ToString();
        }

        private string GetPath(UploadRequest request)
        {
            string path = !string.IsNullOrWhiteSpace(request.Path) ? Path.Combine(request.Path, request.FileName) : request.FileName;
            var builder = new UriBuilder
            {
                Path = path
            };
            return builder.Path;
        }

        private static string GetS3HostUrlForRegion(RegionEndpoint region)
        {
            return $"https://s3-{region.SystemName}.amazonaws.com";
        }
    }
}