using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFilesServer.Models
{
    public class AudioMetaData
    {
        public IFormFile file { get; set; }
        public string userId { get; set; }
        public string partnerId { get; set; }
        public string blobUrl { get; set; }
        public int fileSize { get; set; }
        public string fileType { get; set; }  
    }
}
