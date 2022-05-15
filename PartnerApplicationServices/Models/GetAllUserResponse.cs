using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerApplicationServices.Models
{
    public class GetAllUserResponse
    {
        public string userid { get; set; }
        public string preferredname { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string gender { get; set; }
        public string location { get; set; }
        public bool isfriend { get; set; }
        public bool isadmin { get; set; }
    }
}
