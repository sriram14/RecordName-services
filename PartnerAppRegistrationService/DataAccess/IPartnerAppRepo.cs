using PartnerAppRegistrationService.Models;
using System.Collections.Generic;

namespace PartnerAppRegistrationService.DataAccess
{
    public interface IPartnerAppRepo
    {
        public string PartnerAppRegistration(PartnerAppRegistrationDetails partnerAppRegistrationRequest);

        public IEnumerable<GetAllPartnerAppsResponse> GetAllPartnerApps();
    }
}
