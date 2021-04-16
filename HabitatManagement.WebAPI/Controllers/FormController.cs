using HabitatManagement.BusinessEntities;
using HabitatManagement.WebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        [HttpPost]
        [Route("SaveFormData")]
        public bool SaveFormData(string data)
        {
            bool success = true;
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    List<TemplateFormFieldDataBE> templateFormFieldDatas = JsonConvert.DeserializeObject<List<TemplateFormFieldDataBE>>(data);

                    foreach (var templateFormFieldDataBE in templateFormFieldDatas)
                    {
                        string digitalSignatureImage64BitString = templateFormFieldDataBE.DigitalSignatureImage64BitString;
                        string signatureID = templateFormFieldDataBE.FieldValue;
                        if (templateFormFieldDataBE.FieldType == FormFieldType.Signature.ToString())
                        {
                            int surrogate = 0;
                            DigitalSignatureBE digitalSignature = FormLogic.FetchDigitalSignature(Functions.ToInt(signatureID));
                            if (digitalSignature != null)
                            {
                                digitalSignature.DigitalSignatureImage64BitString = digitalSignatureImage64BitString ?? string.Empty;
                                digitalSignature.LastUpdatedDate = DateTime.Now;
                                FormLogic.UpdateDigitalSignature(digitalSignature);
                            }
                            else if (!string.IsNullOrWhiteSpace(digitalSignatureImage64BitString))
                            {
                                digitalSignature = new DigitalSignatureBE();
                                digitalSignature.CreationDateTime = DateTime.Now;
                                digitalSignature.LastUpdatedDate = DateTime.Now;
                                digitalSignature.DigitalSignatureImage64BitString = digitalSignatureImage64BitString ?? string.Empty;
                                FormLogic.AddDigitalSignature(digitalSignature, out surrogate);
                            }
                            if (surrogate > 0)
                            {
                                templateFormFieldDataBE.FieldValue = surrogate.ToString();
                            }
                        }

                        if (templateFormFieldDataBE.FormID > 0 && templateFormFieldDataBE.Field > 0)
                        {
                            success = FormLogic.SaveTemplateFormFieldData(templateFormFieldDataBE);
                        }
                    }
                }
            }
            catch
            {
                success = false;
            }

            return success;
        }
    }
}