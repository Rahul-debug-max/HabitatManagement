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
using System.Net.Http;

namespace HabitatManagement.Controllers
{
    public class FormController : Controller
    {
        public async Task<IActionResult> Index()
        {
            FormDesignTemplateModelBE model = new FormDesignTemplateModelBE();
            using (var httpClient = new HttpClient())
            {
                string url = DBConfiguration.WebAPIHostingURL;
                if (!string.IsNullOrWhiteSpace(url))
                {
                    string webAPIURL = string.Format("{0}form/GetForms", url);
                    using (var response = await httpClient.GetAsync(webAPIURL))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        List<SelectListItem> forms = JsonConvert.DeserializeObject<List<SelectListItem>>(apiResponse);
                        ViewData["FormList"] = forms;
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> SaveFormFeedback(string data)
        {
            bool success = true;
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    using (var httpClient = new HttpClient())
                    {
                        string url = DBConfiguration.WebAPIHostingURL;
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            string webAPIURL = string.Format("{0}form/SaveFormData/", url);
                            var contentData = new StringContent(data);
                           // var contentData = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                           // HttpResponseMessage response = httpClient.PostAsync(webAPIURL, contentData).Result;
                           // ViewBag.Message = response.Content.ReadAsStringAsync().Result;

                            using (var response = await httpClient.PostAsync(webAPIURL, contentData))
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                            }
                        }
                    }

                    //List<TemplateFormFieldDataBE> templateFormFieldDatas = JsonConvert.DeserializeObject<List<TemplateFormFieldDataBE>>(data);

                    //foreach (var templateFormFieldDataBE in templateFormFieldDatas)
                    //{
                    //    string digitalSignatureImage64BitString = templateFormFieldDataBE.DigitalSignatureImage64BitString;
                    //    string signatureID = templateFormFieldDataBE.FieldValue;
                    //    if (templateFormFieldDataBE.FieldType == FormFieldType.Signature.ToString())
                    //    {
                    //        int surrogate = 0;
                    //        DigitalSignatureBE digitalSignature = FormLogic.FetchDigitalSignature(Functions.ToInt(signatureID));
                    //        if (digitalSignature != null)
                    //        {
                    //            digitalSignature.DigitalSignatureImage64BitString = digitalSignatureImage64BitString ?? string.Empty;
                    //            digitalSignature.LastUpdatedDate = DateTime.Now;
                    //            FormLogic.UpdateDigitalSignature(digitalSignature);
                    //        }
                    //        else if (!string.IsNullOrWhiteSpace(digitalSignatureImage64BitString))
                    //        {
                    //            digitalSignature = new DigitalSignatureBE();
                    //            digitalSignature.CreationDateTime = DateTime.Now;
                    //            digitalSignature.LastUpdatedDate = DateTime.Now;
                    //            digitalSignature.DigitalSignatureImage64BitString = digitalSignatureImage64BitString ?? string.Empty;
                    //            FormLogic.AddDigitalSignature(digitalSignature, out surrogate);
                    //        }
                    //        if (surrogate > 0)
                    //        {
                    //            templateFormFieldDataBE.FieldValue = surrogate.ToString();
                    //        }
                    //    }

                    //    if (templateFormFieldDataBE.FormID > 0 && templateFormFieldDataBE.Field > 0)
                    //    {
                    //        success = FormLogic.SaveTemplateFormFieldData(templateFormFieldDataBE);
                    //    }
                    //}
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