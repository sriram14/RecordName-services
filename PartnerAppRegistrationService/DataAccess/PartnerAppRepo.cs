using Npgsql;
using NpgsqlTypes;
using PartnerAppRegistrationService.Common;
using PartnerAppRegistrationService.Models;
using System.Data;
using System.Threading.Tasks;

namespace PartnerAppRegistrationService.DataAccess
{
    public class PartnerAppRepo : BaseRepo, IPartnerAppRepo
    {
        readonly IUtility _utility;
        public PartnerAppRepo(IUtility utility)
        {
            _utility = utility;
        }
        public string PartnerAppRegistration(PartnerAppRegistrationRequest partnerAppRegistrationRequest)
        {
            string errors = string.Empty;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[8];

            NpgsqlParameter guidIdParam = new NpgsqlParameter("guid", NpgsqlDbType.Varchar);
            guidIdParam.Value = partnerAppRegistrationRequest.guid;
            npgsqlParameters[0] = guidIdParam;

            NpgsqlParameter hosturlParam = new NpgsqlParameter("hosturl", NpgsqlDbType.Varchar);
            hosturlParam.Value = partnerAppRegistrationRequest.hosturl;
            npgsqlParameters[1] = hosturlParam;

            NpgsqlParameter descriptionParam = new NpgsqlParameter("description", NpgsqlDbType.Varchar);
            descriptionParam.Value = partnerAppRegistrationRequest.description;
            npgsqlParameters[2] = descriptionParam;

            NpgsqlParameter createdbyParam = new NpgsqlParameter("createdby", NpgsqlDbType.Varchar);
            createdbyParam.Value = partnerAppRegistrationRequest.createdby;
            npgsqlParameters[3] = createdbyParam;

            NpgsqlParameter createdtimeParam = new NpgsqlParameter("createdtime", NpgsqlDbType.Date);
            createdtimeParam.Value = partnerAppRegistrationRequest.createdtime.Date;
            npgsqlParameters[4] = createdtimeParam;

            NpgsqlParameter updatedbyParam = new NpgsqlParameter("updatedby", NpgsqlDbType.Varchar);
            updatedbyParam.Value = partnerAppRegistrationRequest.updatedby;
            npgsqlParameters[5] = updatedbyParam;

            NpgsqlParameter updatedtimeyParam = new NpgsqlParameter("updatedtime", NpgsqlDbType.Date);
            updatedtimeyParam.Value = partnerAppRegistrationRequest.updatedtime.Date;
            npgsqlParameters[6] = updatedtimeyParam;

            NpgsqlParameter errorParam = new NpgsqlParameter("errors", NpgsqlDbType.Text, -1)
            {
                Direction = ParameterDirection.InputOutput
            };
            errorParam.Value = "";
            npgsqlParameters[7] = errorParam;

            errors = base.UpdateDataToPartnerDB("call public.registerpartnerapp(:guid, :hosturl, :description, :createdby, :createdtime, :updatedby, :updatedtime, :errors)", CommandType.Text, npgsqlParameters);

            return errors;

        }


    }
}
