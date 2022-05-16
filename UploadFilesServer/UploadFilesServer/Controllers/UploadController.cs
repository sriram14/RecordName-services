using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UploadFilesServer.Models;
using UploadFilesServer.services;



namespace UploadFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        IUploadService uploadService;
        public UploadController(IConfiguration configuration)
        {
            this.uploadService = new UploadService(configuration) ?? throw new ArgumentNullException(nameof(uploadService));
        }
        [HttpPost(nameof(UploadFile))]
        public async Task<IActionResult> UploadFile([FromForm] AudioMetaData audioMetaData)
        {
            try
            {
                
                if (audioMetaData.file.Length > 0)
                {
                    var fileName = audioMetaData.file.FileName;
                    string blobName = await uploadService.UploadAsync(audioMetaData.file.OpenReadStream(), fileName, audioMetaData.file.ContentType);
                    IConfigurationSection connectionStrings = Startup.ConnectionStrings;
                    audioMetaData.blobUrl  =  String.Format(connectionStrings.GetSection("blobStorageUrl").Value + "recordvoice/" + blobName) ;
                    string result = await uploadService.AddAudioMetaData(audioMetaData);
                    return Ok(new { audioMetaData.blobUrl });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

       
    }
}
