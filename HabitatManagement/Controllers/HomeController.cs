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
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;

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

        public async Task<IActionResult> GetFormDesignerData(string searchInput, string sidx, string sord, int page = 1, int rows = 10)
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
            model.TemplateSectionDetail = FormLogic.FetchAllTemplateFormSection(model.FormID);
            return View(model);
        }


        [HttpPost]
        public ActionResult PermitFormScreenDesignTemplateDetail(PermitFormScreenDesignTemplateDetailModelBE model)
        {
            bool success = false;
            //success = FormLogic.Save(model.TemplateDetails);
            return new JsonResult(new { Success = success, TemplateDetails = success ? model.TemplateDetails : null });
        }

        public ActionResult TemplateSectionList(int formID)
        {
            PermitFormScreenDesignTemplateDetailModelBE model = new PermitFormScreenDesignTemplateDetailModelBE();
            model.TemplateSectionDetail = FormLogic.FetchAllTemplateFormSection(formID);
            return PartialView("TemplateSectionList", model);
        }

        public ActionResult TemplateSection(int formID, string section)
        {
            TemplateFormSectionBE model;

            model = FormLogic.FetchTemplateFormSection(formID, section ?? "");
            if (model == null)
            {
                model = new TemplateFormSectionBE();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult TemplateSection(TemplateFormSectionBE model)
        {

            bool success = false;
            int id = 0;
            bool sectionExist = true;
            TemplateFormSectionBE templateFormSection = FormLogic.FetchTemplateFormSection(model.FormID, model.Section);
            if (templateFormSection == null)
            {
                sectionExist = false;
                templateFormSection = new TemplateFormSectionBE();
            }
            templateFormSection.FormID = model.FormID;
            templateFormSection.Section = model.Section.ToUpper();
            templateFormSection.Description = model.Description;
            if (!sectionExist)
            {
                success = FormLogic.AddTemplateFormSection(templateFormSection);
            }
            else
            {
                success = FormLogic.UpdateTemplateFormSection(templateFormSection);
            }
            return Json(new { success, id });
        }

        public JsonResult DeleteTemplateSection(int formID, string section)
        {
            bool success = false;

            success = FormLogic.DeleteTemplateFormSection(formID, section);

            return new JsonResult(new { Success = success });
        }


        [HttpPost]
        public ActionResult TemplateSectionList(PermitFormScreenDesignTemplateDetailModelBE model)
        {
            bool success = true;

            if (model.TemplateSectionDetail != null && model.TemplateSectionDetail.Count > 0)
            {
                foreach (var se in model.TemplateSectionDetail)
                {
                    if (success)
                    {
                        TemplateFormSectionBE templateFormSection = FormLogic.FetchTemplateFormSection(se.FormID, se.Section);
                        if (templateFormSection != null)
                        {
                            templateFormSection.Sequence = se.Sequence;
                            success = FormLogic.UpdateTemplateFormSection(templateFormSection);
                        }
                    }
                }
            }
            return Json(new { Success = success });
        }

        public ActionResult PermitFormTemplateFields(int formID, bool? isRenderForDragnDrop = null)
        {
            List<PermitFormScreenDesignTemplateDetailBE> templateDetails = FormLogic.FetchAllPermitFormScreenDesignTemplateDetail(formID);
            List<TemplateFormFieldDataBE> templateFormFieldData = FormLogic.FetchAllTemplateFormFieldData(formID);
            FormDesignTemplateModelBE model = new FormDesignTemplateModelBE(templateDetails, templateFormFieldData);
            model.FormID = formID;
            model.RenderForDragnDrop = true;
            if (isRenderForDragnDrop != null)
            {
                model.RenderForDragnDrop = isRenderForDragnDrop.Value;
            }
            return View(model);
        }

        public JsonResult GetFieldDesignerColumnNames()
        {
            try
            {
                string[] columnNames =
                {
                    "Field",
                    "Field Name",
                    "Field Type",
                    "Section",
                    "Sequence"
            };
                return this.Json(new { columnNames });
            }
            catch (Exception ex)
            {
                return this.Json(new { ErrorMessage = ex.Message });
            }
        }

        public ActionResult GetFieldDesignerData(int formID, string sidx, string sord, int page = 1, int rows = 10)
        {
            var jsonData = new
            {
                total = 0,
                page,
                records = 0,
                rows = new List<PermitFormScreenDesignTemplateDetailBE>()
            };

            try
            {
                IEnumerable<PermitFormScreenDesignTemplateDetailBE> list = FormLogic.BlockFetchPermitFormScreenDesignTemplateDetail(formID, page, rows, out int totalRecords);

                if (list == null)
                {
                    return Json(jsonData);
                }
                else
                {
                    var resultFormTemplate = (from obj in list
                                              select new PermitFormScreenDesignTemplateDetailBE
                                              {
                                                  Field = obj.Field,
                                                  FieldName = obj.FieldName,
                                                  FieldTypeValue = obj.FieldType.ToString(),
                                                  Section = obj.Section,
                                                  Sequence = obj.Sequence
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


        public ActionResult EditPermitFormField(int formID, int fieldID)
        {
            PermitFormScreenDesignTemplateDetailBE model;

            model = FormLogic.FetchPermitFormScreenDesignTemplateDetail(formID, fieldID);
            if (model == null)
            {
                model = new PermitFormScreenDesignTemplateDetailBE();
            }

            ViewData["SectionList"] = FormLogic.FetchAllTemplateFormSection(formID).Select(m => new SelectListItem()
            {
                Text = m.Section,
                Value = m.Section
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPermitFormField(PermitFormScreenDesignTemplateDetailBE model)
        {

            bool success = false;
            int id = 0;

            PermitFormScreenDesignTemplateDetailBE permitFormScreenDesignTemplateDetail = FormLogic.FetchPermitFormScreenDesignTemplateDetail(model.FormID, model.Field);
            if (permitFormScreenDesignTemplateDetail == null)
            {
                permitFormScreenDesignTemplateDetail = new PermitFormScreenDesignTemplateDetailBE();
            }
            permitFormScreenDesignTemplateDetail.FormID = model.FormID;
            permitFormScreenDesignTemplateDetail.Field = model.Field;
            permitFormScreenDesignTemplateDetail.FieldName = model.FieldName;
            permitFormScreenDesignTemplateDetail.FieldType = model.FieldType;
            permitFormScreenDesignTemplateDetail.Section = model.Section;
            permitFormScreenDesignTemplateDetail.Sequence = model.Sequence;

            if (model.Field <= 0)
            {
                success = FormLogic.AddPermitFormScreenDesignTemplateDetail(permitFormScreenDesignTemplateDetail);
            }
            else
            {
                success = FormLogic.UpdatePermitFormScreenDesignTemplateDetail(permitFormScreenDesignTemplateDetail);
            }
            return Json(new { success, id });
        }

        public JsonResult DeletePermitFormField(int formID, int fieldID)
        {
            bool success = false;

            success = FormLogic.DeletePermitFormScreenDesignTemplateDetail(formID, fieldID);

            return new JsonResult(new { Success = success });
        }

        public ActionResult GetDigitalSignature(int signatureId)
        {
            string signature = FormLogic.GetDigitalSignature(signatureId);
            return new JsonResult(new { Signature = signature });
        }

        public ActionResult PermitFormScreenDesignTemplateDetailFields(PermitFormScreenDesignTemplateDetailBE formDetail)
        {
            bool success = true;
            if (formDetail != null)
            {
                PermitFormScreenDesignTemplateDetailBE permitFormScreenDesignTemplateDetailBE = FormLogic.FetchPermitFormScreenDesignTemplateDetail(formDetail.FormID, formDetail.Field);
                if (permitFormScreenDesignTemplateDetailBE != null)
                {
                    permitFormScreenDesignTemplateDetailBE.Section = formDetail.Section;
                    permitFormScreenDesignTemplateDetailBE.Sequence = formDetail.Sequence;
                    success = FormLogic.UpdatePermitFormScreenDesignTemplateDetail(permitFormScreenDesignTemplateDetailBE);
                }
            }
            return Json(new { Success = success });
        }
    }
}