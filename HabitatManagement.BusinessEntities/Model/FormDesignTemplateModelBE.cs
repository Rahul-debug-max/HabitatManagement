using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HabitatManagement.BusinessEntities
{
    public class FormDesignTemplateModelBE
    {
        public string Form { get; set; }
        public string HtmlForm { get; set; }
        private List<PermitFormScreenDesignTemplateDetailBE> _fields;

        public FormDesignTemplateModelBE()
        {
        }

        public FormDesignTemplateModelBE(List<PermitFormScreenDesignTemplateDetailBE> fields)
        {
            _fields = fields ?? new List<PermitFormScreenDesignTemplateDetailBE>();
        }

        public FormDesignTemplateModelBE(List<PermitFormScreenDesignTemplateDetailBE> fields, bool renderForDragnDrop) : this(fields)
        {
            _renderForDragnDrop = renderForDragnDrop;
        }

        public FormDesignTemplateModelBE(List<PermitFormScreenDesignTemplateDetailBE> fields, TemplateFormFieldDataBE templateFormFieldDataBE)
            : this(fields)
        {
            _templateFormFieldData = templateFormFieldDataBE;
        }

        public FormDesignTemplateModelBE(List<PermitFormScreenDesignTemplateDetailBE> fields, List<TemplateFormFieldDataBE> templateFormFieldDataList)
          : this(fields)
        {
            _templateFormFieldDataList = templateFormFieldDataList;
        }

        #region Properties

        private TemplateFormFieldDataBE _templateFormFieldData = null;
        private List<TemplateFormFieldDataBE> _templateFormFieldDataList = null;

        private bool _renderForDragnDrop = false;

        public bool RenderForDragnDrop
        {
            get { return _renderForDragnDrop; }
            set { _renderForDragnDrop = value; }
        }

        public int FormID { get; set; }

        #endregion

        #region Methods

        public string FormSectionFields()
        {
            StringBuilder sb = new StringBuilder();
            var sectionGroupFields = _fields.OrderBy(m=> m.SectionSequence).ThenBy(m => m.Sequence).GroupBy(o => o.Section).Select(x => x).ToList();

            if (sectionGroupFields != null && sectionGroupFields.Count > 0)
            {
                foreach (var sectionGroupField in sectionGroupFields)
                {
                    sb.Append(HtmlFields(sectionGroupField));
                }
            }
            return sb.ToString();
        }

        #endregion

        #region Private Methods

        private string HtmlFields(IGrouping<string, PermitFormScreenDesignTemplateDetailBE> groupingFields)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div class=\"container-fluid formOuterStyle bgLightGray sortable_list connectedSortable\" data-section = \"{0}\" >", groupingFields.Key);
            var sortableGroupingFields = groupingFields.OrderBy(s => s.Sequence).ToList() ?? new List<PermitFormScreenDesignTemplateDetailBE>();
            sb.AppendFormat("<label class=\"col-lg-12 col-form-label fontBold\">{0}</label>", sortableGroupingFields[0].SectionDescription);
            List<PermitFormScreenDesignTemplateDetailBE> permitFormScreenDesignTemplateDetailsCheckList = sortableGroupingFields.Where(s => s.FieldType == FormFieldType.CheckList).OrderBy(s=>s.Field).ThenBy(s=>s.Sequence).Select(s => s).ToList();
            bool isCheckList = false;

            foreach (var field in sortableGroupingFields)
            {
                switch (field.FieldType)
                {
                    case FormFieldType.Checkbox:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        sb.AppendFormat("<div class=\"form-group row\" data-field = \"{0}\" field-Type = \"{1}\">", field.Field, (int)field.FieldType);
                        sb.AppendFormat("<label class=\"col-lg-4 text-right col-form-label paddrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-8 pr-5 formFieldTypeCheckbox\" style=\"padding-top:7px;\">");
                        if (RenderForDragnDrop)
                        {
                            sb.AppendFormat("<label class=\"align-items-center d-inline-flex\"> Yes <input type=\"checkbox\" class=\"ml-2\" forType=\"yes\" name=\"{0}\" disabled>  </label>", field.Field);
                            sb.AppendFormat("<label class=\"align-items-center d-inline-flex\" style=\"margin-left:22px;\"> No <input type=\"checkbox\" class=\"ml-2\" forType=\"no\" name=\"{0}\" disabled> </label>", field.Field);
                        }
                        else
                        {
                            sb.AppendFormat("<label class=\"align-items-center d-inline-flex\"> Yes <input type=\"checkbox\" class=\"ml-2\" forType=\"yes\" name=\"{0}\" {1}>  </label>", field.Field, !string.IsNullOrWhiteSpace(_templateFormFieldData?.FieldValue) && Functions.IdhammarCharToBool(_templateFormFieldData?.FieldValue) ? "checked" : "");
                            sb.AppendFormat("<label class=\"align-items-center d-inline-flex\" style=\"margin-left:22px;\"> No <input type=\"checkbox\" class=\"ml-2\" forType=\"no\" name=\"{0}\" {1}> </label>", field.Field, !string.IsNullOrWhiteSpace(_templateFormFieldData?.FieldValue) && !Functions.IdhammarCharToBool(_templateFormFieldData?.FieldValue) ? "checked" : "");
                        }
                        sb.Append("</div>");
                        sb.Append("</div>");
                        break;
                    case FormFieldType.Label:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        sb.AppendFormat("<div class=\"form-group row\" style=\"padding-left: 25px;padding-right: 20px;text-align: justify;\" data-field = \"{0}\" field-Type = \"{1}\">", field.Field, (int)field.FieldType);
                        sb.AppendFormat("<label class=\"col-form-label\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("</div>");
                        break;
                    case FormFieldType.Textbox:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        sb.AppendFormat("<div class=\"form-group row\" data-field = \"{0}\" field-Type = \"{1}\">", field.Field, (int)field.FieldType);
                        sb.AppendFormat("<label class=\"col-lg-4 text-right col-form-label paddrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-8 pr-5\">");
                        if (RenderForDragnDrop)
                        {
                            sb.AppendFormat("<input type=\"text\" class=\"form-control\" readonly=\"readonly\" \\>");
                        }
                        else
                        {
                            sb.AppendFormat("<input type=\"text\" id=\"{1}\" name=\"{1}\" value=\"{0}\" class=\"form-control\" \\>", _templateFormFieldData?.FieldValue, field.Field);
                        }
                        sb.Append("</div>");
                        sb.Append("</div>");
                        break;
                    case FormFieldType.TextArea:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        sb.AppendFormat("<div class=\"form-group row\" data-field = \"{0}\" field-Type = \"{1}\">", field.Field, (int)field.FieldType);
                        sb.AppendFormat("<label class=\"col-lg-4 text-right col-form-label paddrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-8 pr-5\">");
                        if (RenderForDragnDrop)
                        {
                            sb.AppendFormat("<textarea class=\"form-control textAreaVerticalResizing\" readonly=\"readonly\" rows=\"3\" ></textarea>");
                        }
                        else
                        {
                            sb.AppendFormat("<textarea id=\"{1}\" name=\"{1}\" class=\"form-control textAreaVerticalResizing\" rows=\"3\" >{0}</textarea>", _templateFormFieldData?.FieldValue, field.Field);
                        }
                        sb.Append("</div>");
                        sb.Append("</div>");
                        break;
                    case FormFieldType.Date:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        DateTime dateTime;
                        bool isSuccess = DateTime.TryParse(_templateFormFieldData?.FieldValue, out dateTime);
                        sb.AppendFormat("<div class=\"form-group row\" data-field = \"{0}\" field-Type = \"{1}\">", field.Field, (int)field.FieldType);
                        sb.AppendFormat("<label class=\"col-lg-4 text-right col-form-label paddrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-8 pr-5\">");
                        sb.AppendFormat("<div class=\"input-group date col-lg-4 pl-0\" name=\"datetimepicker\">");

                        if (RenderForDragnDrop)
                        {
                            sb.AppendFormat("<input class=\"form-control\" readonly=\"readonly\" placeholder=\"DD/MM/YYYY\" />");
                        }
                        else
                        {
                            sb.AppendFormat("<input class=\"form-control\" name=\"{1}\" placeholder=\"DD/MM/YYYY\" value=\"{0}\"/>", dateTime != DateTime.MinValue ? dateTime.ToShortDateString() : "", field.Field);
                        }
                        sb.Append("<span class=\"input-group-append input-group-addon\">");
                        sb.Append("<span class=\"input-group-text\"><i class=\"glyphicons glyphicons-calendar\"></i></span>");
                        sb.Append("</span>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                        break;
                    case FormFieldType.DateAndTime:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        DateTime dateAndTime;
                        bool success = DateTime.TryParse(_templateFormFieldData?.FieldValue, out dateAndTime);
                        sb.AppendFormat("<div class=\"form-group row\" data-field = \"{0}\" field-Type = \"{1}\">", field.Field, (int)field.FieldType);
                        sb.AppendFormat("<label class=\"col-lg-4 text-right col-form-label paddrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-3 pr-5\">");
                        sb.AppendFormat("<div class=\"input-group date pl-0\" name=\"datetimepicker\">");
                        if (RenderForDragnDrop)
                        {
                            sb.AppendFormat("<input class=\"form-control\" type=\"text\" placeholder=\"DD/MM/YYYY\" readonly=\"readonly\" />");
                        }
                        else
                        {
                            sb.AppendFormat("<input class=\"form-control\" name=\"{1}\" type=\"text\" placeholder=\"DD/MM/YYYY\" value=\"{0}\"/>", dateAndTime != DateTime.MinValue ? dateAndTime.ToShortDateString() : "", field.Field);
                        }
                        sb.Append("<span class=\"input-group-append input-group-addon\">");
                        sb.Append("<span class=\"input-group-text\"><i class=\"glyphicons glyphicons-calendar\"></i></span>");
                        sb.Append("</span>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("<div class=\"col-lg-3 ml-lg-n5\">");
                        sb.Append("<div class=\"input-group time\" name=\"timepicker\" style=\"width:134px;\">");
                        if (RenderForDragnDrop)
                        {
                            sb.AppendFormat("<input class=\"form-control\" placeholder=\"HH: MM\" readonly=\"readonly\" />");
                        }
                        else
                        {
                            sb.AppendFormat("<input class=\"form-control\" placeholder=\"HH: MM\" value=\"{0}\"/>", dateAndTime != DateTime.MinValue ? dateAndTime.ToString("HH:mm") : "");
                        }
                        sb.Append("<span class=\"input-group-append input-group-addon\">");
                        sb.Append("<span class=\"input-group-text\"><i class=\"glyphicons glyphicons-clock\"></i></span>");
                        sb.Append("</span>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                        break;
                    case FormFieldType.Signature:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        sb.AppendFormat("<div class=\"form-group row\" data-field = \"{0}\" field-Type = \"{1}\">", field.Field, (int)field.FieldType);
                        sb.AppendFormat("<label class=\"col-lg-4 text-right col-form-label paddrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-8 pr-5\">");
                        sb.Append("<div class=\"col-xs-12 col-sm-12 col-md-12 col-lg-12 pl-0 pr-0 dvSignatureDataType\" >");
                        sb.Append("<div class=\"panel panel-default\" style=\"margin-bottom:0px;border: 1px solid;\" >");
                        sb.Append("<div class=\"panel-heading\" style=\"text-align:right;background-color: #f5f5f5; padding: 10px 15px;\" >");

                        if (RenderForDragnDrop)
                        {
                            sb.AppendFormat("<button type=\"button\" class=\"btn btn-primary x1\" disabled title=\"{0}\" style=\"margin-right:5px;\" \"><span class=\"glyphicons glyphicons-edit\"></span></button>", "Edit");
                            sb.AppendFormat("<button type=\"button\" class=\"btn btn-primary x1\" disabled title=\"{0}\" \"><span class=\"glyphicons glyphicons-refresh\"></span></button>", "Reset");
                        }
                        else
                        {
                            sb.AppendFormat("<button type=\"button\" class=\"btn btn-primary x1\" onclick=\"editDigitalSignature(this)\" title=\"{0}\" style=\"margin-right:5px;\" \"><span class=\"glyphicons glyphicons-edit\"></span></button>", "Edit");
                            sb.AppendFormat("<button type=\"button\" class=\"btn btn-primary x1\" onclick=\"resetDigitalSignature(this)\" title=\"{0}\" \"><span class=\"glyphicons glyphicons-refresh\"></span></button>", "Reset");
                        }
                        sb.Append("</div>");
                        sb.Append("<div class=\"panel-body pl-0 pr-0\" style=\"padding-bottom: 0; padding-top: 0;\" >");
                        sb.AppendFormat("<div id=\"digitalcanvasouter_{0}\">", field.Field);
                        sb.AppendFormat("<div class=\"conditionDigitalSignature\" id=\"digitalSignature_{0}\"></div>", field.Field);
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        if (!RenderForDragnDrop)
                        {
                            var digitalSignatureImage64BitString = FormLogic.GetDigitalSignature(Convert.ToInt32(_templateFormFieldData?.FieldValue));
                            sb.AppendFormat("<input type=\"hidden\" id='SignatureResponse' readonly=\"readonly\" class=\"form-control\" value=\"{0}\"  />", digitalSignatureImage64BitString);
                            sb.AppendFormat("<input type=\"hidden\" id='SignatureId' readonly=\"readonly\" class=\"form-control\" value=\"{0}\"  />", _templateFormFieldData?.FieldValue);
                        }
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                        break;
                    case FormFieldType.CheckList:

                        if (!isCheckList)
                        {
                            if (permitFormScreenDesignTemplateDetailsCheckList != null && permitFormScreenDesignTemplateDetailsCheckList.Count > 0)
                            {
                                isCheckList = true;
                                sb.AppendFormat("<div class=\"dvCheckList form-group\" style=\"margin-bottom: 10px;padding-top: 5px;\" data-field = \"{0}\" field-Type = \"{1}\">", permitFormScreenDesignTemplateDetailsCheckList[0].Field, (int)FormFieldType.CheckList);
                                sb.Append("<table class=\"tableCheckList\"><tr><th></th><th style=\"width: 50px;text-align:center;\"> Yes </th><th style=\"width: 50px;text-align:center;\"> No </th></tr>");

                                foreach (var permitFormScreenDesignTemplateDetail in permitFormScreenDesignTemplateDetailsCheckList)
                                {
                                    if (RenderForDragnDrop)
                                    {
                                        sb.AppendFormat("<tr> <td> {0} </td>", permitFormScreenDesignTemplateDetail.FieldName);
                                        sb.AppendFormat("<td class=\"bgColorWhite\" style=\"text-align:center;\"> <input type=\"checkbox\" name=\"checkListEntityType\" disabled></td>");
                                        sb.AppendFormat("<td class=\"bgColorWhite\" style=\"text-align:center;\"> <input type=\"checkbox\" name=\"checkListEntityType\" disabled></td>");
                                        sb.Append("</tr>");
                                    }
                                    else
                                    {
                                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == permitFormScreenDesignTemplateDetail.Field).FirstOrDefault();
                                        sb.AppendFormat("<tr class=\"checkListTR\"> <td data-field = \"{1}\"> {0} </td>", permitFormScreenDesignTemplateDetail.FieldName, permitFormScreenDesignTemplateDetail.Field);
                                        sb.AppendFormat("<td class=\"bgColorWhite\" style=\"text-align:center;\"> <input type=\"checkbox\" forType=\"yes\" name=\"{1}\" {0}></td>", !string.IsNullOrWhiteSpace(_templateFormFieldData?.FieldValue) && Functions.IdhammarCharToBool(_templateFormFieldData?.FieldValue) ? "checked" : "", permitFormScreenDesignTemplateDetail.Field);
                                        sb.AppendFormat("<td class=\"bgColorWhite\" style=\"text-align:center;\"> <input type=\"checkbox\" forType=\"no\" name=\"{1}\" {0}></td>", !string.IsNullOrWhiteSpace(_templateFormFieldData?.FieldValue) && !Functions.IdhammarCharToBool(_templateFormFieldData?.FieldValue) ? "checked" : "", permitFormScreenDesignTemplateDetail.Field);
                                        sb.Append("</tr>");
                                    }
                                }

                                sb.Append("</table>");
                                sb.Append("</div>");
                            }
                        }
                        break;

                }
            }

            sb.Append("</div>");
            return sb.ToString();
        }

        #endregion
    }
}