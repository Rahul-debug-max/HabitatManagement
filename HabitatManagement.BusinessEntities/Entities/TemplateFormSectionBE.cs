using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.Business
{
    public class TemplateFormSectionBE : BusinessEntity
    {
        public int FormID { get; set; }

        [MaxFieldLength(20)]
        public string Section { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
    }
}