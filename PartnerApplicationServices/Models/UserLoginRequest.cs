using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerApplicationServices.Models
{
    public class LoginRequest
    {
        public string userid { get; set; }
        public string password { get; set; }
    }
    public class LoginResponse
    {
        public bool verifyuser { get; set; }
    }
}
