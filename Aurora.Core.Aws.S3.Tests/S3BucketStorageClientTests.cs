using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Aurora.Core.Aws.S3.Tests
{
    public class S3BucketStorageClientTests
    {
        [Fact]
        public async Task UploadFile()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var target = new S3BucketStorageClient(config);
            string path = Path.Combine(Environment.CurrentDirectory, "TextFileSample.txt");
            string text = System.IO.File.ReadAllText(path);
            string result;
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                var s3Request = new UploadRequest();

                s3Request.Path = "attachments/customer/";
                s3Request.FileName = $"{Guid.NewGuid()}-TextFileSample.txt";
                s3Request.Content = fileStream;
                result = await target.Upload(s3Request);
            }
        }
    }
}
