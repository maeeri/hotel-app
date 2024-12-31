using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace DataAccessLibrary.Databases
{
    public class SqlServerDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlServerDataAccess(IConfiguration config)
        {
            _config = config;
        }
        public List<T> LoadData<T, U>(string sql,
                                      U parameters,
                                      string connectionStringName,
                                      bool isStoredProcedure = false)
        {

            string? connectionString = _config.GetConnectionString(connectionStringName);
            CommandType commandType = CommandType.Text;
            if (isStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sql, parameters, commandType: commandType).ToList();
                return rows ?? [];
            }
        }

        public void SaveData<T, U>(string sql,
                                   U parameters,
                                   string connectionStringName,
                                   bool isStoredProcedure = false)
        {
            CommandType commandType = CommandType.Text;

            if (isStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            string connectionString = _config.GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sql, parameters, commandType: commandType);
            }
        }
    }
}
