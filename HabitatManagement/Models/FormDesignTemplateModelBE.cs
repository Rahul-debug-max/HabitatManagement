using HabitatManagement.BusinessEntities;
using HabitatManagement.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HabitatManagement.Models
{
    public class FormDesignTemplateModelBE
    {
        private List<PermitFormScreenDesignTemplateDetailBE> _fields;

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

        public int TemplateID { get; set; }

        #endregion

        #region Methods

        public string FormSectionFields()
        {
            StringBuilder sb = new StringBuilder();
            var sectionGroupFields = _fields.GroupBy(o => o.Section).OrderBy(s=> s.Key).Select(x=> x).ToList();

            if (sectionGroupFields != null && sectionGroupFields.Count > 0)
            {
                sb.AppendFormat("<div class=\"formOuterStyle fontBold bgLightGray\">");
                sb.Append("NOTE: This Permit is valid for one shift only");
                sb.Append("</div>");

                foreach (var sectionGroupField in sectionGroupFields)
                {
                    sb.Append(HtmlFields(sectionGroupField));
                }
            }
            return sb.ToString();
        }

        #endregion

        #region Private Methods

        private string HtmlFields(IGrouping<PromptFormSectionField, PermitFormScreenDesignTemplateDetailBE> groupingFields)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div class=\"container-fluid formOuterStyle bgLightGray\" data-field = \"{0}\" >", groupingFields.Key);

            if (groupingFields.Key == PromptFormSectionField.SectionA)
            {
                sb.AppendFormat("<label class=\"col-lg-12 col-form-label fontBold\">{0}</label>", "PART A – PREPARATION");
            }
            else if (groupingFields.Key == PromptFormSectionField.SectionB)
            {
                sb.AppendFormat("<label class=\"col-lg-12 col-form-label fontBold\">{0}</label>", "PART B – PRECAUTIONS CHECKLIST");
            }
            else if (groupingFields.Key == PromptFormSectionField.SectionC)
            {
                sb.AppendFormat("<label class=\"col-lg-12 col-form-label fontBold\">{0}</label>", "PART C – AUTHORISATION");
            }
            else if (groupingFields.Key == PromptFormSectionField.SectionD)
            {
                sb.AppendFormat("<label class=\"col-lg-12 col-form-label fontBold\">{0}</label>", "PART D – ACCEPTANCE OF PERMIT");
            }
            else if (groupingFields.Key == PromptFormSectionField.SectionE)
            {
                sb.AppendFormat("<label class=\"col-lg-12 col-form-label fontBold\">{0}</label>", "PART E – COMPLETION OF WORK");
            }
            else if (groupingFields.Key == PromptFormSectionField.SectionF)
            {
                sb.AppendFormat("<label class=\"col-lg-12 col-form-label fontBold\">{0}</label>", "PART F – SUPERCEDED PERMIT");
            }

            foreach (var field in groupingFields)
            {
                switch (field.FieldType)
                {
                    case FormFieldType.Textbox:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        sb.AppendFormat("<div class=\"form-group row\">");
                        sb.AppendFormat("<label class=\"col-lg-3 text-right col-form-label paddlftrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-7\">");
                        sb.AppendFormat("<input type=\"text\" id=\"{1}\" name=\"{1}\" value=\"{0}\" class=\"form-control\" \\>", _templateFormFieldData?.FieldValue, field.Field);
                        sb.Append("</div>");
                        sb.Append("</div>");
                        break;
                    case FormFieldType.TextArea:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        sb.AppendFormat("<div class=\"form-group row\">");
                        sb.AppendFormat("<label class=\"col-lg-3 text-right col-form-label paddlftrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-7\">");
                        sb.AppendFormat("<textarea id=\"{1}\" name=\"{1}\" class=\"form-control textAreaVerticalResizing\" readonly=\"readonly\" rows=\"3\" >{0}</textarea>", _templateFormFieldData?.FieldValue, field.Field);
                        sb.Append("</div>");
                        sb.Append("</div>");
                        break;
                    case FormFieldType.Date:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        DateTime dateTime;
                        bool isSuccess = DateTime.TryParse(_templateFormFieldData?.FieldValue, out dateTime);
                        sb.AppendFormat("<div class=\"form-group row\">");
                        sb.AppendFormat("<label class=\"col-lg-3 text-right col-form-label paddlftrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-7\">");
                        sb.AppendFormat("<div class=\"input-group date col-lg-6 paddlft-none\" id=\"datepicker\">");
                        sb.AppendFormat("<input class=\"form-control\" placeholder=\"MM/DD/YYYY\" value=\"{0}\"/>", dateTime != DateTime.MinValue ? dateTime.ToString("MM/dd/yyyy") : "");
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
                        sb.AppendFormat("<div class=\"form-group row\">");
                        sb.AppendFormat("<label class=\"col-lg-3 text-right col-form-label paddlftrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-4 pr-5\">");
                        sb.AppendFormat("<div class=\"input-group date paddlft-none\" id=\"datetimepicker\">");
                        sb.AppendFormat("<input class=\"form-control\" type=\"text\" placeholder=\"MM/DD/YYYY\" value=\"{0}\"/>", dateAndTime != DateTime.MinValue ? dateAndTime.ToString("MM/dd/yyyy") : "");
                        sb.Append("<span class=\"input-group-append input-group-addon\">");
                        sb.Append("<span class=\"input-group-text\"><i class=\"glyphicons glyphicons-calendar\"></i></span>");
                        sb.Append("</span>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("<div class=\"col-lg-3 ml-lg-n5\">");
                        sb.Append("<div class=\"input-group time\" id=\"timepicker\" style=\"width:134px;\">");
                        sb.AppendFormat("<input class=\"form-control\" placeholder=\"HH: MM\" value=\"{0}\"/>", dateAndTime != DateTime.MinValue ? dateAndTime.ToString("HH:mm") : "");
                        sb.Append("<span class=\"input-group-append input-group-addon\">");
                        sb.Append("<span class=\"input-group-text\"><i class=\"glyphicons glyphicons-clock\"></i></span>");
                        sb.Append("</span>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
 
                        break;
                    case FormFieldType.Signature:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        sb.AppendFormat("<div class=\"form-group row\">");
                        sb.AppendFormat("<label class=\"col-lg-3 text-right col-form-label paddlftrght-none\" for=\"{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-lg-7\">");
                        sb.Append("<div class=\"col-xs-12 col-sm-12 col-md-12 col-lg-12 paddlftrght-none dvSignatureDataType\" >");
                        sb.Append("<div class=\"panel panel-default\" style=\"margin-bottom:0px;\" >");
                        sb.Append("<div class=\"panel-heading\" style=\"text-align:right;background-color: #f5f5f5; padding: 10px 15px;\" >");
                        sb.AppendFormat("<button type=\"button\" class=\"btn btn-primary x1\" onclick=\"editDigitalSignature(this)\" title=\"{0}\" style=\"margin-right:5px;\" \"><span class=\"glyphicons glyphicons-edit\"></span></button>", "Edit");
                        sb.AppendFormat("<button type=\"button\" class=\"btn btn-primary x1\" onclick=\"resetDigitalSignature(this)\" title=\"{0}\" \"><span class=\"glyphicons glyphicons-refresh\"></span></button>", "Reset");
                        sb.Append("</div>");
                        sb.Append("<div class=\"panel-body paddlftrght-none\" style=\"padding-bottom: 0; padding-top: 0;\" >");
                        sb.AppendFormat("<div id=\"digitalcanvasouter_{0}\">", field.Field);
                        sb.AppendFormat("<div class=\"conditionDigitalSignature\" id=\"digitalSignature_{0}\"></div>", field.Field);
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        var digitalSignatureImage64BitString = FormLogic.GetDigitalSignature(Convert.ToInt32(_templateFormFieldData?.FieldValue));
                        sb.AppendFormat("<input type=\"hidden\" id='SignatureResponse' readonly=\"readonly\" class=\"form-control\" value=\"{0}\"  />", digitalSignatureImage64BitString);
                        sb.AppendFormat("<input type=\"hidden\" id='SignatureId' readonly=\"readonly\" class=\"form-control\" value=\"{0}\"  />", _templateFormFieldData?.FieldValue);
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                        break;
                }
            }

            sb.Append("</div>");
            sb.Append("<p></p>");

            return sb.ToString();
        }

        #endregion
    }
}