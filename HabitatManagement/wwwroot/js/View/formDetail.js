window.PermitFormFieldDetail = (function () {
    var defaults = {
        formID: 0,
        addEditPopupTitle: "Add/Edit Form Field",
        addEditURL: '',
        deleteURL: ''
    }

    var selectedRow = [];
    var selectedSectionRow = [];

    var onInit = function (obj) {
        $.extend(defaults, obj);

        window.setTimeout(PermitFormFieldDetail.reloadGridForPositioner, 200);

        $('#AddSection').off('click').on('click', function (e) {            
            e.stopPropagation();
            fireonClick('addSection');
        });

        $('#EditSection').off('click').on('click', function (e) {
            e.stopPropagation();
            fireonClick('editSection', true);
        });

        $('#DeleteSection').off('click').on('click', function (e) {
            e.stopPropagation();
            fireonClick('delSection', true);
        });

        $('#Add').off('click').on('click', function (e) {
            e.stopPropagation();
            fireonClick('add');
        });

        $('#Edit').off('click').on('click', function (e) {
            e.stopPropagation();
            fireonClick('edit', true);
        });

        $('#Delete').off('click').on('click', function (e) {
            e.stopPropagation();
            fireonClick('del', true);
        });
    }

    var fireonClick = function (clickFor, selectionRequired) {
        var isValid = true;
        if (selectionRequired) {
            if ((selectedSectionRow.length <= 0 && (clickFor == "delSection" || clickFor == "editSection"))) {
                ExtendedDialog.openOkBtnDialog({
                    requiredDialogTitle: "Entity not selected",
                    requiredDialogMessage: "Select entity"
                });
                isValid = false;
            }
            else if ((selectedRow.length <= 0 && (clickFor == "del" || clickFor == "edit"))) {
                ExtendedDialog.openOkBtnDialog({
                    requiredDialogTitle: "Entity not selected",
                    requiredDialogMessage: "Select entity"
                });
                isValid = false;
            }
        }

        if (isValid) {          
            var surrogateDate = selectedRow;
            var surrogateSectionDate = selectedSectionRow;
            switch (clickFor) {
                case 'delSection':
                    ExtendedDialog.openConfirmationDialogWithAJAX({
                        url: defaults.deleteSectionURL,
                        formData: surrogateSectionDate[0],
                        traditional: true,
                        onSuccess: function (result) {
                            if (!result.success) {
                                showAndDismissAlert('danger', wcmVariables.dataSaveErrMsg);
                            }
                            else {
                                selectedRow = [];
                                reloadGridForPositioner();
                            }
                        },
                        onError: function (result) {
                            showAndDismissAlert('danger', wcmVariables.dataSaveErrMsg);
                        }
                    });
                    break;
                case 'addSection':
                    ExtendedDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-xl",
                        url: defaults.addEditSectionURL,
                        data: { formID: defaults.formID, sectionName: "" },
                        buttons: [
                            {
                                Button: 'save', onClick: function () {
                                    var dialogID = $(this).attr("aria-modalID");
                                    saveSectionDetail(dialogID);
                                }
                            }
                        ],
                        onOpen: function (instance) {

                        }
                    });
                    break;
                case 'editSection':
                    ExtendedDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-xl",
                        url: defaults.addEditSectionURL,
                        data: surrogateSectionDate[0],
                        buttons: [
                            {
                                Button: 'save', onClick: function () {
                                    var dialogID = $(this).attr("aria-modalID");
                                    saveSectionDetail(dialogID);
                                }
                            }
                        ]
                    });
                    break;
                case 'del':
                    ExtendedDialog.openConfirmationDialogWithAJAX({
                        url: defaults.deleteURL,
                        formData: { formID: defaults.formID, fieldID: surrogateDate[0] },
                        traditional: true,
                        onSuccess: function (result) {
                            if (!result.success) {
                                showAndDismissAlert('danger', wcmVariables.dataSaveErrMsg);
                            }
                            else {
                                selectedRow = [];
                                reloadGridForPositioner();
                            }
                        },
                        onError: function (result) {
                            showAndDismissAlert('danger', wcmVariables.dataSaveErrMsg);
                        }
                    });
                    break;
                case 'add':
                    ExtendedDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-xl",
                        url: defaults.addEditURL,
                        data: { formID: defaults.formID, fieldID: 0 },
                        buttons: [
                            {
                                Button: 'save', onClick: function () {
                                    var dialogID = $(this).attr("aria-modalID");
                                    saveFormField(dialogID);
                                }
                            }
                        ],
                        onOpen: function (instance) {

                        }
                    });
                    break;
                case 'edit':
                    ExtendedDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-xl",
                        url: defaults.addEditURL,
                        data: { formID: defaults.formID, fieldID: surrogateDate[0] },
                        buttons: [
                            {
                                Button: 'save', onClick: function () {
                                    var dialogID = $(this).attr("aria-modalID");
                                    saveFormField(dialogID);
                                }
                            }
                        ]
                    });
                    break;
            }
        }
    }

    var saveSectionDetail = function (dialogID) {       
                
        var ajx = $.ajax({
            type: 'POST',
            cache: false,
            url: defaults.addEditSectionURL,
            dataType: 'JSON',
            data: $('#TemplateSectionForm').serialize(),
            success: function (result) {
                if (result.success) {
                    $("#" + dialogID).modal("hide");
                    selectedSectionRow = [];
                    renderSectionList();
                }
                else if (result.Success != undefined && !result.Success) {
                    showAndDismissAlert('danger', wcmVariables.dataSaveErrMsg);
                }
            },
            error: function () {
            },
            beforeSend: function () { $("#wait").css("display", "block"); },
            complete: function () {
                $("#wait").css("display", "none");
            }
        });
        return ajx;
    }

    var renderSectionList = function () {        
        $.ajax({
            url: defaults.getSectionListURL,
            cache: false,            
            data: { formID: defaults.formID },
            success: function (result) {               
                $("#dvSectionDetail").html(result);               
            },
            error: function () {

            },
            beforeSend: function () { $("#wait").css("display", "block"); },
            complete: function () {
                $("#wait").css("display", "none");
            }
        });
    }


    var saveFormField = function (dialogID) {

        var tableFieldTypeMaster = new Array();
        $(".tableType TH").each(function () {        
            var row = $(this);
            var tableColumn = row.html();
            var columnfieldType = row.attr("columnfieldtype");

            var tableFieldType = {};
            tableFieldType.ColumnName = tableColumn;
            tableFieldType.RowCount = $("#TableRowCount").val();
            tableFieldType.ColumnType = columnfieldType;
            tableFieldTypeMaster.push(tableFieldType);
        });

        var ajx = $.ajax({
            type: 'POST',
            cache: false,
            url: defaults.addEditURL,
            dataType: 'JSON',
            data: $('#PermitFormField').serialize() + '&' + $.param({ tableFieldTypeMaster: tableFieldTypeMaster }),
            success: function (result) {
                if (result.success) {
                    $("#" + dialogID).modal("hide");
                    selectedRow = [];
                    reloadGridForPositioner();
                }
                else if (result.Success != undefined && !result.Success) {
                    showAndDismissAlert('danger', wcmVariables.dataSaveErrMsg);
                }
            },
            error: function () {
            },
            beforeSend: function () { $("#wait").css("display", "block"); },
            complete: function () {
                $("#wait").css("display", "none");
            }
        });
        return ajx;
    }

    var reloadGridForPositioner = function () {
        var formDesignerColumnNames = [];
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
                            formDesignerColumnNames.push(data.columnNames[i]);
                        }
                    }
                }
            },
            complete: function () {
                formDesignerGrid(formDesignerColumnNames);
            }
        });
    }

    var formDesignerGrid = function (formDesignerColumnNames) {
        $("#tblFieldDesigner").jqGrid("GridUnload");
        $("#tblFieldDesigner").jqGrid({
            url: defaults.jqGridDataURL,
            datatype: 'json',
            mtype: 'Post',
            colNames: formDesignerColumnNames,
            colModel: [
                { key: false, name: 'field', index: 'field', search: false, hidden: true },
                { key: false, name: 'fieldName', index: 'fieldName', search: false, width: 350 },
                { key: false, name: 'fieldTypeValue', index: 'fieldTypeValue', search: false, width: 150 },
                { key: false, name: 'section', index: 'section', search: false, width: 200 },
                { key: false, name: 'mandatoryField', index: 'mandatoryField', search: false, width: 100 },
                { key: false, name: 'sequence', index: 'sequence', search: false, width: 100 }
            ],
            pager: jQuery('#pagerFieldDesigner'),
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
                        formID: defaults.formID
                    }
                });
            },
            gridComplete: function (e) {
            },
        }).navGrid('#pagerFieldDesigner', { edit: false, add: false, del: false, search: false, refresh: false })

        $("#tblFieldDesigner").jqGrid('setLabel', 'Select', '', { 'text-align': 'center' });
    }

    var getSelectedTemplate = function () {
        var gr = $("#tblFieldDesigner").getGridParam('selrow');
        if (gr != null) {
            var surrogate = $("#tblFieldDesigner").getRowData(gr).field;
            selectedRow = [];
            selectedRow.push(surrogate);
        }
    }

    var getSelectedSection = function (e) {
        var formID = $(e).attr('data-FormID');
        var section = $(e).attr('data-Section');
        selectedSectionRow = [];
        selectedSectionRow.push({ formID: formID, section: section });
    }

    return {
        onInit: onInit,
        fireonClick: fireonClick,
        reloadGridForPositioner: reloadGridForPositioner,
        getSelectedTemplate: getSelectedTemplate,
        saveFormField: saveFormField,
        getSelectedSection: getSelectedSection
    }
}());