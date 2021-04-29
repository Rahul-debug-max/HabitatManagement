using HabitatManagement.BusinessEntities;
using HabitatManagement.WebAPI.Model;
using Microsoft.AspNetCore.Cors;
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
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class FormController : ControllerBase
    {
        [HttpGet]
        [EnableCors("AllowOrigin")]
        [Route("GetForms")]
        public IEnumerable<SelectListItem> GetForms()
        {
            IEnumerable<FormDesignTemplateBE> listFormDesignTemplate = FormLogic.BlockFetchFormDesignTemplate(1, Int32.MaxValue, out int totalRecords, "");
            List<SelectListItem> forms = new List<SelectListItem>();
            forms = listFormDesignTemplate.Select(m => new SelectListItem()
            {
                Text = m.Design,
                Value = m.FormID.ToString()
            }).ToList();

            forms.Insert(0, new SelectListItem { Text = "--Select Form--", Value = "-1" });
            return forms;
        }


        [HttpGet]
        [EnableCors("AllowOrigin")]
        [Route("GetDigitalSignature/{signatureId:int}")]
        public string GetDigitalSignature(int signatureId)
        {
            string signature = FormLogic.GetDigitalSignature(signatureId);
            return signature;
        }


        [HttpGet]
        [EnableCors("AllowOrigin")]
        [Route("GetFormHtml/{formID:int}/{surrogate:int}/{isRenderForDragnDrop:bool}")]
        public string GetFormHtml(int formID, int surrogate, bool isRenderForDragnDrop)
        {
            List<FormDesignTemplateDetailBE> templateDetails = FormLogic.FetchAllFormDesignTemplateDetail(formID);
            List<TemplateFormFieldDataBE> templateFormFieldData = new List<TemplateFormFieldDataBE>();
            if (surrogate > 0)
            {
                templateFormFieldData = FormLogic.FetchAllTemplateFormFieldData(formID, surrogate);
            }
            FormDesignTemplateModelBE model = new FormDesignTemplateModelBE(templateDetails, templateFormFieldData);
            model.FormID = formID;
            model.Surrogate = surrogate;
            model.RenderForDragnDrop = isRenderForDragnDrop;
            return model.FormSectionFields();
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [EnableCors("AllowOrigin")]
        [Route("SaveFormData")]
        public bool SaveFormData([FromForm] string data)
        {
            bool success = true;
            try
            {
                List<TemplateFormFieldDataBE> templateFormFieldDatas = JsonConvert.DeserializeObject<List<TemplateFormFieldDataBE>>(data);
                if (templateFormFieldDatas != null)
                {
                    int formDataSurrogate = Functions.ToInt(Request.Form["surrogate"]);                    
                    int formID = templateFormFieldDatas.Select(m => m.FormID).Distinct().FirstOrDefault();
                    if (formDataSurrogate <= 0)
                    {
                        formDataSurrogate = FormLogic.GetMaxProjectFormSurroagate() + 1;
                        templateFormFieldDatas.ForEach(m => m.Surrogate = formDataSurrogate);
                        templateFormFieldDatas.ForEach(m => m.CreationDate = DateTime.Now);
                    }
                    else
                    {
                        templateFormFieldDatas.ForEach(m => m.Surrogate = formDataSurrogate);
                        List<TemplateFormFieldDataBE> templateFormFieldDataValue = FormLogic.FetchAllTemplateFormFieldData(formID, formDataSurrogate);   
                        if(templateFormFieldDataValue != null && templateFormFieldDataValue.Count > 0)
                        {
                            templateFormFieldDatas.ForEach(m => m.CreationDate = templateFormFieldDataValue[0].CreationDate);
                        }  
                    }
                    foreach (var templateFormFieldDataBE in templateFormFieldDatas)
                    {
                        string digitalSignatureImage64BitString = templateFormFieldDataBE.DigitalSignatureImage64BitString;
                        string signatureID = templateFormFieldDataBE.FieldValue;
                        if (templateFormFieldDataBE.FieldType == FormFieldType.Signature.ToString())
                        {
                            int digitalSignatureSurrogate = 0;
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
                                FormLogic.AddDigitalSignature(digitalSignature, out digitalSignatureSurrogate);
                            }
                            if (digitalSignatureSurrogate > 0)
                            {
                                templateFormFieldDataBE.FieldValue = digitalSignatureSurrogate.ToString();
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