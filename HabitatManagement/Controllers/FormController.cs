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
                            string webAPIURL = string.Format("{0}form/SaveFormData", url);
                            List<TemplateFormFieldDataBE> templateFormFieldDatas = JsonConvert.DeserializeObject<List<TemplateFormFieldDataBE>>(data);
                            if (templateFormFieldDatas != null)
                            {
                                var jsonStringData = JsonConvert.SerializeObject(templateFormFieldDatas);
                                var contentData = new StringContent(jsonStringData, System.Text.Encoding.UTF8, "application/json");

                                using (var response = await httpClient.PostAsync(webAPIURL, contentData))
                                {
                                    var apiResponse = response.Content.ReadAsStringAsync().Result;
                                    success = Functions.ToBool(apiResponse);
                                }
                            }
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