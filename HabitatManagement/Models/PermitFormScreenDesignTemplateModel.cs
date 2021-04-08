using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitatManagement.Models
{
    public class PermitFormScreenDesignTemplateModel
    {
        public string FormID { get; set; }
        public string Design { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string CreatedDateTime { get; set; }
        public string LastUpdatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}