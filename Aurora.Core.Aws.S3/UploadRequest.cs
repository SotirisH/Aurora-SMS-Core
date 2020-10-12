using System.IO;

namespace Aurora.Core.Aws.S3
{
    public class UploadRequest
    {
        /// <summary>
        /// Folder in the bucket where the object will be stored
        /// </summary>
        public string Path { get; set; }

        public string BucketName { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; }

        public Stream Content { get; set; }
        public string ContentType { get; set; }
    }
}