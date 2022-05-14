using PartnerUserLoginService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerUserLoginService.DataAccess
{
    public interface IPartnerUserRepo
    {
        long PartnerUserLogin(PartnerUserLoginRequest partnerUserLoginRequest);
    }
}
