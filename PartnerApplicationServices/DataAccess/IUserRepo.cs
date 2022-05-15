

using PartnerApplicationServices.Models;

namespace PartnerApplicationServices.DataAccess
{
    public interface IUserRepo
    {
        public string RegisterUser(UserRegistrationRequest userRegistrationRequest);
        bool VerifyUser(UserLoginRequest userLoginRequest);
    }
}
