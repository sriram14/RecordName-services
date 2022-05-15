using PartnerAppRegistrationService.Models;

namespace PartnerAppRegistrationService.DataAccess
{
    public interface IPartnerAppRepo
    {
        public string PartnerAppRegistration(PartnerAppRegistrationDetails partnerAppRegistrationRequest);
    }
}
