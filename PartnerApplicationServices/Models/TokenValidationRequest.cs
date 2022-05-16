using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerApplicationServices.Models
{
    public class TokenValidationRequest
    {
        public string token { get; set; }
        public string userid { get; set; }
    }

    public class TokenValidationResponse
    {
        public bool isadmin { get; set; }
    }
}
