
using HabitatManagement.BusinessEntities;
using System.Collections.Generic;

namespace HabitatManagement.Models
{
    public class FormDesignTemplateDetailModel : FormDesignTemplateBE
    {
        public List<FormDesignTemplateDetailBE> TemplateDetails { get; set; }

        public List<TemplateFormSectionBE> TemplateSectionDetail { get; set; }
    }
}