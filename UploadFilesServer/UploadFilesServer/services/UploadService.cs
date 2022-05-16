
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
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
        public UploadService(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetConnectionString("AzureStorage");
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

            errors = base.UpdateDataToPartnerDB("call createaudiometadata(:userid, :partnerid, :bloburl, :filesize, :filetype,:createdby,:createdtime,:updatedby,:updatedtime, :errors)", CommandType.Text, npgsqlParameters);
           
            return errors;

        }
    }
}
