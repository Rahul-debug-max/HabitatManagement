using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.BusinessEntities
{
    public class TemplateFormFieldDataBE : BusinessEntity
    {
        public int FormID { get; set; }
        public int Field { get; set; }
        public string FieldValue { get; set; }
        public string DigitalSignatureImage64BitString { get; set; }
        public string FieldType { get; set; }
    }
}