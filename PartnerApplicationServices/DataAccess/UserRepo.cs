
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

        public string CreateAdmin(string createrUserId, string userId)
        {
            string results = string.Empty;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[3];

            NpgsqlParameter createruserIdParam = new NpgsqlParameter("createruserid", NpgsqlDbType.Varchar);
            createruserIdParam.Value = createrUserId;
            npgsqlParameters[0] = createruserIdParam;

            NpgsqlParameter othersuseridParam = new NpgsqlParameter("othersuserid", NpgsqlDbType.Varchar);
            othersuseridParam.Value = userId;
            npgsqlParameters[1] = othersuseridParam;

            NpgsqlParameter errorParam = new NpgsqlParameter("errors", NpgsqlDbType.Text, -1)
            {
                Direction = ParameterDirection.InputOutput
            };
            errorParam.Value = "";
            npgsqlParameters[2] = errorParam;

            results = base.UpdateDataToPartnerDB("select public.createadminrole(:createruserid, :othersuserid, :errors)", CommandType.Text, npgsqlParameters);
            return results;
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

        public GetUserDetailResponse GetUserDetail(string userId)
        {
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[1];

            NpgsqlParameter userIdParam = new NpgsqlParameter("uid", NpgsqlDbType.Varchar);
            userIdParam.Value = userId;
            npgsqlParameters[0] = userIdParam;


            var response = base.GetDataFromPartnerDBAsync("select * from  public.getuserdetail(:uid)", CommandType.Text, npgsqlParameters).Tables[0];

            var responseList = _utility.ConvertToData<GetUserDetailResponse>(response);

            return responseList.Count > 0 ? responseList[0] : null;
        }

        public string UpdateUserDetail(UpdateUserDetailRequest updateUserDetailRequest)
        {
            string errors = string.Empty;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[11];

            NpgsqlParameter idParam = new NpgsqlParameter("sid", NpgsqlDbType.Integer);
            idParam.Value = updateUserDetailRequest.userid;
            npgsqlParameters[0] = idParam;

            NpgsqlParameter uidParam = new NpgsqlParameter("uid", NpgsqlDbType.Varchar);
            uidParam.Value = updateUserDetailRequest.userid;
            npgsqlParameters[1] = uidParam;

            NpgsqlParameter firstnameParam = new NpgsqlParameter("fname", NpgsqlDbType.Varchar);
            firstnameParam.Value = updateUserDetailRequest.firstname;
            npgsqlParameters[2] = firstnameParam;

            NpgsqlParameter lastnameParam = new NpgsqlParameter("lname", NpgsqlDbType.Varchar);
            lastnameParam.Value = updateUserDetailRequest.lastname;
            npgsqlParameters[3] = lastnameParam;

            NpgsqlParameter preferrednameParam = new NpgsqlParameter("prefname", NpgsqlDbType.Varchar);
            preferrednameParam.Value = updateUserDetailRequest.preferredname;
            npgsqlParameters[4] = preferrednameParam;

            NpgsqlParameter avataridParam = new NpgsqlParameter("avataarid", NpgsqlDbType.Integer);
            avataridParam.Value = updateUserDetailRequest.avatarid;
            npgsqlParameters[5] = avataridParam;

            NpgsqlParameter locParam = new NpgsqlParameter("loc", NpgsqlDbType.Varchar);
            locParam.Value = updateUserDetailRequest.location;
            npgsqlParameters[6] = locParam;

            NpgsqlParameter genderParam = new NpgsqlParameter("sex", NpgsqlDbType.Varchar);
            genderParam.Value = updateUserDetailRequest.gender;
            npgsqlParameters[7] = genderParam;

            NpgsqlParameter emailidParam = new NpgsqlParameter("email", NpgsqlDbType.Varchar);
            emailidParam.Value = updateUserDetailRequest.email;
            npgsqlParameters[8] = emailidParam;

            NpgsqlParameter pwdParam = new NpgsqlParameter("pwd", NpgsqlDbType.Varchar);
            pwdParam.Value = updateUserDetailRequest.password;
            npgsqlParameters[9] = pwdParam;

            NpgsqlParameter errorParam = new NpgsqlParameter("errors", NpgsqlDbType.Text, -1)
            {
                Direction = ParameterDirection.InputOutput
            };
            errorParam.Value = "";
            npgsqlParameters[10] = errorParam;

            errors = base.UpdateDataToPartnerDB("select public.updateuserdetail(:sid,:uid, :fname, :lname, :prefname, :avataarid, :loc, :sex, :email, :pwd, :errors)", CommandType.Text, npgsqlParameters);

            return errors;
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

            NpgsqlParameter prefferednameParam = new NpgsqlParameter("preferredname", NpgsqlDbType.Varchar);
            prefferednameParam.Value = userRegistrationRequest.preferredname;
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

            errors = base.UpdateDataToPartnerDB("call public.registeruser(:uid, :pwd, :rolename, :expirymode, :expirytime, :avatarid, :emailid, :firstname, :lastname, :preferredname, :loc, :errors)", CommandType.Text, npgsqlParameters);

            return errors;
        }

        public bool VerifyUser(LoginRequest userLoginRequest)
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

            var status = _utility.ConvertToData<LoginResponse>(loginStatus);

            success_status = status[0].verifyuser;

            return success_status;
        }
    }
}
