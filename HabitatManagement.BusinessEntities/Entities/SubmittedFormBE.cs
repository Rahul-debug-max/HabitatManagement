using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.Business
{
    public class SubmittedFormBE : BusinessEntity
    {
        public int ReferenceNumber { get; set; }
        public int ProjectId { get; set; }
        public int FormId { get; set; }
        public SubmittedFormStatusField Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
	}
}