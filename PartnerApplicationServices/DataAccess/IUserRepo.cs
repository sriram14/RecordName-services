using PartnerApplicationServices.Models;
using System.Collections.Generic;

namespace PartnerApplicationServices.DataAccess
{
    public interface IUserRepo
    {
        public string RegisterUser(UserRegistrationRequest userRegistrationRequest);
        bool VerifyUser(UserLoginRequest userLoginRequest);
        IList<GetAllUserResponse> GetAllUsers(string userid);
    }
}
