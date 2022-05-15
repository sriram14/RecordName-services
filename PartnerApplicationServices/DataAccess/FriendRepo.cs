using Npgsql;
using NpgsqlTypes;
using PartnerApplicationServices.Common;
using PartnerApplicationServices.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PartnerApplicationServices.DataAccess
{
    public class FriendRepo : BaseRepo, IFriendRepo
    {
        readonly IUtility _utility;
        public FriendRepo(IUtility utility)
        {
            _utility = utility;
        }
        public string AddFriend(Friend friend)
        {
            string errors = string.Empty;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[3];

            NpgsqlParameter userIdParam = new NpgsqlParameter("userid", NpgsqlDbType.Varchar);
            userIdParam.Value = friend.userid;
            npgsqlParameters[0] = userIdParam;

            NpgsqlParameter friendUserIdParam = new NpgsqlParameter("frienduserid", NpgsqlDbType.Varchar);
            friendUserIdParam.Value = friend.frienduserid;
            npgsqlParameters[1] = friendUserIdParam;

            NpgsqlParameter errorParam = new NpgsqlParameter("errors", NpgsqlDbType.Text, -1)
            {
                Direction = ParameterDirection.InputOutput
            };
            errorParam.Value = "";
            npgsqlParameters[2] = errorParam;

            errors = base.UpdateDataToPartnerDB("call public.addfriend(:userid, :frienduserid, :errors)", CommandType.Text, npgsqlParameters);

            return errors;

        }

        public string RemoveFriend(Friend friend)
        {
            string errors = string.Empty;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[3];

            NpgsqlParameter userIdParam = new NpgsqlParameter("userid", NpgsqlDbType.Varchar);
            userIdParam.Value = friend.userid;
            npgsqlParameters[0] = userIdParam;

            NpgsqlParameter friendUserIdParam = new NpgsqlParameter("frienduserid", NpgsqlDbType.Varchar);
            friendUserIdParam.Value = friend.frienduserid;
            npgsqlParameters[1] = friendUserIdParam;

            NpgsqlParameter errorParam = new NpgsqlParameter("errors", NpgsqlDbType.Text, -1)
            {
                Direction = ParameterDirection.InputOutput
            };
            errorParam.Value = "";
            npgsqlParameters[2] = errorParam;

            errors = base.UpdateDataToPartnerDB("call public.deletefriend(:userid, :frienduserid, :errors)", CommandType.Text, npgsqlParameters);

            return errors;

        }

        public List<GetFriendResponse> GetFriend(string userid)
        {
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[1];

            NpgsqlParameter userIdParam = new NpgsqlParameter("userid", NpgsqlDbType.Varchar);
            userIdParam.Value = userid;
            npgsqlParameters[0] = userIdParam;

            var freindsList = base.GetDataFromPartnerDBAsync("SELECT * from public.getfriends(:userid)", CommandType.Text, npgsqlParameters).Tables[0];
             var result = _utility.ConvertToData<GetFriendResponse>(freindsList);
            return (List<GetFriendResponse>)result;
        }

    }
}
