using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.BusinessEntities
{
    public class TemplateFormSectionBE : BusinessEntity
    {
        public int FormID { get; set; }
        public string SectionName { get; set; }
        public string SectionDescription { get; set; }
        public int Sequence { get; set; }
    }
}