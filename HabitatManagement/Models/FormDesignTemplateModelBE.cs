using HabitatManagement.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

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
            // SetValues();
        }

        public FormDesignTemplateModelBE(List<PermitFormScreenDesignTemplateDetailBE> fields, TemplateFormFieldDataBE templateFormFieldDataBE)
            : this(fields)
        {
            _templateFormFieldData = templateFormFieldDataBE;
            // SetValues();
        }


        #region Properties

        private TemplateFormFieldDataBE _templateFormFieldData = null;

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
            var leftFields = _fields.OrderBy(o => o.Sequence).ToList();

            if (leftFields != null)
            {
                foreach (var field in leftFields)
                {
                    sb.Append(HtmlFields(field));
                }
            }
            return sb.ToString();
        }

        #endregion

        #region Private Methods

        private string HtmlFields(PermitFormScreenDesignTemplateDetailBE field)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div class=\"form-group\" data-field = \"{0}\" >", field.Field);

            switch (field.FieldType)
            {
                case FormFieldType.Textbox:
                    sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" for=\"formDesignField\">{0}</label>", field.FieldName);
                    sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");
                    sb.AppendFormat("<input type=\"text\" id=\"{1}\" name=\"{1}\" value=\"{0}\" class=\"form-control\" \\>", _templateFormFieldData?.FieldValue, field.Field);
                    sb.Append("</div>");
                    break;

                    //case FormFieldType.Label:
                    //    sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\">{0}</label>", field.FieldName);
                    //    sb.AppendFormat("<div class=\"col-sm-6 col-md-3 col-lg-3 paddrght-none\"><div class=\"form-control\" readonly>{0}</div></div>", _templateFormFieldData?.FieldValue);
                    //    break;
                    // case TaskFeedbackTemplateFormField.Comments:
                    //case FormFieldType.Checkbox:
                    // sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" for=\"ConditionComment\">{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     //if (RenderForDragnDrop)
                    //     //{
                    //     //    sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     //}
                    //     //else
                    //     //{
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");

                    //         sb.AppendFormat("<textarea name=\"ConditionDataType\" class=\"form-control textAreaVerticalResizing\" readonly=\"readonly\" rows=\"3\" >{0}</textarea>", Functions.TrimRight(field.GetCustomDataValue<string>("Comments")));

                    //         sb.Append("</div>");
                    //     //}
                    //     break;
                    // case TaskFeedbackTemplateFormField.CompletedDate:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" for=\"ConditionDateRecorded_DatePart_{1}\">{0}</label>", field.Field.GetLocalizedDisplayName(), _taskCondition?.ConditionSurrogate);
                    // if (RenderForDragnDrop)
                    // {
                    //     sb.Append("<div class=\"col-sm-6 col-md-4 col-lg-4\"><div class=\"form-control\" readonly></div></div>");
                    //     sb.Append("<div class=\"col-sm-3 col-md-2 col-lg-2 paddlftrght-none\"><div class=\"form-control\" readonly></div></div>");
                    // }
                    // else
                    // {
                    //     sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none conditiondate\">");

                    //     DateTime completedDateTime = field.GetCustomDataValue<DateTime>("CompletedDate");

                    //     sb.Append("<div style=\"width: 196px!important; display:inline-block; \" >");

                    //     sb.Append("<div class=\"input-group \" >");

                    //     sb.AppendFormat("<input  class=\"dateTextBox form-control\"  type=\"text\" name=\"ConditionDateRecorded_DatePart_{1}\" id=\"ConditionDateRecorded_DatePart_{1}\" value=\"{0}\"/>", completedDateTime != DateTime.MinValue ? ViewHelper.GetDateString(completedDateTime) : "", _taskCondition?.ConditionSurrogate);

                    //     sb.Append("<span class=\"input-group-addon input-group-calendar-span\">");

                    //     sb.AppendFormat("<span id=\"imgClearDesignTemplateFaultCompletedDateTime_{0}\" class=\"ui-datepicker-trigger glyphicons glyphicons-remove\" style=\"cursor: pointer !important;\" />", _taskCondition?.ConditionSurrogate);

                    //     sb.Append("</span>");

                    //     sb.Append("</div>");

                    //     sb.Append("</div>");

                    //     sb.Append("<div style=\"width: 117px!important; display:inline-block;padding-left: 10px; \" >");
                    //     sb.Append("<div class=\"timectrlwidth input-group \" >");

                    //     sb.AppendFormat("<input  class=\"timeTextBox form-control\" type=\"text\" name=\"ConditionDateRecorded_TimePart_{1}\" id=\"ConditionDateRecorded_TimePart_{1}\" value=\"{0}\" />", completedDateTime != DateTime.MinValue ? ViewHelper.GetTimeString(completedDateTime) : "", _taskCondition?.ConditionSurrogate);

                    //     sb.Append("</div>");
                    //     sb.Append("</div>");

                    //     sb.Append("</div>");
                    // }
                    //     break;
                    // case TaskFeedbackTemplateFormField.RequiredBy:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" for=\"ConditionComment\">{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         DateTime requiredByDateTime = field.GetCustomDataValue<DateTime>("RequiredByDateTime");

                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");
                    //         sb.Append("<div class=\"col-xs-8 col-sm-9 col-md-8 col-lg-8 paddlftrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", ViewHelper.GetDateString(requiredByDateTime));
                    //         sb.Append("</div>");
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-4 col-lg-4 paddrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\"  value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", ViewHelper.GetTimeString(requiredByDateTime));
                    //         sb.Append("</div>");

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.UnitOfMeasure:

                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" for=\"UOMDescription\">{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");
                    //         sb.Append("<div class=\"col-xs-8 col-sm-9 col-md-8 col-lg-8 paddlftrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\" name=\"UOMDescription\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("UOMDescription")));
                    //         sb.Append("</div>");
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-4 col-lg-4 paddrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\" name=\"UnitOfMeasure\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("UnitOfMeasure")));
                    //         sb.Append("</div>");
                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.ExpectedValue:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" for=\"ConditionExpectedValue\">{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-4 col-lg-4 paddlftrght-none\">");

                    //         sb.AppendFormat("<input type=\"text\" name=\"ConditionExpectedValue\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", field.GetCustomDataValue<double>("ExpectedValue"));

                    //         sb.Append("</div>");
                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.Feedback:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" for=\"ConditionComment\">{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");
                    //         sb.AppendFormat("<textarea name=\"ConditionResponse_{2}\" class=\"form-control textAreaVerticalResizing\" {1} rows=\"3\" >{0}</textarea>", Functions.TrimRight(field.GetCustomDataValue<string>("Feedback")), canEdit ? "" : "readonly=\"readonly\" ", _taskCondition?.ConditionSurrogate);

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.MinMaxValue:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" for=\"ConditionMaximumValue\">{0}</label>", Functions.GetLabel("lblMaxValue"));
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-3 col-lg-3 paddrght-none\">");
                    //         sb.Append("<div class=\"form-control\" readonly></div>");
                    //         sb.Append("</div>");

                    //         sb.AppendFormat("<label class=\"col-xs-12 col-sm-12 col-md-2 col-lg-2 control-label paddrght-none text-right smtxtlft\" for=\"ConditionMinimumValue\">{0}</label>", Functions.GetLabel("lblMinValue"));
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-3 col-lg-3 paddrght-none\">");
                    //         sb.Append("<div class=\"form-control\" readonly></div>");
                    //         sb.Append("</div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-3 col-lg-3 paddrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\"  value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", field.GetCustomDataValue<double>("MaxValue"), _taskCondition?.ConditionSurrogate);
                    //         sb.Append("</div>");

                    //         sb.AppendFormat("<label class=\"col-xs-12 col-sm-12 col-md-2 col-lg-2 control-label paddrght-none text-right smtxtlft\" >{0}</label>", Functions.GetLabel("lblMinValue"));
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-3 col-lg-3 paddrght-none\">");

                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", field.GetCustomDataValue<double>("MinValue"), _taskCondition?.ConditionSurrogate);

                    //         sb.AppendFormat("<input type=\"hidden\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<bool>("ConditionMinimumZero")), _taskCondition?.ConditionSurrogate);
                    //         sb.AppendFormat("<input type=\"hidden\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<bool>("ConditionMaximumZero")), _taskCondition?.ConditionSurrogate);

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.Function:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");

                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("Function")));

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.MachinePart:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");

                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("MachinePart")));

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.Priority:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");
                    //         sb.Append("<div class=\"col-xs-8 col-sm-9 col-md-8 col-lg-8 paddlftrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("PriorityDescription")));
                    //         sb.Append("</div>");
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-4 col-lg-4 paddrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\"  value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("Priority")));
                    //         sb.Append("</div>");

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.PlanningConstraint:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");
                    //         sb.Append("<div class=\"col-xs-8 col-sm-9 col-md-8 col-lg-8 paddlftrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("PlanningConstraintDescription")));
                    //         sb.Append("</div>");
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-4 col-lg-4 paddrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\"  value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("PlanningConstraint")));
                    //         sb.Append("</div>");

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.Constraint:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");
                    //         sb.Append("<div class=\"col-xs-8 col-sm-9 col-md-8 col-lg-8 paddlftrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("ConstraintDescription")));
                    //         sb.Append("</div>");
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-4 col-lg-4 paddrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\"  value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("Constraint")));
                    //         sb.Append("</div>");

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.Instrument:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");

                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("Instrument")));

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.StandardTolerance:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");

                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("StandardTolerance")));

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.ToolsMaterialMethods:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");

                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("ToolsMaterialMethods")));

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.RecommendedAction:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");

                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("RecommendedAction")));

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.ActivityType:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");
                    //         sb.Append("<div class=\"col-xs-8 col-sm-9 col-md-8 col-lg-8 paddlftrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("ActivityTypeDescription")));
                    //         sb.Append("</div>");
                    //         sb.Append("<div class=\"col-xs-4 col-sm-3 col-md-4 col-lg-4 paddrght-none\">");
                    //         sb.AppendFormat("<input type=\"text\"  value=\"{0}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<string>("ActivityType")));
                    //         sb.Append("</div>");

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.Duration:
                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //     if (RenderForDragnDrop)
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //     }
                    //     else
                    //     {
                    //         sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\">");

                    //         sb.Append("<div class=\"col-xs-2 col-sm-1 col-md-1 col-lg-1  paddlftrght-none\" style=\"width: 70px; \">");

                    //         sb.AppendFormat("<input type=\"text\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" maxlength='4' \\ />", field.GetCustomDataValue<double>("Duration"));

                    //         sb.Append("</div>");

                    //         sb.Append("</div>");
                    //     }
                    //     break;
                    // case TaskFeedbackTemplateFormField.Equipment:
                    //     var equipments = field.GetCustomData("Equipments");

                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\">{0}</label>", field.Field.GetLocalizedDisplayName());

                    //     sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" style=\"height: 120px; overflow: auto;\">");

                    //     if (equipments != null)
                    //     {
                    //         var equipmentList = (List<EquipmentBE>)(equipments);

                    //         foreach (var equipment in equipmentList)
                    //         {

                    //             sb.AppendFormat("<div class=\"{0}\">", RenderForDragnDrop ? "" : "divicon");
                    //             sb.Append("<div class=\"form-group\">");
                    //             sb.AppendFormat("<a href=\"javascript:void(0);\" title=\"{0}\" >", equipment.Comments);
                    //             if (string.IsNullOrEmpty(equipment.Image64BitString))
                    //             {
                    //                 sb.Append("<div style=\"text-align: center; font-size: 68px; height: 68px; color: #428bca;\">");
                    //                 sb.Append("<span class=\"glyphicons glyphicons-alert\" style=\"vertical-align: top; line-height: 78px; \"></span>");
                    //                 sb.Append("</div>");
                    //             }
                    //             else
                    //             {
                    //                 sb.Append("<div style=\"text-align: center; height: 68px;\">");
                    //                 sb.AppendFormat("<img style=\"height: 68px; width: 68px; \" src=\"{0}\" />", equipment.Image64BitString);
                    //                 sb.Append("</div>");
                    //             }
                    //             sb.AppendFormat("<div class=\"txt truncate\" style=\"color: #222222;\">{0}</div>", equipment.EquipmentName);
                    //             sb.Append("</a>");
                    //             sb.Append("</div>");
                    //             sb.Append("<div class=\"clearfix\"></div>");
                    //             sb.Append("</div>");
                    //         }
                    //     }
                    //     sb.Append("</div></div>");
                    //     break;
                    // case TaskFeedbackTemplateFormField.ConditionImage:
                    //     var images = field.GetCustomData("ConditionImage");

                    //     sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());

                    //     sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" style=\"height: 88px; overflow: auto;\">");

                    //     if (images != null)
                    //     {
                    //         var imageList = (List<TaskConditionImageBE>)(images);

                    //         foreach (var image in imageList)
                    //         {
                    //             sb.AppendFormat("<div class=\"{0}\">", RenderForDragnDrop ? "" : "divicon");
                    //             sb.Append("<div class=\"form-group\">");
                    //             sb.Append("<a href=\"javascript:void(0);\" >");

                    //             if (string.IsNullOrEmpty(image.Image64BitString))
                    //             {
                    //                 sb.Append("<div style=\"text-align: center; font-size: 68px; height: 68px; color: #428bca;\">");
                    //                 sb.Append("<span class=\"glyphicons glyphicons-alert\" style=\"vertical-align: top; line-height: 78px; \"></span>");
                    //                 sb.Append("</div>");
                    //             }
                    //             else
                    //             {
                    //                 sb.Append("<div style=\"text-align: center; height: 68px;\">");
                    //                 sb.AppendFormat("<img style=\"height: 68px; width: 68px; \" src=\"{0}\" />", image.Image64BitString);
                    //                 sb.Append("</div>");
                    //             }
                    //             sb.Append("</a>");
                    //             sb.Append("</div>");
                    //             sb.Append("<div class=\"clearfix\"></div>");
                    //             sb.Append("</div>");
                    //         }
                    //     }

                    //     sb.Append("</div></div>");
                    //     break;
                    //case TaskFeedbackTemplateFormField.Response:
                    //    sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" >{0}</label>", field.Field.GetLocalizedDisplayName());
                    //    if (RenderForDragnDrop)
                    //    {
                    //        sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\"><div class=\"form-control\" readonly></div></div>");
                    //    }
                    //    else
                    //    {
                    //        sb.AppendFormat("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none\" id=\"dvDataType\" >");

                    //        if (_taskCondition != null)
                    //        {
                    //            switch (_taskCondition.ConditionDataType)
                    //            {
                    //                case ConditionDataTypeField.Character:
                    //                    sb.AppendFormat("<input type=\"text\" name=\"CharacterValue_{1}\" {2} class=\"form-control\" value=\"{0}\"   maxlength=\"256\" />", Functions.TrimRight(field.GetCustomDataValue<string>("Response")), _taskCondition?.ConditionSurrogate, canEdit ? "" : "readonly=\"readonly\" ");
                    //                    break;
                    //                case ConditionDataTypeField.Numeric:
                    //                    sb.AppendFormat("<input type=\"text\" name=\"NumericValue_{1}\" {2} class=\"form-control\" value=\"{0}\"  />", field.GetCustomData("Response") != null ? field.GetCustomDataValue<decimal>("Response").ToString() : "", _taskCondition?.ConditionSurrogate, canEdit ? "" : "readonly=\"readonly\" ");

                    //                    sb.AppendFormat("<input type=\"hidden\" value=\"{0}\" id=\"ConditionMinimumValue_{1}\" class=\"form-control\" readonly=\"readonly\" \\>", field.GetCustomData("MinValue"), _taskCondition?.ConditionSurrogate);

                    //                    sb.AppendFormat("<input type=\"hidden\" value=\"{0}\" id=\"ConditionMaximumValue_{1}\" class=\"form-control\" readonly=\"readonly\" \\>", field.GetCustomData("MaxValue"), _taskCondition?.ConditionSurrogate);

                    //                    sb.AppendFormat("<input type=\"hidden\" value=\"{0}\" id=\"ConditionMinimumZero_{1}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<bool>("ConditionMinimumZero")), _taskCondition?.ConditionSurrogate);

                    //                    sb.AppendFormat("<input type=\"hidden\" value=\"{0}\" id=\"ConditionMaximumZero_{1}\" class=\"form-control\" readonly=\"readonly\" \\>", Functions.TrimRight(field.GetCustomDataValue<bool>("ConditionMaximumZero")), _taskCondition?.ConditionSurrogate);

                    //                    break;
                    //                case ConditionDataTypeField.Boolen:
                    //                    if (canEdit)
                    //                    {
                    //                        sb.Append("<div class=\"input-group\" >");
                    //                        sb.AppendFormat("<input type=\"text\" name=\"BoolenValueDescription_{1}\" id=\"BoolenValueDescription_{1}\" class=\"form-control\" value=\"{0}\"  />", Functions.TrimRight(field.GetCustomDataValue<string>("ResponseDescription")), _taskCondition?.ConditionSurrogate);
                    //                        sb.Append("<div class=\"input-group-addon\" >");
                    //                        sb.AppendFormat("<span class=\"glyphicon glyphicon-chevron-down\" style=\"cursor: pointer\" onclick=\"openDropDown('BoolenValueDescription_{0}');\" ></span>", _taskCondition?.ConditionSurrogate);
                    //                        sb.Append("</div>");
                    //                        sb.Append("</div>");
                    //                    }
                    //                    else
                    //                    {
                    //                        sb.AppendFormat("<input type=\"text\" name=\"BoolenValueDescription_{0}\" readonly=\"readonly\" class=\"form-control\" value=\"{1}\"  />", _taskCondition?.ConditionSurrogate, Functions.TrimRight(field.GetCustomDataValue<string>("ResponseDescription")));
                    //                    }

                    //                    sb.AppendFormat("<input type=\"hidden\" name=\"BoolenValue_{0}\" id=\"BoolenValue_{0}\" readonly=\"readonly\" class=\"form-control\" value=\"{1}\"  />", _taskCondition?.ConditionSurrogate, field.GetCustomData("Response").ToString());

                    //                    break;
                    //                case ConditionDataTypeField.Status:
                    //                    if (canEdit)
                    //                    {
                    //                        sb.Append("<div class=\"input-group\" id=\"dvDataTypeStatus\" >");
                    //                        sb.AppendFormat("<input type=\"text\" name=\"StatusValue_{1}\" id=\"StatusValue_{1}\" class=\"form-control\" value=\"{0}\"  />", Functions.TrimRight(field.GetCustomDataValue<string>("Response")), _taskCondition?.ConditionSurrogate);
                    //                        sb.Append("<div class=\"input-group-addon\" >");
                    //                        sb.AppendFormat("<span class=\"glyphicon glyphicon-chevron-down\" style=\"cursor: pointer\" onclick=\"openDropDown('StatusValue_{0}');\" ></span>", _taskCondition?.ConditionSurrogate);
                    //                        sb.AppendFormat("<input type=\"hidden\" name=\"WarningStatus_{0}\" id=\"WarningStatus_{0}\" readonly=\"readonly\" class=\"form-control\" />", _taskCondition?.ConditionSurrogate);
                    //                        sb.Append("</div>");
                    //                        sb.Append("</div>");
                    //                    }
                    //                    else
                    //                    {
                    //                        sb.AppendFormat("<input type=\"text\" name=\"StatusValue_{0}\" readonly=\"readonly\" class=\"form-control\" value=\"{1}\"  />", _taskCondition?.ConditionSurrogate, Functions.TrimRight(field.GetCustomDataValue<string>("Response")));
                    //                    }

                    //                    break;
                    //                case ConditionDataTypeField.Date:
                    //                    var date = field.GetCustomDataValue<DateTime>("Response");
                    //                    if (canEdit)
                    //                    {
                    //                        sb.Append("<div style=\"width: 196px!important; display:inline-block; \" >");

                    //                        sb.Append("<div class=\"input-group \" >");

                    //                        sb.AppendFormat("<input  class=\"dateTextBox form-control\"  type=\"text\" name=\"Date_DatePart_{1}\" value=\"{0}\"/>", ViewHelper.GetDateString(date), _taskCondition?.ConditionSurrogate);

                    //                        sb.Append("<span class=\"input-group-addon input-group-calendar-span\">");

                    //                        sb.AppendFormat("<span id=\"imgClearDate_{0}\" class=\"ui-datepicker-trigger glyphicons glyphicons-remove\" style=\"cursor: pointer !important;\" />", _taskCondition?.ConditionSurrogate);

                    //                        sb.Append("</span>");

                    //                        sb.Append("</div>");

                    //                        sb.Append("</div>");

                    //                    }
                    //                    else
                    //                    {
                    //                        sb.Append("<div style=\"width: 196px!important; display:inline-block; \" >");

                    //                        sb.AppendFormat("<input type=\"text\" name=\"Date_{1}\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" />", ViewHelper.GetDateString(date), _taskCondition?.ConditionSurrogate);

                    //                        sb.Append("</div>");

                    //                    }
                    //                    break;
                    //                case ConditionDataTypeField.DateTime:
                    //                    var dateTime = field.GetCustomDataValue<DateTime>("Response");
                    //                    if (canEdit)
                    //                    {
                    //                        sb.Append("<div style=\"width: 196px!important; display:inline-block; \" >");

                    //                        sb.Append("<div class=\"input-group \" >");

                    //                        sb.AppendFormat("<input  class=\"dateTextBox form-control \"  type=\"text\" name=\"DateTime_DatePart_{1}\" value=\"{0}\"/>", ViewHelper.GetDateString(dateTime), _taskCondition?.ConditionSurrogate);

                    //                        sb.Append("<span class=\"input-group-addon input-group-calendar-span\">");

                    //                        sb.AppendFormat("<span id=\"imgClearDesignDateTime_{0}\" class=\"ui-datepicker-trigger glyphicons glyphicons-remove\" style=\"cursor: pointer !important;\" />", _taskCondition?.ConditionSurrogate);

                    //                        sb.Append("</span>");

                    //                        sb.Append("</div>");

                    //                        sb.Append("</div>");

                    //                        sb.Append("<div style=\"width: 117px!important; display:inline-block;padding-left: 10px; \" >");
                    //                        sb.Append("<div class=\"timectrlwidth input-group \" >");

                    //                        sb.AppendFormat("<input  class=\"timeTextBox form-control \" type=\"text\" name=\"DateTime_TimePart_{1}\" value=\"{0}\" />", ViewHelper.GetTimeString(dateTime), _taskCondition?.ConditionSurrogate);

                    //                        sb.Append("</div>");
                    //                        sb.Append("</div>");
                    //                    }
                    //                    else
                    //                    {
                    //                        sb.Append("<div style=\"width: 196px!important; display:inline-block; \" >");

                    //                        sb.AppendFormat("<input type=\"text\" name=\"DateTime_DatePart_{1}\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" />", ViewHelper.GetDateString(dateTime), _taskCondition?.ConditionSurrogate);

                    //                        sb.Append("</div>");

                    //                        sb.Append("<div style=\"width: 117px!important; display:inline-block;padding-left: 10px; \" >");
                    //                        sb.AppendFormat("<input type=\"text\" name=\"DateTime_TimePart_{1}\" value=\"{0}\" class=\"form-control\" readonly=\"readonly\" />", ViewHelper.GetTimeString(dateTime), _taskCondition?.ConditionSurrogate);

                    //                        sb.Append("</div>");
                    //                    }
                    //                    break;
                    //                case ConditionDataTypeField.Signature:
                    //                    sb.Append("<div class=\"col-xs-12 col-sm-12 col-md-12 col-lg-12 paddlftrght-none dvSignatureDataType\" >");
                    //                    sb.Append("<div class=\"panel panel-default\" style=\"margin-bottom:0px;\" >");
                    //                    sb.Append("<div class=\"panel-heading\" style=\"text-align:right;background-color: #f5f5f5; padding: 10px 15px;\" >");
                    //                    if (canEdit)
                    //                    {

                    //                        sb.AppendFormat("<button type=\"button\" class=\"btn btn-primary x1\" onclick=\"editDigitalSignature(this)\" title=\"{0}\" style=\"margin-right:5px;\" \"><span class=\"glyphicons glyphicons-edit\"></span></button>", Functions.GetLabel("lblEdit"));
                    //                        sb.AppendFormat("<button type=\"button\" class=\"btn btn-primary x1\" onclick=\"resetDigitalSignature(this)\" title=\"{0}\" \"><span class=\"glyphicons glyphicons-refresh\"></span></button>", Functions.GetLabel("lblReset"));
                    //                    }
                    //                    sb.Append("</div>");
                    //                    sb.Append("<div class=\"panel-body paddlftrght-none\" style=\"padding-bottom: 0; padding-top: 0;\" >");
                    //                    sb.AppendFormat("<div id=\"digitalcanvasouter_{0}\">", _taskCondition?.ConditionSurrogate);
                    //                    sb.AppendFormat("<div class=\"conditionDigitalSignature\" id=\"digitalSignature_{0}\"></div>", _taskCondition?.ConditionSurrogate);
                    //                    sb.Append("</div>");
                    //                    sb.Append("</div>");

                    //                    sb.Append("</div>");

                    //                    sb.AppendFormat("<input type=\"hidden\" id='SignatureResponse' readonly=\"readonly\" class=\"form-control\" value=\"{0}\"  />", Functions.TrimRight(field.GetCustomData("Response")));

                    //                    sb.AppendFormat("<input type=\"hidden\" id='SignatureId' readonly=\"readonly\" class=\"form-control\" value=\"{0}\"  />", Functions.TrimRight(field.GetCustomData("SignatureID")));

                    //                    sb.Append("</div>");
                    //                    break;
                    //                case ConditionDataTypeField.PassFail:
                    //                    if (canEdit)
                    //                    {
                    //                        sb.Append("<div class=\"input-group\" >");
                    //                        sb.AppendFormat("<input type=\"text\" id=\"PassFailDescription_{1}\" name=\"PassFailDescription_{1}\" class=\"form-control\" value=\"{0}\"  />", Functions.TrimRight(field.GetCustomDataValue<string>("ResponseDescription")), _taskCondition?.ConditionSurrogate);
                    //                        sb.Append("<div class=\"input-group-addon\" >");
                    //                        sb.AppendFormat("<span class=\"glyphicon glyphicon-chevron-down\" style=\"cursor: pointer\" onclick=\"openDropDown('PassFailDescription_{0}');\" ></span>", _taskCondition?.ConditionSurrogate);
                    //                        sb.Append("</div>");
                    //                        sb.Append("</div>");
                    //                    }
                    //                    else
                    //                    {
                    //                        sb.AppendFormat("<input type=\"text\" name=\"PassFailDescription_{0}\"  id=\"PassFailDescription_{0}\" readonly=\"readonly\" class=\"form-control\" value=\"{1}\"  />", _taskCondition?.ConditionSurrogate, Functions.TrimRight(field.GetCustomDataValue<string>("ResponseDescription")));
                    //                    }

                    //                    sb.AppendFormat("<input type=\"hidden\" name=\"PassFail_{0}\" id=\"PassFail_{0}\" readonly=\"readonly\" class=\"form-control\" value=\"{1}\"  />", _taskCondition?.ConditionSurrogate, field.GetCustomData("Response").ToString());
                    //                    break;
                    //                case ConditionDataTypeField.OKNOK:
                    //                    if (canEdit)
                    //                    {
                    //                        sb.Append("<div class=\"input-group\" >");
                    //                        sb.AppendFormat("<input type=\"text\" name=\"OKNOKDescription_{1}\"   id=\"OKNOKDescription_{1}\"class=\"form-control\" value=\"{0}\"  />", Functions.TrimRight(field.GetCustomDataValue<string>("ResponseDescription")), _taskCondition?.ConditionSurrogate);
                    //                        sb.Append("<div class=\"input-group-addon\" >");
                    //                        sb.AppendFormat("<span class=\"glyphicon glyphicon-chevron-down\" style=\"cursor: pointer\" onclick=\"openDropDown('OKNOKDescription_{0}');\" ></span>", _taskCondition?.ConditionSurrogate);
                    //                        sb.Append("</div>");
                    //                        sb.Append("</div>");
                    //                    }
                    //                    else
                    //                    {
                    //                        sb.AppendFormat("<input type=\"text\" name=\"OKNOKDescription_{0}\" readonly=\"readonly\" class=\"form-control\" value=\"{1}\"  />", _taskCondition?.ConditionSurrogate, Functions.TrimRight(field.GetCustomDataValue<string>("ResponseDescription")));
                    //                    }

                    //                    sb.AppendFormat("<input type=\"hidden\" name=\"OKNOK_{0}\" id=\"OKNOK_{0}\" readonly=\"readonly\" class=\"form-control\" value=\"{1}\"  />", _taskCondition?.ConditionSurrogate, field.GetCustomData("Response").ToString());
                    //                    break;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        sb.Append("<input type=\"text\" readonly=\"readonly\" class=\"form-control\" />");
                    //    }

                    //    sb.Append("</div>");
                    //}
                    //break;
            }

            sb.Append("<div class=\"clearfix\"></div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        //private void SetValues()
        //{
        //    if (_fields != null)
        //    {
        //        foreach (var field in _fields)
        //        {
        //            switch (field.Field)
        //            {
        //                case TaskFeedbackTemplateFormField.Asset:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("Asset", _taskCondition.Asset);
        //                        field.SetCustomData("AssetName", _taskCondition.AssetName);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.Condition:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("ConditionDescription", _taskCondition.ConditionChecklistDescription);
        //                        field.SetCustomData("Condition", _taskCondition.Condition);
        //                        field.SetCustomData("ConditionSurrogate", _taskCondition?.ConditionSurrogate);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.Sequence:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("Sequence", _taskCondition?.ConditionSequenceNo);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.ResponseType:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("ConditionDataTypeDescription", _taskCondition.ConditionDataType.GetLocalizedDisplayName());
        //                        field.SetCustomData("ConditionDataType", _taskCondition.ConditionDataType);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.Comments:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("Comments", _taskCondition.GetCustomDataValue<string>("TaskConditionExtensionComment"));
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.CompletedDate:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("CompletedDate", _taskCondition.ConditionDateRecorded);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.RequiredBy:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("RequiredByDateTime", _taskCondition.GetCustomDataValue<DateTime>("RequiredBy"));
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.Response:
        //                    if (_taskCondition != null)
        //                    {
        //                        switch (_taskCondition.ConditionDataType)
        //                        {
        //                            case ConditionDataTypeField.Character:
        //                            case ConditionDataTypeField.Status:
        //                                field.SetCustomData("Response", _taskCondition.ConditionValue);
        //                                break;
        //                            case ConditionDataTypeField.Numeric:
        //                                decimal numericValue = 0;
        //                                var isDecimal = decimal.TryParse(_taskCondition.ConditionValue, NumberStyles.AllowDecimalPoint, new CultureInfo("en-GB"), out numericValue);

        //                                if (!isDecimal)
        //                                {
        //                                    isDecimal = decimal.TryParse(_taskCondition.ConditionValue, out numericValue);
        //                                }
        //                                if (isDecimal)
        //                                {
        //                                    field.SetCustomData("Response", numericValue);
        //                                }
        //                                field.SetCustomData("MinValue", _taskCondition.ConditionMinimumValue);
        //                                field.SetCustomData("ConditionMinimumZero", _taskCondition.ConditionMinimumZero);
        //                                field.SetCustomData("MaxValue", _taskCondition.ConditionMaximumValue);
        //                                field.SetCustomData("ConditionMaximumZero", _taskCondition.ConditionMaximumZero);

        //                                break;
        //                            case ConditionDataTypeField.Boolen:
        //                                BooleanResponseFieldYN booleanResponseField = _taskCondition.ConditionValue.FromEnumStringCode<BooleanResponseFieldYN>();
        //                                field.SetCustomData("Response", booleanResponseField);
        //                                if (booleanResponseField != BooleanResponseFieldYN.None)
        //                                {
        //                                    field.SetCustomData("ResponseDescription", booleanResponseField == BooleanResponseFieldYN.Yes ? Functions.GetLabel("lblYes") : Functions.GetLabel("lblNo"));
        //                                }

        //                                break;
        //                            case ConditionDataTypeField.Date:
        //                            case ConditionDataTypeField.DateTime:
        //                                if (!string.IsNullOrWhiteSpace(_taskCondition.ConditionValue))
        //                                {
        //                                    bool isValidDate = DateTime.TryParse(_taskCondition.ConditionValue, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime date);
        //                                    if (isValidDate)
        //                                    {
        //                                        field.SetCustomData("Response", date);
        //                                    }
        //                                }
        //                                break;
        //                            case ConditionDataTypeField.Signature:
        //                                if (!string.IsNullOrWhiteSpace(_taskCondition.ConditionValue))
        //                                {
        //                                    var digitalSignatureImage64BitString = DigitalSignatureLogic.GetDigitalSignature(Functions.ToInt(_taskCondition.ConditionValue));
        //                                    field.SetCustomData("Response", digitalSignatureImage64BitString);
        //                                    field.SetCustomData("SignatureID", _taskCondition.ConditionValue);
        //                                }
        //                                break;
        //                            case ConditionDataTypeField.PassFail:
        //                                if (!string.IsNullOrWhiteSpace(_taskCondition.ConditionValue))
        //                                {
        //                                    ConditionDataTypePassFailField conditionDataTypePassFailField = _taskCondition.ConditionValue.FromEnumStringCode<ConditionDataTypePassFailField>();

        //                                    field.SetCustomData("Response", conditionDataTypePassFailField);
        //                                    if (conditionDataTypePassFailField != ConditionDataTypePassFailField.None)
        //                                    {
        //                                        field.SetCustomData("ResponseDescription", conditionDataTypePassFailField == ConditionDataTypePassFailField.Pass ? Functions.GetLabel("lblPass") : Functions.GetLabel("lblFail"));
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    field.SetCustomData("Response", ConditionDataTypePassFailField.None);
        //                                    field.SetCustomData("ResponseDescription", "");
        //                                }
        //                                break;
        //                            case ConditionDataTypeField.OKNOK:
        //                                if (!string.IsNullOrWhiteSpace(_taskCondition.ConditionValue))
        //                                {
        //                                    ConditionDataTypeOkNotOKField conditionDataTypeOkNotOKField = _taskCondition.ConditionValue.FromEnumStringCode<ConditionDataTypeOkNotOKField>();

        //                                    field.SetCustomData("Response", conditionDataTypeOkNotOKField);
        //                                    if (conditionDataTypeOkNotOKField != ConditionDataTypeOkNotOKField.None)
        //                                    {
        //                                        field.SetCustomData("ResponseDescription", conditionDataTypeOkNotOKField == ConditionDataTypeOkNotOKField.Ok ? Functions.GetLabel("OK_String") : Functions.GetLabel("lblNOK"));
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    field.SetCustomData("Response", ConditionDataTypeOkNotOKField.None);
        //                                    field.SetCustomData("ResponseDescription", "");
        //                                }
        //                                break;
        //                        }
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.Feedback:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("Feedback", _taskCondition.ConditionResponse);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.ExpectedValue:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("ExpectedValue", _taskCondition.ConditionExpectedValue);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.MinMaxValue:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("MinValue", _taskCondition.ConditionMinimumValue);
        //                        field.SetCustomData("ConditionMinimumZero", _taskCondition.ConditionMinimumZero);
        //                        field.SetCustomData("MaxValue", _taskCondition.ConditionMaximumValue);
        //                        field.SetCustomData("ConditionMaximumZero", _taskCondition.ConditionMaximumZero);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.UnitOfMeasure:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("UnitOfMeasure", _taskCondition.UnitOfMeasure);
        //                        field.SetCustomData("UOMDescription", _taskCondition.GetCustomDataValue<string>("UOMDescription"));
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.Function:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("Function", _taskCondition.GetCustomDataValue<string>("Function"));
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.MachinePart:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("MachinePart", _taskCondition.GetCustomDataValue<string>("MachinePart"));
        //                    }
        //                    break;

        //                case TaskFeedbackTemplateFormField.Priority:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("PriorityDescription", _taskCondition.GetCustomDataValue<string>("PriorityDescription"));
        //                        field.SetCustomData("Priority", _taskCondition.GetCustomDataValue<string>("Priority"));
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.PlanningConstraint:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("PlanningConstraintDescription", _taskCondition.GetCustomDataValue<string>("PlanningConstraintDescription"));
        //                        field.SetCustomData("PlanningConstraint", _taskCondition.GetCustomDataValue<string>("PlanningConstraint"));
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.Constraint:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("ConstraintDescription", _taskCondition.GetCustomDataValue<string>("ConstraintDescription"));
        //                        field.SetCustomData("Constraint", _taskCondition.GetCustomDataValue<string>("Constraint"));
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.Instrument:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("Instrument", _taskCondition.InstrumentAssetReference);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.StandardTolerance:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("StandardTolerance", _taskCondition.ConditionStandardTolerance);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.ToolsMaterialMethods:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("ToolsMaterialMethods", _taskCondition.ConditionMethod);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.RecommendedAction:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("RecommendedAction", _taskCondition.ConditionCorrectiveActions);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.ActivityType:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("ActivityTypeDescription", _taskCondition.GetCustomDataValue<string>("ActivityTypeDescription"));
        //                        field.SetCustomData("ActivityType", _taskCondition.GetCustomDataValue<string>("ActivityType"));
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.Duration:
        //                    if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("Duration", _taskCondition.Duration);
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.ConditionImage:
        //                    if (RenderForDragnDrop)
        //                    {
        //                        var images = new List<TaskConditionImageBE>() {
        //                                new TaskConditionImageBE()
        //                            };
        //                        field.SetCustomData("ConditionImage", images);
        //                    }
        //                    else if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("ConditionImage", _taskCondition.GetCustomData("ImageArray"));
        //                    }
        //                    break;
        //                case TaskFeedbackTemplateFormField.Equipment:
        //                    if (RenderForDragnDrop)
        //                    {
        //                        var equipments = new List<EquipmentBE>() {
        //                                new EquipmentBE()
        //                            };
        //                        field.SetCustomData("Equipments", equipments);
        //                    }
        //                    else if (_taskCondition != null)
        //                    {
        //                        field.SetCustomData("Equipments", _taskCondition.GetCustomData("Equipments"));
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //}

        //private bool HasValue(ScreenDesignTemplateTaskFeedbackDetailBE field)
        //{
        //    bool hasValue = false;
        //    if (RenderForDragnDrop)
        //    {
        //        hasValue = true;
        //    }
        //    else
        //    {
        //        switch (field.Field)
        //        {
        //            case TaskFeedbackTemplateFormField.Asset:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(_taskCondition.Asset);
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.Condition:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(_taskCondition.Condition);
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.Sequence:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = _taskCondition.ConditionSequenceNo > 0;
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.ResponseType:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = true;
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.Comments:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(Functions.TrimRight(_taskCondition.GetCustomDataValue<string>("TaskConditionExtensionComment")));
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.CompletedDate:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = true;
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.RequiredBy:
        //                if (_taskCondition != null)
        //                {
        //                    var requiredBy = _taskCondition.GetCustomDataValue<DateTime>("RequiredBy");
        //                    hasValue = (requiredBy != DateTime.MinValue);
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.Response:
        //                if (_taskCondition != null)
        //                {
        //                    switch (_taskCondition.ConditionDataType)
        //                    {
        //                        case ConditionDataTypeField.Character:
        //                        case ConditionDataTypeField.Status:
        //                        case ConditionDataTypeField.Numeric:
        //                        case ConditionDataTypeField.Boolen:
        //                        case ConditionDataTypeField.Signature:
        //                        case ConditionDataTypeField.PassFail:
        //                        case ConditionDataTypeField.OKNOK:
        //                        case ConditionDataTypeField.Date:
        //                        case ConditionDataTypeField.DateTime:
        //                            hasValue = true;
        //                            break;
        //                    }
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.Feedback:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = true;
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.ExpectedValue:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = true;
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.MinMaxValue:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = ((_taskCondition.ConditionMinimumValue != 0 || (_taskCondition.ConditionMinimumZero && _taskCondition.ConditionMinimumValue == 0)) || (_taskCondition.ConditionMaximumValue != 0 || (_taskCondition.ConditionMaximumZero && _taskCondition.ConditionMaximumValue == 0)));
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.UnitOfMeasure:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(_taskCondition.UnitOfMeasure);
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.Function:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(Functions.TrimRight(_taskCondition.GetCustomDataValue<string>("Function")));
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.MachinePart:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(Functions.TrimRight(_taskCondition.GetCustomDataValue<string>("MachinePart")));
        //                }
        //                break;

        //            case TaskFeedbackTemplateFormField.Priority:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(Functions.TrimRight(_taskCondition.GetCustomDataValue<string>("Priority")));
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.PlanningConstraint:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(Functions.TrimRight(_taskCondition.GetCustomDataValue<string>("PlanningConstraint")));
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.Constraint:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(Functions.TrimRight(_taskCondition.GetCustomDataValue<string>("Constraint")));
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.Instrument:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(_taskCondition.InstrumentAssetReference);
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.StandardTolerance:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(_taskCondition.ConditionStandardTolerance);
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.ToolsMaterialMethods:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(_taskCondition.ConditionMethod);
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.RecommendedAction:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(_taskCondition.ConditionCorrectiveActions);
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.ActivityType:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = !string.IsNullOrEmpty(Functions.TrimRight(_taskCondition.GetCustomDataValue<string>("ActivityType")));
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.Duration:
        //                if (_taskCondition != null)
        //                {
        //                    hasValue = _taskCondition.Duration > 0;
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.ConditionImage:
        //                if (_taskCondition != null)
        //                {
        //                    var images = field.GetCustomData("ConditionImage");
        //                    if (images != null)
        //                    {
        //                        hasValue = ((List<TaskConditionImageBE>)(images)).Count > 0;
        //                    }
        //                }
        //                break;
        //            case TaskFeedbackTemplateFormField.Equipment:
        //                if (_taskCondition != null)
        //                {
        //                    var equipments = field.GetCustomData("Equipments");
        //                    if (equipments != null)
        //                    {
        //                        hasValue = ((List<EquipmentBE>)(equipments)).Count > 0;
        //                    }
        //                }
        //                break;
        //        }
        //    }
        //    return hasValue;
        //}
        #endregion
    }
}