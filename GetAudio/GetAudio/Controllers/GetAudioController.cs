using GetAudio.DataAccess;
using GetAudio.Models;
using GetAudio.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetAudio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AudioPolicy")]
    public class GetAudioController : ControllerBase
    {
        private readonly ILogger<GetAudioController> _logger;
        private readonly IGetAudioRepo _getAudioRepo;
        public GetAudioController(ILogger<GetAudioController> logger, IGetAudioRepo getAudioRepo)
        {
            _logger = logger;
            _getAudioRepo = getAudioRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string userid, string partnerid, string voicespeed, string preferredname, string gender, string country)
        {
            if (String.IsNullOrEmpty(userid))
            {
                return BadRequest();
            }

            GetAudioRequest getAudioRequest = new GetAudioRequest();
            getAudioRequest.parterid = partnerid;
            getAudioRequest.useridList = new List<string>() { userid };
            string blobURL = _getAudioRepo.GetBlobURL(getAudioRequest);
            if (!String.IsNullOrEmpty(blobURL))
            {
                var audioFile = await GetAudioService.GetBlobFromAzureBlob(blobURL);
                return File(audioFile, "audio/mp3");
            } 
            else if(preferredname != null)
            {
                var audioFile = await GetAudioService.GetAudioFromAzure(voicespeed, preferredname, gender, country); 
                return File(audioFile, "audio/mp3");
            }
            return BadRequest();
        }
    }
}


