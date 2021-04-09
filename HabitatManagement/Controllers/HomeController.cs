using HabitatManagement.BusinessEntities;
using HabitatManagement.BusinessLogic;
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

namespace HabitatManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult GetFormDesignerColumnNames()
        {
            try
            {
                // Report bundles jqgrid column names
                string[] columnNames =
                {
                    "Id",
                    "Design",
                    "Description",
                    "Active",
                    "Created Date",
                    "Updated Date",
                    "Created By",
                    "Updated By"
            };
                return this.Json(new { columnNames });
            }
            catch (Exception ex)
            {
                return this.Json(new { ErrorMessage = ex.Message });
            }
        }

        public ActionResult GetFormDesignerData(string searchInput, string sidx, string sord, int page = 1, int rows = 10)
        {
            var jsonData = new
            {
                total = 0,
                page,
                records = 0,
                rows = new List<PermitFormScreenDesignTemplateModel>()
            };

            try
            {
                IEnumerable<PermitFormScreenDesignTemplateBE> listPermitFormScreenDesignTemplate = FormLogic.BlockFetchPermitFormScreenDesignTemplate(page, rows, out int totalRecords, searchInput);

                if (listPermitFormScreenDesignTemplate == null)
                {
                    return Json(jsonData);
                }
                else
                {
                    var resultFormTemplate = (from permitFormScreenDesignTemplateObj in listPermitFormScreenDesignTemplate
                                            select new PermitFormScreenDesignTemplateModel
                                            {
                                                FormID = permitFormScreenDesignTemplateObj.FormID.ToString(),
                                                Design = permitFormScreenDesignTemplateObj.Design,
                                                Description = permitFormScreenDesignTemplateObj.Description,
                                                Active = permitFormScreenDesignTemplateObj.Active.ToString(),
                                                CreatedDateTime = permitFormScreenDesignTemplateObj.CreatedDateTime.ToString(),
                                                LastUpdatedDateTime = permitFormScreenDesignTemplateObj.LastUpdatedDateTime.ToString(),
                                                CreatedBy = permitFormScreenDesignTemplateObj.CreatedBy,
                                                UpdatedBy = permitFormScreenDesignTemplateObj.UpdatedBy
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
    }
}