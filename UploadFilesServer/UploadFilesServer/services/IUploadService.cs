using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UploadFilesServer.Models;

namespace UploadFilesServer.services
{
    interface IUploadService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
        Task<string> AddAudioMetaData(AudioMetaData audioMetaData);

    }
}
