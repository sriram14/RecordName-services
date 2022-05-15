using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerApplicationServices.Models
{
    public class UserLoginRequest
    {
        public string userid { get; set; }
        public string password { get; set; }
    }
    public class GetUserCount
    {
        public bool verifyuser { get; set; }
    }
}
