using Microsoft.Extensions.Configuration;
using Todo.Repository;

namespace Todo.Library
{
    public class BL
    {
        private static IConfiguration configuration;
        private static BL instance;

        public IConfiguration Configuration
        {
            get
            {
                return configuration;
            }
        }

        public static BL Instance
        {
            get { return instance ?? (instance = new BL()); }
        }

        public static void Initialize(IConfiguration config)
        {
            configuration = config;
            DBConnection.Initialize(configuration);
        }
    }
}