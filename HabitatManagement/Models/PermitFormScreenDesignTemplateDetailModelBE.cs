
using HabitatManagement.BusinessEntities;
using System.Collections.Generic;

namespace HabitatManagement.Models
{
    public class PermitFormScreenDesignTemplateDetailModelBE : PermitFormScreenDesignTemplateBE
    {
        public List<PermitFormScreenDesignTemplateDetailBE> TemplateDetails { get; set; }
    }
}