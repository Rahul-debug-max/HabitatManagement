using HabitatManagement.BusinessEntity;
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
        string _connectionstring = string.Empty;
       //private readonly ILogger<HomeController> _logger;

       //public HomeController(ILogger<HomeController> logger)
       //{
       //    _logger = logger;
       //}

       IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionstring = _configuration["ConnectionStrings:DefaultConnection"];
        }

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
                    "CreatedDateTime",
                    "LastUpdatedDateTime",
                    "CreatedBy",
                    "UpdatedBy"
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
                IEnumerable<PermitFormScreenDesignTemplateBE> listPermitFormScreenDesignTemplate = BlockFetch(page, rows, out int totalRecords, searchInput);

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
                //var jsonResult = Json(jsonData, JsonRequestBehavior.AllowGet);
                //jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(jsonData);
            }
        }

        public List<PermitFormScreenDesignTemplateBE> BlockFetch(int pageIndex, int pageSize, out int totalRecords, string searchForm)
        {
            totalRecords = 0;
            List<PermitFormScreenDesignTemplateBE> list = new List<PermitFormScreenDesignTemplateBE>();

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplate_BlockFetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("SearchForm", searchForm ?? string.Empty);
                cmd.Parameters.AddWithValue("PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("PageSize", pageSize);
                cmd.Parameters.Add("RecordCount", SqlDbType.Int, 8);
                cmd.Parameters["RecordCount"].Direction = ParameterDirection.Output;
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToBE(sqlDataReader));
                    }
                }
                if (cmd.Parameters["RecordCount"].Value != DBNull.Value)
                {
                    totalRecords = Convert.ToInt32(cmd.Parameters["RecordCount"].Value);
                }
            }
            return list;
        }

        private PermitFormScreenDesignTemplateBE ToBE(SqlDataReader sqlDataReader)
        {
            PermitFormScreenDesignTemplateBE designTemplate = new PermitFormScreenDesignTemplateBE();
            designTemplate.FormID = Convert.ToInt32(sqlDataReader["FormID"]);
            designTemplate.Design = Convert.ToString(sqlDataReader["Design"]);
            designTemplate.Description = Convert.ToString(sqlDataReader["Description"]);
            designTemplate.Active = Convert.ToBoolean(sqlDataReader["Active"]);
            designTemplate.CreatedDateTime = Convert.ToDateTime(sqlDataReader["CreatedDateTime"]);
            designTemplate.LastUpdatedDateTime = Convert.ToDateTime(sqlDataReader["LastUpdatedDateTime"]);
            designTemplate.UpdatedBy = Convert.ToString(sqlDataReader["UpdatedBy"]);
            designTemplate.CreatedBy = Convert.ToString(sqlDataReader["CreatedBy"]);
            return designTemplate;
        }
    }
}