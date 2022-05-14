using Npgsql;
using NpgsqlTypes;
using RecordYourName_API.Models;
using System.Data;
using System.Threading.Tasks;

namespace RecordYourName_API.DataAccess
{
    public class PartnerUserRepo : BaseRepo, IPartnerUserRepo
    {
        public string RegisterPartnerUser(RegisterPartnerUserRequest registerPartnerUserRequest)
        {
            string errors = string.Empty;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[6];

            NpgsqlParameter userIdParam = new NpgsqlParameter("userid", NpgsqlDbType.Varchar);
            userIdParam.Value = registerPartnerUserRequest.userid;
            npgsqlParameters[0] = userIdParam;

            NpgsqlParameter pwdParam = new NpgsqlParameter("pwd", NpgsqlDbType.Varchar);
            pwdParam.Value = registerPartnerUserRequest.password;
            npgsqlParameters[1] = pwdParam;

            NpgsqlParameter firstNameParam = new NpgsqlParameter("firstname", NpgsqlDbType.Varchar);
            firstNameParam.Value = registerPartnerUserRequest.firstname;
            npgsqlParameters[2] = firstNameParam;

            NpgsqlParameter lastNameParam = new NpgsqlParameter("lastname", NpgsqlDbType.Varchar);
            lastNameParam.Value = registerPartnerUserRequest.lastname;
            npgsqlParameters[3] = lastNameParam;

            NpgsqlParameter emailParam = new NpgsqlParameter("email", NpgsqlDbType.Varchar);
            emailParam.Value = registerPartnerUserRequest.email;
            npgsqlParameters[4] = emailParam;

            NpgsqlParameter errorParam = new NpgsqlParameter("errors", NpgsqlDbType.Text, -1)
            {
                Direction = ParameterDirection.InputOutput
            };
            errorParam.Value = "";
            npgsqlParameters[5] = errorParam;

            errors = base.UpdateDataToPartnerDB("call createpartneruser(:userid, :pwd, :firstname, :lastname, :email, :errors)", CommandType.Text, npgsqlParameters);

            return errors;
            
        }

        
    }
}
