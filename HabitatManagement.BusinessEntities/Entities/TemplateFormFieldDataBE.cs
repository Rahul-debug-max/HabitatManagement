using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.Business
{
    public class TemplateFormFieldDataBE : BusinessEntity
    {
        public int ReferenceNumber { get; set; }
        public int FormID { get; set; }
        public int Field { get; set; }
        public string FieldValue { get; set; }

        public DateTime CreationDate { get; set; }

        public string DigitalSignatureImage64BitString { get; set; }
        public string FieldType { get; set; }    
        public string Design { get; set; }
        public string Description { get; set; }
    }
}