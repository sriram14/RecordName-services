﻿using PartnerApplicationServices.Models;
using System.Collections.Generic;

namespace PartnerApplicationServices.DataAccess
{
    public interface IUserRepo
    {
        public string RegisterUser(UserRegistrationRequest userRegistrationRequest);
        public bool VerifyUser(UserLoginRequest userLoginRequest);
        public IList<GetAllUserResponse> GetAllUsers(string userid);
        public string CreateAdmin(string createrUserId, string userId);
        GetUserDetailResponse GetUserDetail(string userId);
    }
}
