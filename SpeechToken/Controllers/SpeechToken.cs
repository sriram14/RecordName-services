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
    [Route("[controller]")]
    public class SpeechTokenController : ControllerBase
    {
        private readonly string FetchTokenUri;
        private readonly string SubscriptionKey;
        private readonly string Database;
        public SpeechTokenController(ILogger<SpeechTokenController> logger, IConfiguration configuration)
        {
            Database = configuration["ConnectionStrings:Database"];
            FetchTokenUri = configuration["ConnectionStrings:Token_URL"];
            SubscriptionKey = configuration["ConnectionStrings:Subscription_Key"];
        }

        [HttpGet("TestDB")]
        public async Task<IActionResult> TestDBAsync()
        {
            await using var conn = new NpgsqlConnection(Database);
            await conn.OpenAsync();
            var output = "";

            await using (var cmd = new NpgsqlCommand("SELECT dname FROM public.dept", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    output += reader.GetString(0);
                }
            }
            return Ok(output);
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
