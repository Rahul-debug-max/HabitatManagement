using HabitatManagement.BusinessEntities;
using HabitatManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace HabitatManagement.Controllers
{
    public class FormController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<PermitFormScreenDesignTemplateBE> listPermitFormScreenDesignTemplate = FormLogic.BlockFetchPermitFormScreenDesignTemplate(1, Int32.MaxValue, out int totalRecords, "");
            List<SelectListItem> forms = new List<SelectListItem>();
            forms = listPermitFormScreenDesignTemplate.Select(m => new SelectListItem()
            {
                Text = m.Design,
                Value = m.FormID.ToString()
            }).ToList();

            forms.Insert(0, new SelectListItem { Text = "--Select Form--", Value = "-1" });
            ViewData["FormList"] = forms;
            return View();
        }

        public ActionResult SaveFormFeedback(string data)
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
            catch(Exception ex)
            {
                success = false;
            }

            return Json(new { Success = success });
        }
    }
}