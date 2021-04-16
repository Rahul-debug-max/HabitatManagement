using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HabitatManagement.BusinessEntities
{
    public interface IDBConfiguration
    {
        static string Connection { get; }
        static string WebAPIHostingURL { get; }
    }
}