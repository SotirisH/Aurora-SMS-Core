using System;
using System.Collections.Generic;
using System.Text;

namespace Aurora.Insurance.EFModel
{
    public class Attachment
    {
        public Guid AttachmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        /// <summary>
        /// Url to aws S3 object
        /// </summary>
        public string Url { get; set; }
        public string MimeType { get; set; }
        public long ContentLength { get; set; }
    }
}
