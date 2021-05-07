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
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetProjectColumnNames()
        {
            try
            {
                string[] columnNames =
                {
                    "ID",
                    "Project",
                    "Description",
                    "Manager Name",
                    "Created Date"
            };
                return this.Json(new { columnNames });
            }
            catch (Exception ex)
            {
                return this.Json(new { ErrorMessage = ex.Message });
            }
        }

        public async Task<IActionResult> ProjectBlockFetch(string sidx, string sord, int page = 1, int rows = 10)
        {
            var jsonData = new
            {
                total = 0,
                page,
                records = 0,
                rows = new List<ProjectModel>()
            };

            try
            {
                IEnumerable<ProjectBE> listFormDesignTemplate = FormLogic.BlockFetchProject(page, rows, out int totalRecords);

                if (listFormDesignTemplate == null)
                {
                    return Json(jsonData);
                }
                else
                {
                    var resultFormTemplate = (from o in listFormDesignTemplate
                                              select new ProjectModel
                                              {
                                                  ID = o.ID,
                                                  Project = o.Project,
                                                  Description = o.Description,
                                                  Manager = o.Manager,
                                                  CreationDate = o.CreatedDateTime.ToString()
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

        public async Task<ActionResult> EditProject(int projectID)
        {
            ProjectModel model = new ProjectModel();

            ProjectBE project = FormLogic.FetchProject(projectID);
            if (Functions.IsNull(project))
            {
                project = new ProjectBE();
            }
            BusinessEntityHelper.ConvertBEToBEForUI<ProjectBE, ProjectModel>(project, model);
            using (var httpClient = new HttpClient())
            {
                string url = DBConfiguration.WebAPIHostingURL;
                if (!string.IsNullOrWhiteSpace(url))
                {
                    string webAPIURL = string.Empty;

                    webAPIURL = string.Format("{0}form/GetForms", url);

                    using (var response = await httpClient.GetAsync(webAPIURL))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        List<SelectListItem> forms = JsonConvert.DeserializeObject<List<SelectListItem>>(apiResponse);
                        ViewData["FormList"] = forms;
                    }
                }
            }
            List<ProjectFormBE> list = FormLogic.BlockFetchProjectForm(projectID, 1, int.MaxValue, out int totalRecords);
            
            if(list != null)
            {
                model.ProjectFormList = list.Select(m => m.FormId).ToList();
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult EditProject(ProjectModel model)
        {
            bool success = false;
            int id = 0;

            ProjectBE project = FormLogic.FetchProject(model.ID);
            if (Functions.IsNull(project))
            {
                project = new ProjectBE();
                BusinessEntityHelper.ConvertBEToBEForUI<ProjectModel, ProjectBE>(model, project);
                project.CreatedDateTime = DateTime.Now;
                project.LastUpdatedDateTime = DateTime.Now;
                project.CreatedBy = "Habitat";
                project.UpdatedBy = "Habitat";
                success = FormLogic.AddProject(project, out id);
            }
            else
            {
                project.Project = model.Project;
                project.Description = model.Description;
                project.Manager = model.Manager;
                project.LastUpdatedDateTime = DateTime.Now;
                project.UpdatedBy = "Habitat";
                success = FormLogic.UpdateProject(project);
            }


            if (success)
            {
                int projectID = model.ID > 0 ? model.ID : id;
                success = FormLogic.SaveProjectForm(projectID, string.Join(',', model.ProjectFormList));
            }

            return Json(new { success, id });
        }
    }
}
