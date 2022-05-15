using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpeechToken.Controllers
{
    [ApiController]
    public class SpeechController : ControllerBase
    {
        private readonly string FetchTokenUri;
        private readonly string SubscriptionKey;
        public SpeechController(ILogger<SpeechController> logger, IConfiguration configuration)
        {
            FetchTokenUri = configuration["ConnectionStrings:Token_URL"];
            SubscriptionKey = configuration["ConnectionStrings:Subscription_Key"];
        }

        [HttpGet("GetToken")]
        public async Task<IActionResult> GetToken(string clientID, string userID)
        {
                var AccessToken = "";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
                    UriBuilder uriBuilder = new UriBuilder(FetchTokenUri);

                    var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                    AccessToken = await result.Content.ReadAsStringAsync();
                }
                return Ok(AccessToken);
        }

    }
}
