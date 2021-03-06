using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.Business
{
    public class FormDesignTemplateDetailBE : BusinessEntity
    {
        public int FormID { get; set; }

        public int Field { get; set; }

        public string FieldName { get; set; }

        public FormFieldType FieldType { get; set; }

        [MaxFieldLength(20)]
        public string Section { get; set; }

        public int Sequence { get; set; }

        public bool Mandatory { get; set; }

        public string FieldTypeValue { get; set; }

        public string SectionDescription { get; set; }

        public int SectionSequence { get; set; }

        public int TableRowCount { get; set; }

        public string BackgroundColor { get; set; }

        public string TextColor { get; set; }

        public string MandatoryField { get; set; }


    }
}