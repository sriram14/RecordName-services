using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerUserLoginService.Models
{
    public class PartnerUserLoginRequest
    {
        public string userid { get; set; }
        public string password { get; set; }
    }

    public class GetUserCount
    {
        public long getpartnercount { get; set; }
    }
}
