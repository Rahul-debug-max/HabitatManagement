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
            // SetValues();
        }

        public FormDesignTemplateModelBE(List<PermitFormScreenDesignTemplateDetailBE> fields, TemplateFormFieldDataBE templateFormFieldDataBE)
            : this(fields)
        {
            _templateFormFieldData = templateFormFieldDataBE;
            // SetValues();
        }

        public FormDesignTemplateModelBE(List<PermitFormScreenDesignTemplateDetailBE> fields, List<TemplateFormFieldDataBE> templateFormFieldDataList)
          : this(fields)
        {
            _templateFormFieldDataList = templateFormFieldDataList;
            // SetValues();
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

            if (sectionGroupFields != null)
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
                        sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" for=\"FormDate_DatePart_{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none conditiondate\">");
                        DateTime dateTime;
                        bool isSuccess = DateTime.TryParse(_templateFormFieldData?.FieldValue, out dateTime);
                        sb.Append("<div style=\"width: 196px!important; display:inline-block; \" >");
                        sb.Append("<div class=\"input-group \" >");
                        sb.AppendFormat("<input  class=\"dateTextBox form-control\"  type=\"text\" name=\"FormDate_DatePart_{1}\" id=\"FormDate_DatePart_{1}\" value=\"{0}\"/>", dateTime != DateTime.MinValue ? dateTime.ToString() : "", field.Field);
                        sb.Append("<span class=\"input-group-addon input-group-calendar-span\">");
                        sb.AppendFormat("<span id=\"imgClearFormDateTime_{0}\" class=\"ui-datepicker-trigger glyphicons glyphicons-remove\" style=\"cursor: pointer !important;\" />", _templateFormFieldData?.FieldValue);
                        sb.Append("</span>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                        break;
                    case FormFieldType.DateAndTime:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
                        sb.AppendFormat("<label class=\"col-sm-12 col-md-4 col-lg-4 control-label paddrght-none text-right smtxtlft\" for=\"FormDate_DatePart_{1}\">{0}</label>", field.FieldName, field.Field);
                        sb.Append("<div class=\"col-sm-12 col-md-8 col-lg-8 paddrght-none conditiondate\">");
                        DateTime dateAndTime;
                        bool success = DateTime.TryParse(_templateFormFieldData?.FieldValue, out dateAndTime);
                        sb.Append("<div style=\"width: 196px!important; display:inline-block; \" >");
                        sb.Append("<div class=\"input-group \" >");
                        sb.AppendFormat("<input  class=\"dateTextBox form-control\"  type=\"text\" name=\"FormDate_DatePart_{1}\" id=\"FormDate_DatePart_{1}\" value=\"{0}\"/>", dateAndTime != DateTime.MinValue ? dateAndTime.ToString() : "", field.Field);
                        sb.Append("<span class=\"input-group-addon input-group-calendar-span\">");
                        sb.AppendFormat("<span id=\"imgClearFormDateTime_{0}\" class=\"ui-datepicker-trigger glyphicons glyphicons-remove\" style=\"cursor: pointer !important;\" />", _templateFormFieldData?.FieldValue);
                        sb.Append("</span>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("<div style=\"width: 117px!important; display:inline-block;padding-left: 10px; \" >");
                        sb.Append("<div class=\"timectrlwidth input-group \" >");
                        sb.AppendFormat("<input  class=\"timeTextBox form-control\" type=\"text\" name=\"FormDate_TimePart_{1}\" id=\"FormDate_TimePart_{1}\" value=\"{0}\" />", dateAndTime != DateTime.MinValue ? dateAndTime.ToString() : "", field.Field);
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                        break;
                    case FormFieldType.Signature:
                        _templateFormFieldData = _templateFormFieldDataList.Where(s => s.Field == field.Field).FirstOrDefault();
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

                        break;
                }
            }

            sb.Append("</div>");
            sb.Append("<p></p>");

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