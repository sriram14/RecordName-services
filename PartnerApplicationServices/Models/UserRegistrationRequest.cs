using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerApplicationServices.Models
{
    public class UserRegistrationRequest
    {
        public string userid { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string expirymode { get; set; }
        public DateTime expirytime { get; set; }
        public int avatarid { get; set; }
        public string emailid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string prefferedname { get; set; }
        public string location { get; set; }
    }
}
