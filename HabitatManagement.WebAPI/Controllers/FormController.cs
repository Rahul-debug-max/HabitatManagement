using HabitatManagement.BusinessEntities;
using HabitatManagement.WebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitatManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        [HttpGet]
        [Route("GetForms")]
        public IEnumerable<SelectListItem> GetForms()
        {
            IEnumerable<PermitFormScreenDesignTemplateBE> listPermitFormScreenDesignTemplate = FormLogic.BlockFetchPermitFormScreenDesignTemplate(1, Int32.MaxValue, out int totalRecords, "");
            List<SelectListItem> forms = new List<SelectListItem>();
            forms = listPermitFormScreenDesignTemplate.Select(m => new SelectListItem()
            {
                Text = m.Design,
                Value = m.FormID.ToString()
            }).ToList();

            forms.Insert(0, new SelectListItem { Text = "--Select Form--", Value = "-1" });
            return forms;
        }

        [HttpGet]
        [Route("GetFormHtml/{formID:int}/{isRenderForDragnDrop:bool}")]
        public string GetFormHtml(int formID, bool isRenderForDragnDrop)
        {
            List<PermitFormScreenDesignTemplateDetailBE> templateDetails = FormLogic.FetchAllPermitFormScreenDesignTemplateDetail(formID);
            List<TemplateFormFieldDataBE> templateFormFieldData = FormLogic.FetchAllTemplateFormFieldData(formID);
            FormDesignTemplateModelBE model = new FormDesignTemplateModelBE(templateDetails, templateFormFieldData);
            model.FormID = formID;
            model.RenderForDragnDrop = isRenderForDragnDrop;
            return model.FormSectionFields();
        }
    }
}