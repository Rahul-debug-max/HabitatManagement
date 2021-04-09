﻿using HabitatManagement.BusinessEntities;
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

        public ActionResult PermitFormScreenDesignTemplate(int formID)
        {
            PermitFormScreenDesignTemplateBE model = new PermitFormScreenDesignTemplateBE();

            if (formID <= 0)
            {
                model.Active = true;
            }
            else
            {
                PermitFormScreenDesignTemplateBE conditionFeedbackTemplate = FormLogic.FetchPermitFormScreenDesignTemplate(formID);
                if (conditionFeedbackTemplate != null)
                {
                    model.FormID = conditionFeedbackTemplate.FormID;
                    model.Design = conditionFeedbackTemplate.Design;
                    model.Description = conditionFeedbackTemplate.Description;
                    model.Active = conditionFeedbackTemplate.Active;
                }
            }
            return View("PermitForm", model);
        }

        [HttpPost]
        public ActionResult PermitFormScreenDesignTemplate(PermitFormScreenDesignTemplateBE model)
        {

            bool success = false;
            int id = 0;

            PermitFormScreenDesignTemplateBE permitFormScreenDesignTemplate = FormLogic.FetchPermitFormScreenDesignTemplate(model.FormID);
            if (permitFormScreenDesignTemplate == null)
            {
                permitFormScreenDesignTemplate = new PermitFormScreenDesignTemplateBE();
            }
            permitFormScreenDesignTemplate.Design = model.Design.ToUpper();
            permitFormScreenDesignTemplate.Description = model.Description;
            permitFormScreenDesignTemplate.Active = model.Active;

            if (model.FormID <= 0)
            {
                permitFormScreenDesignTemplate.CreatedBy = "Habitat";
                permitFormScreenDesignTemplate.CreatedDateTime = DateTime.Now;
                permitFormScreenDesignTemplate.LastUpdatedDateTime = DateTime.Now;
                success = FormLogic.AddPermitFormScreenDesignTemplate(permitFormScreenDesignTemplate, out id);
            }
            else
            {
                permitFormScreenDesignTemplate.UpdatedBy = "Habitat";
                permitFormScreenDesignTemplate.LastUpdatedDateTime = DateTime.Now;
                success = FormLogic.UpdatePermitFormScreenDesignTemplate(permitFormScreenDesignTemplate);
            }
            return Json(new { success, id });
        }

        public ActionResult PermitFormScreenDesignTemplateDetail(int formID)
        {
            PermitFormScreenDesignTemplateDetailModelBE model = new PermitFormScreenDesignTemplateDetailModelBE();
           
            model.TemplateDetails = new List<PermitFormScreenDesignTemplateDetailBE>();

            PermitFormScreenDesignTemplateBE permitFormScreenDesignTemplate = FormLogic.FetchPermitFormScreenDesignTemplate(formID);
            if (permitFormScreenDesignTemplate != null)
            {
                model.FormID = permitFormScreenDesignTemplate.FormID;
                model.Design = permitFormScreenDesignTemplate.Design;
                model.Description = permitFormScreenDesignTemplate.Description;
                model.Active = permitFormScreenDesignTemplate.Active;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult PermitFormScreenDesignTemplateDetail(PermitFormScreenDesignTemplateDetailModelBE model)
        {
            bool success = false;
            //success = FormLogic.Save(model.TemplateDetails);
            return new JsonResult(new { Success = success, TemplateDetails = success ? model.TemplateDetails : null });
        }


        public ActionResult PermitFormTemplateFields(int formID)
        {
            List<PermitFormScreenDesignTemplateDetailBE> templateDetails = FormLogic.FetchAllPermitFormScreenDesignTemplateDetail(formID);            
            
            return View();
        }

    }
}