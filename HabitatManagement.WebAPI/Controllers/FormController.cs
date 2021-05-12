using HabitatManagement.Business;
using HabitatManagement.WebAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HabitatManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class FormController : ControllerBase
    {
        [HttpGet]
        [EnableCors("AllowOrigin")]
        [Route("GetForms/{projectId:int?}")]
        public IEnumerable<SelectListItem> GetForms(int? projectId)
        {
            IEnumerable<FormDesignTemplateBE> listFormDesignTemplate = FormLogic.BlockFetchFormDesignTemplate(1, Int32.MaxValue, out int totalRecords, "", projectId);
            List<SelectListItem> forms = new List<SelectListItem>();
            forms = listFormDesignTemplate.Select(m => new SelectListItem()
            {
                Text = m.Design,
                Value = m.FormID.ToString()
            }).ToList();

            //forms.Insert(0, new SelectListItem { Text = "--Select Form--", Value = "-1" });
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
        [Route("GetFormHtml/{formID:int}/{isRenderForDragnDrop:bool}/{surrogate:int?}")]
        public string GetFormHtml(int formID, bool isRenderForDragnDrop, int? surrogate)
        {
            List<FormDesignTemplateDetailBE> templateDetails = FormLogic.FetchAllFormDesignTemplateDetail(formID);
            List<TemplateFormFieldDataBE> templateFormFieldData = new List<TemplateFormFieldDataBE>();
            if (surrogate.HasValue && surrogate > 0)
            {
                templateFormFieldData = FormLogic.FetchAllTemplateFormFieldData(formID, surrogate.Value);
            }
            FormDesignTemplateModelBE model = new FormDesignTemplateModelBE(templateDetails, templateFormFieldData);
            model.FormID = formID;
            model.Surrogate = surrogate.HasValue ? surrogate.Value : 0;
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
                using (var scope = new TransactionScope())
                {
                    List<TemplateFormFieldDataBE> templateFormFieldDatas = JsonConvert.DeserializeObject<List<TemplateFormFieldDataBE>>(data);
                    if (templateFormFieldDatas != null)
                    {
                        int referenceNumber = Functions.ToInt(Request.Form["surrogate"]);
                        int projectID = Functions.ToInt(Request.Form["projectID"]);
                        int formID = templateFormFieldDatas.Select(m => m.FormID).Distinct().FirstOrDefault();
                        if (referenceNumber <= 0)
                        {
                            SubmittedFormBE submittedForm = new SubmittedFormBE();
                            submittedForm.ProjectId = projectID;
                            submittedForm.FormId = formID;
                            submittedForm.Status = SubmittedFormStatusField.Submitted;
                            submittedForm.CreatedDateTime = DateTime.Now;
                            submittedForm.LastUpdatedDateTime = DateTime.Now;
                            submittedForm.CreatedBy = "RSK";
                            submittedForm.UpdatedBy = "RSK";
                            success = FormLogic.AddSubmittedForm(submittedForm, out referenceNumber);
                            templateFormFieldDatas.ForEach(m => m.ReferenceNumber = referenceNumber);                            
                        }
                        else
                        {
                            templateFormFieldDatas.ForEach(m => m.ReferenceNumber = referenceNumber);
                            List<TemplateFormFieldDataBE> templateFormFieldDataValue = FormLogic.FetchAllTemplateFormFieldData(formID, referenceNumber);                            
                        }
                        if (success)
                        {
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

                    if (success)
                    {
                        scope.Complete();
                    }                   
                }
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }
    }
}