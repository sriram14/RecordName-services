
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using UploadFilesServer.Common;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using UploadFilesServer.DataAccess;
using UploadFilesServer.Models;


namespace UploadFilesServer.services
{
    public class UploadService: BaseRepo, IUploadService
    {
        private readonly string _storageConnectionString;

        readonly IUtility _utility;
        

        public UploadService(IConfiguration configuration, IUtility utility)
        {
            _storageConnectionString = configuration.GetConnectionString("AzureStorage");
            _utility = utility;

        }
        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            var container = new BlobContainerClient(_storageConnectionString, "recordvoice");
            var createResponse = await container.CreateIfNotExistsAsync();
            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(PublicAccessType.Blob);
            var blob = container.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
            return blob.Name.ToString();
        }

        public async Task<string> AddAudioMetaData(AudioMetaData audioMetaData)
        {
            string errors = string.Empty;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[10];

            NpgsqlParameter userIdParam = new NpgsqlParameter("userid", NpgsqlDbType.Varchar);
            userIdParam.Value = audioMetaData.userId;
            npgsqlParameters[0] = userIdParam;

            NpgsqlParameter partnerIdParam = new NpgsqlParameter("partnerid", NpgsqlDbType.Varchar);
            partnerIdParam.Value = audioMetaData.partnerId;
            npgsqlParameters[1] = partnerIdParam;

            NpgsqlParameter blobUrlParam = new NpgsqlParameter("bloburl", NpgsqlDbType.Varchar);
            blobUrlParam.Value = audioMetaData.blobUrl;
            npgsqlParameters[2] = blobUrlParam;

            NpgsqlParameter fileSizeParam = new NpgsqlParameter("filesize", NpgsqlDbType.Integer);
            fileSizeParam.Value = audioMetaData.fileSize;
            npgsqlParameters[3] = fileSizeParam;

            NpgsqlParameter fileTypeParam = new NpgsqlParameter("filetype", NpgsqlDbType.Varchar);
            fileTypeParam.Value = audioMetaData.fileType;
            npgsqlParameters[4] = fileTypeParam;

            NpgsqlParameter createdByParam = new NpgsqlParameter("createdby", NpgsqlDbType.Varchar);
            createdByParam.Value = audioMetaData.userId;
            npgsqlParameters[5] = createdByParam;

            NpgsqlParameter createdTimeParam = new NpgsqlParameter("createdtime", NpgsqlDbType.Date);
            createdTimeParam.Value = DateTime.Now;
            npgsqlParameters[6] = createdTimeParam;

            NpgsqlParameter updatedByParam = new NpgsqlParameter("updatedby", NpgsqlDbType.Varchar);
            updatedByParam.Value = audioMetaData.userId;
            npgsqlParameters[7] = updatedByParam;

            NpgsqlParameter updatedTimeParam = new NpgsqlParameter("updatedtime", NpgsqlDbType.Date);
            updatedTimeParam.Value = DateTime.Now;
            npgsqlParameters[8] = updatedTimeParam;

            NpgsqlParameter errorParam = new NpgsqlParameter("errors", NpgsqlDbType.Text, -1)
            {
                Direction = ParameterDirection.InputOutput
            };
            errorParam.Value = "";
            npgsqlParameters[9] = errorParam;

            errors = base.UpdateDataToRecordYourNameDB("call createaudiometadata(:userid, :partnerid, :bloburl, :filesize, :filetype,:createdby,:createdtime,:updatedby,:updatedtime, :errors)", CommandType.Text, npgsqlParameters);
           
            return errors;

        }


        public async Task<string> AddAudioMetaData(AudioMetaData audioMetaData, string createrUserId)
        {
            string errors = string.Empty;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[10];

            NpgsqlParameter userIdParam = new NpgsqlParameter("userid", NpgsqlDbType.Varchar);
            userIdParam.Value = audioMetaData.userId;
            npgsqlParameters[0] = userIdParam;

            NpgsqlParameter partnerIdParam = new NpgsqlParameter("partnerid", NpgsqlDbType.Varchar);
            partnerIdParam.Value = audioMetaData.partnerId;
            npgsqlParameters[1] = partnerIdParam;

            NpgsqlParameter blobUrlParam = new NpgsqlParameter("bloburl", NpgsqlDbType.Varchar);
            blobUrlParam.Value = audioMetaData.blobUrl;
            npgsqlParameters[2] = blobUrlParam;

            NpgsqlParameter fileSizeParam = new NpgsqlParameter("filesize", NpgsqlDbType.Integer);
            fileSizeParam.Value = audioMetaData.fileSize;
            npgsqlParameters[3] = fileSizeParam;

            NpgsqlParameter fileTypeParam = new NpgsqlParameter("filetype", NpgsqlDbType.Varchar);
            fileTypeParam.Value = audioMetaData.fileType;
            npgsqlParameters[4] = fileTypeParam;

            NpgsqlParameter createdByParam = new NpgsqlParameter("createdby", NpgsqlDbType.Varchar);
            createdByParam.Value = createrUserId;
            npgsqlParameters[5] = createdByParam;

            NpgsqlParameter createdTimeParam = new NpgsqlParameter("createdtime", NpgsqlDbType.Date);
            createdTimeParam.Value = DateTime.Now;
            npgsqlParameters[6] = createdTimeParam;

            NpgsqlParameter updatedByParam = new NpgsqlParameter("updatedby", NpgsqlDbType.Varchar);
            updatedByParam.Value = createrUserId;
            npgsqlParameters[7] = updatedByParam;

            NpgsqlParameter updatedTimeParam = new NpgsqlParameter("updatedtime", NpgsqlDbType.Date);
            updatedTimeParam.Value = DateTime.Now;
            npgsqlParameters[8] = updatedTimeParam;

            NpgsqlParameter errorParam = new NpgsqlParameter("errors", NpgsqlDbType.Text, -1)
            {
                Direction = ParameterDirection.InputOutput
            };
            errorParam.Value = "";
            npgsqlParameters[9] = errorParam;

            errors = base.UpdateDataToRecordYourNameDB("call createaudiometadata(:userid, :partnerid, :bloburl, :filesize, :filetype,:createdby,:createdtime,:updatedby,:updatedtime, :errors)", CommandType.Text, npgsqlParameters);

            return errors;

        }


        public bool isAdminUser(string userid)
        {
            bool success_status = false;
            NpgsqlParameter[] npgsqlParameters = new NpgsqlParameter[1];

            NpgsqlParameter userIdParam = new NpgsqlParameter("userid", NpgsqlDbType.Varchar);
            userIdParam.Value = userid;
            npgsqlParameters[0] = userIdParam;

         
            var userRole = base.GetDataFromPartnerDBAsync("select * from public.isadmin(:userid)", CommandType.Text, npgsqlParameters).Tables[0];
            var status = _utility.ConvertToData<UserResponse>(userRole);
            success_status = status[0].isadmin;

            return success_status;
        }
    }
}
