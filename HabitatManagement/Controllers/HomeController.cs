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
                rows = new List<FormDesignTemplateModel>()
            };

            try
            {
                IEnumerable<FormDesignTemplateBE> listFormDesignTemplate = FormLogic.BlockFetchFormDesignTemplate(page, rows, out int totalRecords, searchInput);

                if (listFormDesignTemplate == null)
                {
                    return Json(jsonData);
                }
                else
                {
                    var resultFormTemplate = (from o in listFormDesignTemplate
                                              select new FormDesignTemplateModel
                                              {
                                                  FormID = o.FormID.ToString(),
                                                  Design = o.Design,
                                                  Description = o.Description,
                                                  Active = o.Active.ToString(),
                                                  CreatedDateTime = o.CreatedDateTime.ToString(),
                                                  LastUpdatedDateTime = o.LastUpdatedDateTime.ToString(),
                                                  CreatedBy = o.CreatedBy,
                                                  UpdatedBy = o.UpdatedBy
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

        public ActionResult FormDesignTemplate(int formID)
        {
            FormDesignTemplateBE model = new FormDesignTemplateBE();

            if (formID <= 0)
            {
                model.Active = true;
            }
            else
            {
                FormDesignTemplateBE conditionFeedbackTemplate = FormLogic.FetchFormDesignTemplate(formID);
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
        public ActionResult FormDesignTemplate(FormDesignTemplateBE model)
        {

            bool success = false;
            int id = 0;

            FormDesignTemplateBE formDesignTemplate = FormLogic.FetchFormDesignTemplate(model.FormID);
            if (formDesignTemplate == null)
            {
                formDesignTemplate = new FormDesignTemplateBE();
            }
            formDesignTemplate.Design = model.Design != null ? model.Design.ToUpper() : string.Empty;
            formDesignTemplate.Description = model.Description;
            formDesignTemplate.Active = model.Active;

            if (model.FormID <= 0)
            {
                formDesignTemplate.CreatedBy = "Habitat";
                formDesignTemplate.CreatedDateTime = DateTime.Now;
                formDesignTemplate.LastUpdatedDateTime = DateTime.Now;
                success = FormLogic.AddFormDesignTemplate(formDesignTemplate, out id);
            }
            else
            {
                formDesignTemplate.UpdatedBy = "Habitat";
                formDesignTemplate.LastUpdatedDateTime = DateTime.Now;
                success = FormLogic.UpdateFormDesignTemplate(formDesignTemplate);
            }
            return Json(new { success, id });
        }

        public ActionResult FormDesignTemplateDetail(int formID)
        {
            FormDesignTemplateDetailModel model = new FormDesignTemplateDetailModel();

            model.TemplateDetails = new List<FormDesignTemplateDetailBE>();

            FormDesignTemplateBE formDesignTemplate = FormLogic.FetchFormDesignTemplate(formID);
            if (formDesignTemplate != null)
            {
                model.FormID = formDesignTemplate.FormID;
                model.Design = formDesignTemplate.Design;
                model.Description = formDesignTemplate.Description;
                model.Active = formDesignTemplate.Active;
            }
            model.TemplateSectionDetail = FormLogic.FetchAllTemplateFormSection(model.FormID);
            return View(model);
        }


        [HttpPost]
        public ActionResult FormDesignTemplateDetail(FormDesignTemplateDetailModel model)
        {
            bool success = false;
            //success = FormLogic.Save(model.TemplateDetails);
            return new JsonResult(new { Success = success, TemplateDetails = success ? model.TemplateDetails : null });
        }

        public ActionResult TemplateSectionList(int formID)
        {
            FormDesignTemplateDetailModel model = new FormDesignTemplateDetailModel();
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

            if(string.IsNullOrWhiteSpace(model.BackgroundColor))
            {
                model.BackgroundColor = "#ffffff";
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
            templateFormSection.BackgroundColor = model.BackgroundColor;
            templateFormSection.TextColor = model.TextColor;
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
        public ActionResult TemplateSectionList(FormDesignTemplateDetailModel model)
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

        public async Task<IActionResult> PermitFormTemplateFields(int formID, int? surrogate, bool? isRenderForDragnDrop = null)
        {
            FormDesignTemplateModelBE model = new FormDesignTemplateModelBE();
            string htmlForm = string.Empty;
            using (var httpClient = new HttpClient())
            {
                string url = DBConfiguration.WebAPIHostingURL;
                if (!string.IsNullOrWhiteSpace(url))
                {
                    string webAPIURL = string.Format("{0}form/GetFormHtml/{1}/{2}/{3}", url, formID, isRenderForDragnDrop != null ? isRenderForDragnDrop.Value : false, surrogate);
                    using (var response = await httpClient.GetAsync(webAPIURL))
                    {
                        htmlForm = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            model.RenderForDragnDrop = isRenderForDragnDrop != null ? isRenderForDragnDrop.Value : false;
            model.HtmlForm = htmlForm;
            model.FormID = formID;
            string formDialogTitle = "";
            FormDesignTemplateBE formDesignTemplate = FormLogic.FetchFormDesignTemplate(formID);
            if(Functions.IsNotNull(formDesignTemplate))
            {
                formDialogTitle = formDesignTemplate.Description + " - " + formDesignTemplate.Design;
            }
            model.FormDialogTitle = formDialogTitle;
            return View(model);
        }

        public async Task<string> EditCreatedForm(int formID, int surrogate)
        {
            FormDesignTemplateModelBE model = new FormDesignTemplateModelBE();
            string htmlForm = string.Empty;
            using (var httpClient = new HttpClient())
            {
                string url = DBConfiguration.WebAPIHostingURL;
                if (!string.IsNullOrWhiteSpace(url))
                {
                    string webAPIURL = string.Format("{0}form/GetFormHtml/{1}/{2}/{3}", url, formID, false, surrogate);
                    using (var response = await httpClient.GetAsync(webAPIURL))
                    {
                        htmlForm = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return htmlForm;
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
                rows = new List<FormDesignTemplateDetailBE>()
            };

            try
            {
                IEnumerable<FormDesignTemplateDetailBE> list = FormLogic.BlockFetchFormDesignTemplateDetail(formID, page, rows, out int totalRecords);

                if (list == null)
                {
                    return Json(jsonData);
                }
                else
                {
                    var resultFormTemplate = (from obj in list
                                              select new FormDesignTemplateDetailBE
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
            FormDesignTemplateDetailBE model;

            model = FormLogic.FetchFormDesignTemplateDetail(formID, fieldID);
            if (model == null)
            {
                model = new FormDesignTemplateDetailBE();
            }

            ViewData["SectionList"] = FormLogic.FetchAllTemplateFormSection(formID).Select(m => new SelectListItem()
            {
                Text = m.Section,
                Value = m.Section
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPermitFormField(FormDesignTemplateDetailBE model, List<TableFieldTypeMasterBE> tableFieldTypeMaster)
        {

            bool success = false;
            int id = 0;

            FormDesignTemplateDetailBE formDesignTemplateDetail = FormLogic.FetchFormDesignTemplateDetail(model.FormID, model.Field);
            if (formDesignTemplateDetail == null)
            {
                formDesignTemplateDetail = new FormDesignTemplateDetailBE();
            }
            formDesignTemplateDetail.FormID = model.FormID;
            formDesignTemplateDetail.Field = model.Field;
            formDesignTemplateDetail.FieldName = model.FieldName;
            formDesignTemplateDetail.FieldType = model.FieldType;
            formDesignTemplateDetail.Section = model.Section;
            formDesignTemplateDetail.Sequence = model.Sequence;

            if (model.Field <= 0)
            {
                success = FormLogic.AddFormDesignTemplateDetail(formDesignTemplateDetail, out int field);

                //if (tableFieldTypeMaster != null)
                //{
                //    foreach (var o in tableFieldTypeMaster)
                //    {
                //        FormLogic.AddTableFieldTypeMaster(o);
                //    }
                //}
            }
            else
            {
                success = FormLogic.UpdateFormDesignTemplateDetail(formDesignTemplateDetail);

                //if (tableFieldTypeMaster != null)
                //{
                //    foreach (var o in tableFieldTypeMaster)
                //    {
                //        if (o.Field > 0)
                //        {
                //            FormLogic.DeleteTableFieldTypeMaster(o.Field);
                //        }
                //        FormLogic.AddTableFieldTypeMaster(o);
                //    }
                //}
            }
            return Json(new { success, id });
        }

        public JsonResult DeletePermitFormField(int formID, int fieldID)
        {
            bool success = false;

            success = FormLogic.DeleteFormDesignTemplateDetail(formID, fieldID);

            return new JsonResult(new { Success = success });
        }

        public ActionResult GetDigitalSignature(int signatureId)
        {
            string signature = FormLogic.GetDigitalSignature(signatureId);
            return new JsonResult(new { Signature = signature });
        }

        public ActionResult FormDesignTemplateDetailFields(FormDesignTemplateDetailBE formDetail)
        {
            bool success = true;
            if (formDetail != null)
            {
                FormDesignTemplateDetailBE formDesignTemplateDetailBE = FormLogic.FetchFormDesignTemplateDetail(formDetail.FormID, formDetail.Field);
                if (formDesignTemplateDetailBE != null)
                {
                    formDesignTemplateDetailBE.Section = formDetail.Section;
                    formDesignTemplateDetailBE.Sequence = formDetail.Sequence;
                    success = FormLogic.UpdateFormDesignTemplateDetail(formDesignTemplateDetailBE);
                }
            }
            return Json(new { Success = success });
        }
    }


}