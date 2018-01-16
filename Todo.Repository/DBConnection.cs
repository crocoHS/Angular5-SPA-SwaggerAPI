using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Todo.Repository
{
    public class DBConnection
    {
        static IConfiguration configuration;
        static string connectionString;

        public static void Initialize(IConfiguration config)
        {
            configuration = config;
            connectionString = configuration.GetConnectionString("TodoContext");
        }

        public static SqlConnection NewConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
