using GetAudiotatus.DataAccess;
using Npgsql;
using NpgsqlTypes;
using PartnerApp.Common;
using PartnerApp.Models;
using System.Data;

namespace JWTTest.DataAccess
{
    public class LoginRepo : BaseRepo, ILoginRepo
    {
        readonly IUtility _utility;
        public LoginRepo(IUtility utility)
        {
            _utility = utility;
        }
        public bool CheckUser(LoginRequest loginRequest)
        {
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[2];

            NpgsqlParameter userIdParam = new NpgsqlParameter("uid", NpgsqlDbType.Varchar);
            userIdParam.Value = loginRequest.userid;
            npgsqlParameters[0] = userIdParam;

            NpgsqlParameter pwdParam = new NpgsqlParameter("pwd", NpgsqlDbType.Varchar);
            pwdParam.Value = loginRequest.password;
            npgsqlParameters[1] = pwdParam;

            var loginStatus = base.CheckUser("SELECT * from public.verifyuser(:uid,:pwd)", CommandType.Text, npgsqlParameters).Tables[0];

            var result = _utility.ConvertToData<LoginResponse>(loginStatus);

            return result[0].loginResult;
        }

        
    }
}
