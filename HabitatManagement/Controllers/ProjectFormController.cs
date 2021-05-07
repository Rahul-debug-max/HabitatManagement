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
        public async Task<IActionResult> Index(int? projectId)
        {
            FormDesignTemplateModelBE model = new FormDesignTemplateModelBE();
            using (var httpClient = new HttpClient())
            {
                string url = DBConfiguration.WebAPIHostingURL;
                if (!string.IsNullOrWhiteSpace(url))
                {
                    string webAPIURL = string.Empty;
                    if (projectId != null)
                    {
                        webAPIURL = string.Format("{0}form/GetForms/{1}", url, projectId.Value);
                    }
                    else
                    {
                        webAPIURL = string.Format("{0}form/GetForms", url);
                    }
                    using (var response = await httpClient.GetAsync(webAPIURL))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        List<SelectListItem> forms = JsonConvert.DeserializeObject<List<SelectListItem>>(apiResponse);
                        ViewData["FormList"] = forms;
                    }
                }
                ViewData["SaveFormDataURL"] = string.Format("{0}form/SaveFormData", url);
            }

            if (projectId != null)
            {
                ProjectBE projectBE = FormLogic.FetchProject(projectId.Value);
                if (projectBE != null)
                {
                    model.Project = projectBE.Project;
                    model.ProjectDescription = projectBE.Description;
                    model.ProjectId = projectId.Value;
                }
            }

            bool isAjaxRequest = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjaxRequest)
            {
                return PartialView(model);
            }
            else
            {
                return View(model);
            }
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

        public async Task<IActionResult> GetProjectFormListData(int formID, int projectID, string sidx, string sord, int page = 1, int rows = 10)
        {
            var jsonData = new
            {
                total = 0,
                page,
                records = 0,
                rows = new List<ProjectSubmitedFormListModal>()
            };

            try
            {
                List<SubmittedFormBE> list = FormLogic.BlockFetchSubmittedForm(page, rows, out int totalRecords, projectID, formID);

                if (list == null)
                {
                    return Json(jsonData);
                }
                else
                {
                    var resultFormTemplate = (from obj in list
                                              select new ProjectSubmitedFormListModal
                                              {
                                                  Surrogate = obj.ReferenceNumber.ToString(),
                                                  FormSurrogate = obj.FormId.ToString(),
                                                  Design = obj.GetCustomDataValue<string>("Design"),
                                                  Description = obj.GetCustomDataValue<string>("DesignDescription"),
                                                  CreationDate = obj.CreatedDateTime.ToString()
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

        public async Task<IActionResult> ProjectFormCreation(int? formID, int? surrogate, int? projectID)
        {
            ProjectFormCreationModel model = new ProjectFormCreationModel();
            model.FormID = formID ?? 0;
            model.Surrogate = surrogate ?? 0;
            using (var httpClient = new HttpClient())
            {
                string url = DBConfiguration.WebAPIHostingURL;
                if (!string.IsNullOrWhiteSpace(url))
                {
                    string webAPIURL = string.Empty;
                    if (projectID != null)
                    {
                        webAPIURL = string.Format("{0}form/GetForms/{1}", url, projectID.Value);
                    }
                    else
                    {
                        webAPIURL = string.Format("{0}form/GetForms", url);
                    }
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

    public class ProjectSubmitedFormListModal : SubmittedFormBE
    {
        public string Surrogate { get; set; }

        public string FormSurrogate { get; set; }

        public string CreationDate { get; set; }

        public string Design { get; set; }

        public string Description { get; set; }

    }
}
