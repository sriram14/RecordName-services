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
        public string token { get; set; }
        public string userId { get; set; }
        public string partnerId { get; set; }
        public string blobUrl { get; set; }
        public int fileSize { get; set; }
        public string fileType { get; set; }  
    }

    public class UserResponse
    {
        public bool isadmin { get; set; }
    }

    public class TokenReuest
    {
        public string token { get; set; }
        public string userid { get; set; }

    }
}
