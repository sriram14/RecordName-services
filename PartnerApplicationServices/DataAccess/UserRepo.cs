
using Npgsql;
using NpgsqlTypes;
using PartnerApplicationServices.Common;
using PartnerApplicationServices.Models;
using System.Collections.Generic;
using System.Data;

namespace PartnerApplicationServices.DataAccess
{
    public class UserRepo : BaseRepo, IUserRepo
    {
        readonly IUtility _utility;
        public UserRepo(IUtility utility)
        {
            _utility = utility;
        }

        public IList<GetAllUserResponse> GetAllUsers(string userid)
        {
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[1];

            NpgsqlParameter userIdParam = new NpgsqlParameter("uid", NpgsqlDbType.Varchar);
            userIdParam.Value = userid;
            npgsqlParameters[0] = userIdParam;


            var response = base.GetDataFromPartnerDBAsync("select * from public.getallusers(:uid)", CommandType.Text, npgsqlParameters).Tables[0];

            return _utility.ConvertToData<GetAllUserResponse>(response);
        }

        public string RegisterUser(UserRegistrationRequest userRegistrationRequest)
        {
            string errors = string.Empty;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[12];

            NpgsqlParameter uidParam = new NpgsqlParameter("uid", NpgsqlDbType.Varchar);
            uidParam.Value = userRegistrationRequest.userid;
            npgsqlParameters[0] = uidParam;

            NpgsqlParameter passwordParam = new NpgsqlParameter("pwd", NpgsqlDbType.Varchar);
            passwordParam.Value = userRegistrationRequest.password;
            npgsqlParameters[1] = passwordParam;

            NpgsqlParameter roleParam = new NpgsqlParameter("rolename", NpgsqlDbType.Varchar);
            roleParam.Value = userRegistrationRequest.role;
            npgsqlParameters[2] = roleParam;

            NpgsqlParameter expirymodeParam = new NpgsqlParameter("expirymode", NpgsqlDbType.Varchar);
            expirymodeParam.Value = userRegistrationRequest.expirymode;
            npgsqlParameters[3] = expirymodeParam;

            NpgsqlParameter expirytimeParam = new NpgsqlParameter("expirytime", NpgsqlDbType.Date);
            expirytimeParam.Value = userRegistrationRequest.expirytime.Date;
            npgsqlParameters[4] = expirytimeParam;

            NpgsqlParameter avataridParam = new NpgsqlParameter("avatarid", NpgsqlDbType.Integer);
            avataridParam.Value = userRegistrationRequest.avatarid;
            npgsqlParameters[5] = avataridParam;

            NpgsqlParameter emailidParam = new NpgsqlParameter("emailid", NpgsqlDbType.Varchar);
            emailidParam.Value = userRegistrationRequest.emailid;
            npgsqlParameters[6] = emailidParam;

            NpgsqlParameter firstnameParam = new NpgsqlParameter("firstname", NpgsqlDbType.Varchar);
            firstnameParam.Value = userRegistrationRequest.firstname;
            npgsqlParameters[7] = firstnameParam;

            NpgsqlParameter lastnameParam = new NpgsqlParameter("lastname", NpgsqlDbType.Varchar);
            lastnameParam.Value = userRegistrationRequest.lastname;
            npgsqlParameters[8] = lastnameParam;

            NpgsqlParameter prefferednameParam = new NpgsqlParameter("prefferedname", NpgsqlDbType.Varchar);
            prefferednameParam.Value = userRegistrationRequest.prefferedname;
            npgsqlParameters[9] = prefferednameParam;

            NpgsqlParameter locParam = new NpgsqlParameter("loc", NpgsqlDbType.Varchar);
            locParam.Value = userRegistrationRequest.location;
            npgsqlParameters[10] = locParam;

            NpgsqlParameter errorParam = new NpgsqlParameter("errors", NpgsqlDbType.Text, -1)
            {
                Direction = ParameterDirection.InputOutput
            };
            errorParam.Value = "";
            npgsqlParameters[11] = errorParam;

            errors = base.UpdateDataToPartnerDB("call public.registeruser(:uid, :pwd, :rolename, :expirymode, :expirytime, :avatarid, :emailid, :firstname, :lastname, :prefferedname, :loc, :errors)", CommandType.Text, npgsqlParameters);

            return errors;
        }

        public bool VerifyUser(UserLoginRequest userLoginRequest)
        {
            bool success_status = false;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[2];

            NpgsqlParameter userIdParam = new NpgsqlParameter("uid", NpgsqlDbType.Varchar);
            userIdParam.Value = userLoginRequest.userid;
            npgsqlParameters[0] = userIdParam;

            NpgsqlParameter pwdParam = new NpgsqlParameter("pwd", NpgsqlDbType.Varchar);
            pwdParam.Value = userLoginRequest.password;
            npgsqlParameters[1] = pwdParam;

            var loginStatus = base.GetDataFromPartnerDBAsync("select * from public.verifyuser(:uid, :pwd)", CommandType.Text, npgsqlParameters).Tables[0];

            var status = _utility.ConvertToData<GetUserCount>(loginStatus);

            success_status = status[0].verifyuser;

            return success_status;
        }
    }
}
