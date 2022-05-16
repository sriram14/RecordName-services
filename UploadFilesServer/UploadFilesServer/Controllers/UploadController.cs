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
using UploadFilesServer.Common;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;

namespace UploadFilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        IUploadService uploadService;
        public UploadController(IConfiguration configuration,IUtility utility)
        {
            this.uploadService = new UploadService(configuration, utility) ?? throw new ArgumentNullException(nameof(uploadService));
        }
        [HttpPost(nameof(UploadFile))]
        public async Task<IActionResult> UploadFile([FromForm] AudioMetaData audioMetaData)
        {
            try
            {
                TokenReuest tokenReuest = new TokenReuest();
                tokenReuest.userid = audioMetaData.userId;
                tokenReuest.token = audioMetaData.token;
                HttpClient httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(tokenReuest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://ryn-partnerapp.azure-api.net/api/User/ValidateToken", content);
                if(response.IsSuccessStatusCode)
                {
                    if (audioMetaData.file.Length > 0)
                    {
                        var fileName = "file"+ Guid.NewGuid().ToString();
                        string blobName = await uploadService.UploadAsync(audioMetaData.file.OpenReadStream(), fileName, audioMetaData.file.ContentType);
                        IConfigurationSection connectionStrings = Startup.ConnectionStrings;
                        audioMetaData.blobUrl = String.Format(connectionStrings.GetSection("blobStorageUrl").Value + "recordvoice/" + blobName);
                        string result = await uploadService.AddAudioMetaData(audioMetaData);
                        return Ok(new { audioMetaData.blobUrl });
                    }
                    else
                    {
                        return BadRequest();
                    }

                }
                else
                {
                     return Unauthorized();

                }              
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost(nameof(UploadOthersFile))]
        public async Task<IActionResult> UploadOthersFile(IFormFile file,[FromQuery] string token, [FromQuery] string createruserId, [FromQuery] string othersuserId, [FromQuery] string partnerId)
        {
            try
            {
                TokenReuest tokenReuest = new TokenReuest();
                tokenReuest.userid = createruserId;
                tokenReuest.token = token;
                HttpClient httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(tokenReuest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://ryn-partnerapp.azure-api.net/api/User/ValidateToken", content);
                if (response.IsSuccessStatusCode)
                {

                    if (file.Length > 0)
                    {
                        var fileName = "file" + Guid.NewGuid().ToString();
                        string blobName = await uploadService.UploadAsync(file.OpenReadStream(), fileName, file.ContentType);
                        IConfigurationSection connectionStrings = Startup.ConnectionStrings;
                        AudioMetaData audioMetaData = new AudioMetaData();
                        audioMetaData.userId = othersuserId;
                        audioMetaData.partnerId = partnerId;
                        audioMetaData.fileSize = (int)file.Length;
                        string extension = Path.GetExtension(fileName);
                        audioMetaData.fileType = extension;
                        audioMetaData.blobUrl = String.Format(connectionStrings.GetSection("blobStorageUrl").Value + "recordvoice/" + blobName);

                        bool isAdmin = uploadService.isAdminUser(createruserId);
                        if (isAdmin)
                        {
                            string result = await uploadService.AddAudioMetaData(audioMetaData, createruserId);
                            return Ok(new { audioMetaData.blobUrl });
                        }
                        else
                        {
                            return Unauthorized();
                        }

                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private IActionResult Ok(Func<UnauthorizedResult> unauthorized)
        {
            throw new NotImplementedException();
        }
    


    }
}
