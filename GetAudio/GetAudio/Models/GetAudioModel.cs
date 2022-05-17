using System;
using System.Collections.Generic;

namespace GetAudio.Models
{
    public class GetAudioRequest
    {
        public List<string> useridList { get; set; }
        public string parterid { get; set; }
    }

    public class GetAudioStatusResponse
    {
        public string userid_st { get; set; }
        public string bloburl_st { get; set; }
        public long filesize_st { get; set; }
        public string filetype_st { get; set; }
        public DateTime updatedtime_st  { get; set; }
    }

}
