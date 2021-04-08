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

namespace HabitatManagement.Controllers
{
    public class HomeController : Controller
    {
        string _connectionstring = string.Empty;
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
                IEnumerable<PermitFormScreenDesignTemplateBE> listPermitFormScreenDesignTemplate = BlockFetchPermitFormScreenDesignTemplate(page, rows, out int totalRecords, searchInput);

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



    // ***************************************** Data Access Section ********************************************** //

        public List<PermitFormScreenDesignTemplateBE> BlockFetchPermitFormScreenDesignTemplate(int pageIndex, int pageSize, out int totalRecords, string searchForm)
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
                        list.Add(ToPermitFormScreenDesignTemplateBE(sqlDataReader));
                    }
                }
                if (cmd.Parameters["RecordCount"].Value != DBNull.Value)
                {
                    totalRecords = Convert.ToInt32(cmd.Parameters["RecordCount"].Value);
                }
            }
            return list;
        }

        public PermitFormScreenDesignTemplateBE FetchPermitFormScreenDesignTemplate(int formId)
        {
            PermitFormScreenDesignTemplateBE o = null;

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplate_Fetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormID", formId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        o = ToPermitFormScreenDesignTemplateBE(reader);
                }
            }
            return o;
        }

        public bool AddPermitFormScreenDesignTemplate(PermitFormScreenDesignTemplateBE o, out int id)
        {
            bool success = false;
            id = 0;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplate_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                FromPermitFormScreenDesignTemplateBE(ref cmd, o);
                cmd.Parameters.Add("ErrorOccured", SqlDbType.Bit);
                cmd.Parameters["ErrorOccured"].Direction = ParameterDirection.Output;
                cmd.Parameters["FormID"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["ErrorOccured"].Value != DBNull.Value)
                {
                    success = Convert.ToBoolean(cmd.Parameters["ErrorOccured"].Value);
                }
                if (cmd.Parameters["FormID"].Value != DBNull.Value)
                {
                    id = Convert.ToInt32(cmd.Parameters["FormID"].Value);
                }
            }
            return success;
        }

        public bool UpdatePermitFormScreenDesignTemplate(PermitFormScreenDesignTemplateBE o)
        {
            bool success = false;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplate_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                FromPermitFormScreenDesignTemplateBE(ref cmd, o);
                cmd.Parameters.Add("ErrorOccured", SqlDbType.Bit);
                cmd.Parameters["ErrorOccured"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["ErrorOccured"].Value != DBNull.Value)
                {
                    success = Convert.ToBoolean(cmd.Parameters["ErrorOccured"].Value);
                }
            }
            return success;
        }

        public List<PermitFormScreenDesignTemplateDetailBE> FetchAllPermitFormScreenDesignTemplateDetail(int formId)
        {
            List<PermitFormScreenDesignTemplateDetailBE> list = new List<PermitFormScreenDesignTemplateDetailBE>();
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplateDetail_FetchAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormId", formId);
                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        list.Add(ToPermitFormScreenDesignTemplateDetailBE(sqlDataReader));
                    }
                }
            }
            return list;
        }

        public PermitFormScreenDesignTemplateDetailBE FetchPermitFormScreenDesignTemplateDetail(int formId, int field)
        {
            PermitFormScreenDesignTemplateDetailBE o = null;
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplateDetail_Fetch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FormID", formId);
                cmd.Parameters.AddWithValue("Field", field);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        o = ToPermitFormScreenDesignTemplateDetailBE(reader);
                }
            }

            return o;
        }

        public void AddPermitFormScreenDesignTemplateDetail(PermitFormScreenDesignTemplateDetailBE o)
        {
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplateDetail_Add", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                FromPermitFormScreenDesignTemplateDetailBE(ref cmd, o);
                cmd.ExecuteNonQuery();

            }
        }


        public void UpdatePermitFormScreenDesignTemplateDetail(PermitFormScreenDesignTemplateDetailBE o)
        {
            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_PermitFormScreenDesignTemplateDetail_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                FromPermitFormScreenDesignTemplateDetailBE(ref cmd, o);
                cmd.Parameters.AddWithValue("Field", o.Field);
                cmd.ExecuteNonQuery();

            }
        }

        #region Private Methods

        private PermitFormScreenDesignTemplateDetailBE ToPermitFormScreenDesignTemplateDetailBE(SqlDataReader rdr)
        {
            PermitFormScreenDesignTemplateDetailBE o = new PermitFormScreenDesignTemplateDetailBE();
            o.FormID = Convert.ToInt32(rdr["FormID"]);
            o.Field = Convert.ToInt32(rdr["Field"]);
            o.FieldName = Convert.ToString(rdr["FieldName"]);
            o.FieldType = (FormFieldType)Convert.ToInt32(rdr["FieldType"]);
            o.Section = (PromptFormSectionField)Convert.ToInt32(rdr["Section"]);
            o.Sequence = Convert.ToInt32(rdr["Sequence"]);
            return o;
        }

        private void FromPermitFormScreenDesignTemplateDetailBE(ref SqlCommand cmd, PermitFormScreenDesignTemplateDetailBE o)
        {
            cmd.Parameters.AddWithValue("FormID", o.FormID);
            cmd.Parameters.AddWithValue("FieldName", o.FieldName);
            cmd.Parameters.AddWithValue("FieldType", (int)o.FieldType);
            cmd.Parameters.AddWithValue("Section", (int)o.Section);
            cmd.Parameters.AddWithValue("Sequence", o.Sequence);
        }

        private PermitFormScreenDesignTemplateBE ToPermitFormScreenDesignTemplateBE(SqlDataReader sqlDataReader)
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

        private void FromPermitFormScreenDesignTemplateBE(ref SqlCommand cmd, PermitFormScreenDesignTemplateBE o)
        {
            cmd.Parameters.AddWithValue("FormID", o.FormID);
            cmd.Parameters.AddWithValue("Design", o.Design);
            cmd.Parameters.AddWithValue("Description", o.Description);
            cmd.Parameters.AddWithValue("Active", o.Active);
            cmd.Parameters.AddWithValue("CreatedDateTime", o.CreatedDateTime);
            cmd.Parameters.AddWithValue("LastUpdatedDateTime", o.LastUpdatedDateTime);
            cmd.Parameters.AddWithValue("CreatedBy", o.CreatedBy);
            cmd.Parameters.AddWithValue("UpdatedBy", o.UpdatedBy);
        }

        #endregion

    }
}