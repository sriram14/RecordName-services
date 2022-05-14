using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GetAudio.Services
{
    public class GetAudioService
    {
        public static async Task<byte[]> GetAudioFromAzure(string voiceSpeed)
        {
            HttpResponseMessage audioResponse = null;
            using (HttpClient client = new HttpClient())
            {
                var tokenResponse = await client.GetAsync(Startup.ConnectionStrings.GetSection("tokenAPI").Value);
                if (tokenResponse.IsSuccessStatusCode)
                {
                    var token = await tokenResponse.Content.ReadAsStringAsync();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var voiceListResponse = await client.GetAsync(Startup.ConnectionStrings.GetSection("voiceListAPI").Value);
                    if (voiceListResponse.IsSuccessStatusCode)
                    {
                        var voiceList = await voiceListResponse.Content.ReadAsStringAsync();
                        //query DB for user to get gender, name and country
                        var Gender = "male";
                        var Name = "Divya";
                        var Country = "india";
                        if (Gender == null)
                        {
                            var genderProfile = new List<string>() { "male", "female" };
                            Random rnd = new Random();
                            Gender = genderProfile[rnd.Next(genderProfile.Count)];

                        }
                        List<string> ShortList = new List<string>();
                        var resource = JArray.Parse(voiceList);
                        foreach (JObject profile in resource.Children<JObject>())
                        {
                            var VoiceLocaleName = profile.SelectToken("LocaleName").ToString().ToLower();
                            var VoiceShortName = profile.SelectToken("ShortName").ToString();
                            var VoiceStatus = profile.SelectToken("Status").ToString();
                            var VoiceGender = profile.SelectToken("Gender").ToString().ToLower();
                            Console.WriteLine(VoiceLocaleName.ToLower());
                            var isProfileIncluded = (VoiceGender.Contains(Gender) && (VoiceLocaleName.ToLower().Contains(Country) || (VoiceLocaleName.ToLower().Contains("Chinese") && Country == "china")));
                            if (isProfileIncluded)
                            {
                                ShortList.Add(VoiceShortName);
                                Console.WriteLine(VoiceShortName);
                            }
                        }
                        // For now using only the first result from Azure TTS
                        var SSML = $@"<speak version='1.0' xml:lang='en-US'>
                            <voice xml:lang='en-US' name='{ShortList[0]}'> 
                                    <prosody rate='{voiceSpeed}'>    
                                        {Name}
                                    </prosody>
                                </voice>
                        </speak>";
                        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, Startup.ConnectionStrings.GetSection("speechAPI").Value);
                        req.Content = new StringContent(SSML, Encoding.UTF8, "application/ssml+xml");
                        client.DefaultRequestHeaders.Add("X-Microsoft-OutputFormat", "audio-24khz-160kbitrate-mono-mp3");
                        client.DefaultRequestHeaders.Host = "eastus.tts.speech.microsoft.com";
                        client.DefaultRequestHeaders.Add("User-Agent", "RYN");
                        audioResponse = await client.SendAsync(req);
                    }
                }
            }
            return await audioResponse.Content.ReadAsByteArrayAsync();

        }

        public static async Task<byte[]> GetBlobFromAzureBlob(string blobURL) 
        {
            using(HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(blobURL);
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
    }
}
