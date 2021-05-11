using HabitatManagement.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.Business
{
    public class ProjectBE : BusinessEntity
    {
        public int ID { get; set; }

        public int ClientID { get; set; }

        public string Project { get; set; }

        public string ProjectName { get; set; }

        public string Description { get; set; }

        public string Manager { get; set; }

        public string SiteAddress { get; set; }

        public string SitePostcode { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
	}
}