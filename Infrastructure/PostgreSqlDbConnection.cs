using System.Configuration;
using Npgsql;

namespace Change_Point.Infrastructure
{
    public static class PostgreSqlDbConnection
    {
        private static readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["PostgreSqlDbConnectionTest"]?.ConnectionString;

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
