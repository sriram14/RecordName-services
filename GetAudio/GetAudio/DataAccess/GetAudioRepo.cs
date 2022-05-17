using GetAudio.Common;
using GetAudio.Models;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace GetAudio.DataAccess
{
    public class GetAudioRepo : BaseRepo, IGetAudioRepo
    {
        readonly IUtility _utility;
        public GetAudioRepo(IUtility utility)
        {
            _utility = utility;
        }
        public string GetBlobURL(GetAudioRequest getAudioRequest)
        {
            long success_status = 0;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[2];

            NpgsqlParameter userIdParam = new NpgsqlParameter("pid", NpgsqlDbType.Text);
            userIdParam.Value = getAudioRequest.parterid;
            npgsqlParameters[0] = userIdParam;

            NpgsqlParameter pwdParam = new NpgsqlParameter("userdetail", NpgsqlDbType.Array | NpgsqlDbType.Text);
            pwdParam.Value = getAudioRequest.useridList;
            npgsqlParameters[1] = pwdParam;

            var loginStatus = base.GetAudioDataFromDB("SELECT * from public.getaudio(:pid,:userdetail)", CommandType.Text, npgsqlParameters).Tables[0];

            var result = _utility.ConvertToData<GetAudioStatusResponse>(loginStatus);

            return result.Count > 0 ? result[0].bloburl_st : "";
        }

        
    }
}
