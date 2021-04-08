﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitatManagement.BusinessEntities
{
    public class PermitFormScreenDesignTemplateDetailBE
    {
        public int FormID { get; set; }

        public int Field { get; set; }

        public string FieldName { get; set; }

        public FormFieldType FieldType { get; set; }

        public PromptFormSectionField Section { get; set; }

        public int Sequence { get; set; }
    }
}