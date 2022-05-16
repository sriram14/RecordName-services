
using System;

namespace PartnerAppRegistrationService.Models
{
    public class GetAllPartnerAppsResponse
    {
        public int seq { get; set; }
        public string guid { get; set; }
        public string hosturl { get; set; }
        public string description { get; set; }
        public string createdby { get; set; }
        public DateTime createdtime { get; set; }
        public string updatedby { get; set; }
        public DateTime updatedTime { get; set; }
    }
}
