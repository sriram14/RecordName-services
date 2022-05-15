using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerAppRegistrationService.Models
{
    public class PartnerAppRegistrationDetails
    {
        public string guid { get; set; }
        public string hosturl { get; set; }
        public string description { get; set; }
        public string createdby { get; set; }
        public DateTime createdtime { get; set; }
        public string updatedby { get; set; }
        public DateTime updatedtime { get; set; }

	}

    public class PartnerAppRegistrationRequest
    {
        public string hosturl { get; set; }
        public string description { get; set; }
        public string userid { get; set; }
    }

}
