using HabitatManagement.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitatManagement.Models
{
    public class ProjectModel : ProjectBE
    {
        public string CreationDate { get; set; }

        public List<int> ProjectFormList { get; set; }

        public string Client { get; set; }
    }
}