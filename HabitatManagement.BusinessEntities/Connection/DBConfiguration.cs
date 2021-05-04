using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.Business
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
        public static string WebAPIHostingURL
        {
            get
            {
                return _configuration["WebAPIHostingURL"];
            }
        }
        public static string JWTKey
        {
            get
            {
                return _configuration["Jwt:Key"];
            }
        }
        public static string JWTIssuer
        {
            get
            {
                return _configuration["Jwt:Issuer"];
            }
        }

        public static int JWTExpireTokenTimeInMinutes
        {
            get
            {
                return Functions.ToInt(_configuration["Jwt:ExpireTokenTimeInMinutes"]) == 0 ? 120 : Functions.ToInt(_configuration["Jwt:ExpireTokenTimeInMinutes"]);
            }
        }
    }
}