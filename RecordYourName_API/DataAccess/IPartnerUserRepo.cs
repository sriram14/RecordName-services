using RecordYourName_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecordYourName_API.DataAccess
{
    public interface IPartnerUserRepo
    {
        public string RegisterPartnerUser(RegisterPartnerUserRequest registerPartnerUserRequest);


    }
}
