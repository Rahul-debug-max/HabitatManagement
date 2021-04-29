using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitatManagement.BusinessEntities
{
    public class FormDesignTemplateBE : BusinessEntity
    {
        public int FormID { get; set; }
        public string Design { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int TotalRecord { get; set; }
    }
}