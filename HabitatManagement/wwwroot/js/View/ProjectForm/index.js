window.ProjectFormList = (function () {
    var defaults = {
        addEditPopupTitle: "Form",
        addEditURL: '',
        deleteURL: ''
    }

    var selectedRow = [];

    var onInit = function (obj) {
        $.extend(defaults, obj);
        InitiliazeToolbarScrollBar('ProjectFormFlexi');
        reloadGridForPositioner();
        $("#RefreshProjectForm").bind('click', function () {
            selectedRow = [];
            reloadGridForPositioner();
        });

        $('#AddProjectForm').off('click').on('click', function () {
            fireonClick('add');
        });

        $('#EditProjectForm').off('click').on('click', function () {
            fireonClick('edit', true);
        });

        $('.formFeedbackSelector').on('change', function () {
            reloadGridForPositioner();
        });
    }

    var fireonClick = function (clickFor, selectionRequired) {
        var isValid = true;
        if (selectionRequired) {
            if (selectedRow.length <= 0) {
                WCMDialog.openOkBtnDialog({
                    requiredDialogTitle: "Entity not selected",
                    requiredDialogMessage: "Select entity"
                });
                isValid = false;
            }
        }

        if (isValid) {
            var surrogateDate = selectedRow;
            switch (clickFor) {
                case 'add':
                    WCMDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-xl",
                        url: defaults.addEditURL,
                        data: { projectID: defaults.projectId, formID: $('.formFeedbackSelector').val() > 0 ? $('.formFeedbackSelector').val() : 0 },
                        buttons: [
                            {
                                Button: 'save', onClick: function () {
                                    var dialogID = $(this).attr("aria-modalID");
                                    currentDialogID = $("#" + dialogID);
                                    saveDesignTemplate(currentDialogID);
                                }
                            }
                        ],
                        onOpen: function (instance) {

                        }
                    });
                    break;
                case 'edit':
                    WCMDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-xl",
                        url: defaults.addEditURL,
                        data: { projectID: defaults.projectId, formID: surrogateDate[0].formID, surrogate: surrogateDate[0].surrogate },
                        buttons: [
                            {
                                Button: 'save', onClick: function () {
                                    var dialogID = $(this).attr("aria-modalID");
                                    currentDialogID = $("#" + dialogID);
                                    saveDesignTemplate(currentDialogID);
                                }
                            }
                        ]
                    });
                    break;
            }
        }
    }

    var saveDesignTemplate = function (currentDialogID) {
        var data = [];
        $('#dvFormFeedback').find("div[data-field]").each(function (inx, ele) {
            var field = 0;
            var fieldValue = '';
            var digitalSignatureImage64BitString = '';

            obj = $(ele);
            var field = obj.attr('data-field');
            var fieldType = obj.attr('field-Type');

            if (fieldType == defaults.checkListFieldType) {
                $(obj).find('.checkListTR').each(function (inx, ele) {
                    var checked = '';
                    var yesCheckBox = $(ele).find('input')[0];
                    var noCheckBox = $(ele).find('input')[1];
                    field = $(yesCheckBox).attr('name');

                    if ($(yesCheckBox).is(":checked")) {
                        checked = 1;
                    }
                    else if ($(noCheckBox).is(":checked")) {
                        checked = 0;
                    }
                    data.push({
                        FormID: $('.projectFormCreation').val(),
                        Field: field,
                        FieldValue: checked,
                        DigitalSignatureImage64BitString: "",
                        FieldType: ""
                    });
                });
            }
            else if (fieldType == defaults.checkboxFieldType) {
                var yesCheckBoxElement = $(obj).find('.formFieldTypeCheckbox').find('input[type=checkbox]')[0];
                var noCheckBoxElement = $(obj).find('.formFieldTypeCheckbox').find('input[type=checkbox]')[1];
                var checked = '';
                if ($(yesCheckBoxElement).is(":checked")) {
                    checked = 1;
                }
                else if ($(noCheckBoxElement).is(":checked")) {
                    checked = 0;
                }
                fieldValue = checked;
            }
            else if (fieldType == defaults.textboxFieldType) {
                fieldValue = $(obj).find('input[name^=' + field + ']').val();
            }
            else if (fieldType == defaults.dateFieldType) {
                fieldValue = $(obj).find('input[name^=' + field + ']').val();
            }
            else if (fieldType == defaults.dateAndTimeFieldType) {
                var date = $(obj).find('input[name^=' + field + ']').val();
                var time = $(obj).find('.time').find('input').val()
                fieldValue = date + " " + time;
            }
            else if (fieldType == defaults.signatureFieldType) {
                var singnatureDv = $(obj).find('div[id^="digitalSignature_"]');
                if (singnatureDv.length > 0) {
                    digitalSignatureImage64BitString = $(singnatureDv).jSignature('getData');
                }
                fieldValue = $(obj).find('#SignatureId').val();
            }
            else if (fieldType == defaults.textAreaFieldType) {
                fieldValue = $(obj).find('textarea[name^=' + field + ']').val();
            }
            if (fieldType != defaults.checkListFieldType) {
                data.push({
                    FormID: $('.projectFormCreation').val(),
                    Field: field,
                    FieldValue: fieldValue,
                    DigitalSignatureImage64BitString: digitalSignatureImage64BitString,
                    FieldType: fieldType == defaults.signatureFieldType ? defaults.signatureField : ''
                });
            }
        });
      
        var ajx = $.ajax({
            type: 'Post',
            url: defaults.saveDataURL,
            cache: false,
            data: { data: JSON.stringify(data), surrogate: $('#FormFeedback #Surrogate').val(), projectID: defaults.projectId },
            dataType: 'Json',
            traditional: true,
            success: function (result) {
                if (result != null && !result) {
                    alert('Unable to save. Please contact administrator.');
                    return;
                }
                else {
                    $(currentDialogID).modal("hide");
                    reloadGridForPositioner();
                }
            },
            error: function (jqXHR, feedback, eToStringToStringrrorThrown) {
            },
            beforeSend: function () {
                $('#wait').show();
            },
            complete: function () {
                $('#wait').hide();
            }
        });
        return ajx;
    }

    var reloadGridForPositioner = function () {
        var createdFormListColumnName = [];
        $.ajax({
            type: 'GET',
            cache: false,
            url: defaults.jqGridColumnURL,
            dataType: "json",
            contentType: "application/json charset=utf-8",
            success: function (data) {
                if (data.columnNames != null) {
                    for (var i = 0; i < data.columnNames.length; i++) {
                        if (data.columnNames[i] != "") {
                            createdFormListColumnName.push(data.columnNames[i]);
                        }
                    }
                }
            },
            complete: function () {
                createdFormGrid(createdFormListColumnName);
            }
        });
    }

    var createdFormGrid = function (createdFormListColumnName) {
        $("#tblProjectFormList").jqGrid("GridUnload");
        $("#tblProjectFormList").jqGrid({
            url: defaults.jqGridDataURL,
            datatype: 'json',
            mtype: 'Post',
            colNames: createdFormListColumnName,
            colModel: [
                { key: false, name: 'formfurrogate', index: 'formfurrogate', search: false, hidden: true },
                { key: false, name: 'surrogate', index: 'surrogate', search: false },
                { key: false, name: 'design', index: 'design', search: false },
                { key: false, name: 'description', index: 'description', search: false },
                { key: false, name: 'creationDate', index: 'creationDate', search: false }
            ],
            pager: jQuery('#pagerProjectFormList'),
            rowNum: 10,
            rowList: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
            height: '100%',
            viewrecords: true,
            caption: '',
            emptyrecords: 'No records to display',
            jsonReader: {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                Id: "0"
            },
            autowidth: true,
            multiselect: false,
            onSelectRow: function (id) {
                getSelectedTemplate();
            },
            ondblClickRow: function (id) {
                fireonClick('edit', true);
            },
            beforeRequest: function () {
                $(this).jqGrid('setGridParam', {
                    postData: {
                        formID: $('.formFeedbackSelector').val() > 0 ? $('.formFeedbackSelector').val() : 0,
                        projectID: defaults.projectId
                    }
                });
            },
            gridComplete: function (e) {
            },
        }).navGrid('#pagerProjectFormList', { edit: false, add: false, del: false, search: false, refresh: false })

        $("#tblProjectFormList").jqGrid('setLabel', 'Select', '', { 'text-align': 'center' });
    }

    var getSelectedTemplate = function () {
        var gr = $("#tblProjectFormList").getGridParam('selrow');
        if (gr != null) {
            var surrogate = $("#tblProjectFormList").getRowData(gr).surrogate;
            var formID = $("#tblProjectFormList").getRowData(gr).formfurrogate;
            selectedRow = [];
            selectedRow.push({ formID: formID, surrogate: surrogate });
        }
    }

    return {
        onInit: onInit,
        fireonClick: fireonClick,
        reloadGridForPositioner: reloadGridForPositioner,
        getSelectedTemplate: getSelectedTemplate,
        saveDesignTemplate: saveDesignTemplate
    }
}());