using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFilesServer.DataAccess
{
    public abstract class BaseRepo
    {
        protected string PartnerDBConnection
        {
            get
            {
                IConfigurationSection connectionStrings =  Startup.ConnectionStrings;
                return connectionStrings.GetSection("RecordYourNameDBString").Value;

            }
        }

        public virtual async Task<DataSet> GetDataFromPartnerDBAsync(string query, CommandType commandType, NpgsqlParameter[] npgsqlParameters = null)
        {
            DataSet dataSet = new DataSet();
            await using(NpgsqlConnection conn = new NpgsqlConnection(PartnerDBConnection))
            {
                using(NpgsqlCommand command = new NpgsqlCommand(query))
                {
                    command.CommandType = commandType;
                    command.Connection = conn;
                    if(npgsqlParameters!=null)
                    {
                        AddParamtersToCommand(command, npgsqlParameters);
                    }
                    using (NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(command))
                    {
                        conn.Open();
                        npgsqlDataAdapter.Fill(dataSet);
                    }
                    conn.Close();
                }
               
            }
            return dataSet;
        }

        public virtual string UpdateDataToPartnerDB(string query, CommandType commandType, NpgsqlParameter[] npgsqlParameters, bool includeErrorsinOutput = true)
        {
            string errors = string.Empty;
           
            using (NpgsqlConnection conn = new NpgsqlConnection(PartnerDBConnection))
            {
                using (NpgsqlCommand command = new NpgsqlCommand(query))
                {
                    command.CommandType = commandType;
                    command.Connection = conn;
                    command.Parameters.AddRange(npgsqlParameters);
                    
                    conn.Open();
                    using(NpgsqlTransaction npgsqlTransaction = conn.BeginTransaction())
                    {
                        command.Transaction = npgsqlTransaction;
                        command.ExecuteNonQuery();
                        if(includeErrorsinOutput)
                        {
                            errors = command.Parameters["errors"].Value.ToString();
                        }
                        if(string.IsNullOrEmpty(errors))
                        {
                            npgsqlTransaction.Commit();
                        }
                        else
                        {
                            npgsqlTransaction.Rollback();
                        }
                    }
                    conn.Close();
                }

            }

            return errors;
        }

        private void AddParamtersToCommand(NpgsqlCommand command, NpgsqlParameter[] npgsqlParameters)
        {
            if(npgsqlParameters!=null)
            {
                foreach ( var parameter in npgsqlParameters)
                {
                    if(parameter.Value==null)
                    {
                        parameter.Value = System.DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }
        }
    }
}
