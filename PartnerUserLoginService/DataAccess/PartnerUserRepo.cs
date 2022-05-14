using Npgsql;
using NpgsqlTypes;
using PartnerUserLoginService.Common;
using PartnerUserLoginService.Models;
using System.Data;
using System.Threading.Tasks;

namespace PartnerUserLoginService.DataAccess
{
    public class PartnerUserRepo : BaseRepo, IPartnerUserRepo
    {
        readonly IUtility _utility;
        public PartnerUserRepo(IUtility utility)
        {
            _utility = utility;
        }
        public long PartnerUserLogin(PartnerUserLoginRequest partnerUserLoginRequest)
        {
            long success_status = 0;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[2];

            NpgsqlParameter userIdParam = new NpgsqlParameter("uid", NpgsqlDbType.Varchar);
            userIdParam.Value = partnerUserLoginRequest.userid;
            npgsqlParameters[0] = userIdParam;

            NpgsqlParameter pwdParam = new NpgsqlParameter("pwd", NpgsqlDbType.Varchar);
            pwdParam.Value = partnerUserLoginRequest.password;
            npgsqlParameters[1] = pwdParam;

            var loginStatus = base.GetDataFromPartnerDBAsync("select public.getpartnercount(:uid, :pwd)", CommandType.Text, npgsqlParameters).Tables[0];

            var status = _utility.ConvertToData<GetUserCount>(loginStatus);

            success_status = status[0].getpartnercount;

            return success_status;
        }

        
    }
}
