using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SQLite;

namespace DataAccessLibrary.Databases
{
    public class SQLiteDataAccess : ISQLiteDataAccess
    {
        private readonly IConfiguration _config;

        public SQLiteDataAccess(IConfiguration config)
        {
            _config = config;
        }
        public List<T> LoadData<T, U>(string sql,
                                      U parameters,
                                      string connectionStringName)
        {

            string? connectionString = _config.GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sql, parameters).ToList();
                return rows ?? [];
            }
        }

        public void SaveData<T, U>(string sql,
                                   U parameters,
                                   string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute(sql, parameters);
            }
        }
    }
}
