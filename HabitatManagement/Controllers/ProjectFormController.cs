using HabitatManagement.Business;
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
    public class ProjectFormController : Controller
    {
        public async Task<IActionResult> Index()
        {
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
                ViewData["SaveFormDataURL"] = string.Format("{0}form/SaveFormData", url);
            }
            return View();
        }

        public JsonResult GetProjectFormListColumnNames()
        {
            try
            {
                string[] columnNames =
                {
                    "Form ID",
                    "Reference Number",
                    "Design",
                    "Description",
                    "Created Date"
            };
                return this.Json(new { columnNames });
            }
            catch (Exception ex)
            {
                return this.Json(new { ErrorMessage = ex.Message });
            }
        }

        public async Task<IActionResult> GetProjectFormListData(int formID, string sidx, string sord, int page = 1, int rows = 10)
        {
            var jsonData = new
            {
                total = 0,
                page,
                records = 0,
                rows = new List<CreatedFormListModal>()
            };

            try
            {
                List<TemplateFormFieldDataBE> list = FormLogic.BlockFetchByForm(page, rows, out int totalRecords, formID);

                if (list == null)
                {
                    return Json(jsonData);
                }
                else
                {
                    var resultFormTemplate = (from obj in list
                                              select new CreatedFormListModal
                                              {
                                                  Surrogate = obj.Surrogate.ToString(),
                                                  FormID = obj.FormID.ToString(),
                                                  Design = obj.Design,
                                                  Description = obj.Description,
                                                  CreationDate = obj.CreationDate.ToString()
                                              }).ToList();

                    var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

                    jsonData = new
                    {
                        total = totalPages,
                        page,
                        records = totalRecords,
                        rows = resultFormTemplate
                    };
                }

                var jsonResult = Json(jsonData);
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(jsonData);
            }
        }

        public async Task<IActionResult> ProjectFormCreation(int? formID, int? surrogate)
        {
            ProjectFormCreationModel model = new ProjectFormCreationModel();
            model.FormID = formID ?? 0;
            model.Surrogate = surrogate ?? 0;
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

    }

    public class CreatedFormListModal
    {
        public string Surrogate { get; set; }

        public string FormID { get; set; }

        public string Design { get; set; }

        public string Description { get; set; }

        public string CreationDate { get; set; }
    }
}
