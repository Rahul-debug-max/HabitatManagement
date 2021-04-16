using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.BusinessEntities
{
    public class DBConfiguration : IDBConfiguration
    {
        private static IConfiguration _configuration;
        public static void SetConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string Connection
        {
            get
            {
               return _configuration.GetConnectionString("DefaultConnection");
            }
        }
    }
}
