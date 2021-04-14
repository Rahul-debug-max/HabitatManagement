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
            if (selectedRow.length <= 0) {
                WCMDialog.openOkBtnDialog({
                    requiredDialogTitle: "Entity not selected",
                    requiredDialogMessage: "Select entity"
                });
                isValid = false;
            }
        }

        if (isValid) {
            debugger;
            var surrogateDate = selectedRow;
            var surrogateSectionDate = selectedSectionRow;
            switch (clickFor) {
                case 'delSection':
                    WCMDialog.openConfirmationDialogWithAJAX({
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
                    WCMDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-lg",
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
                    WCMDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-lg",
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
                    WCMDialog.openConfirmationDialogWithAJAX({
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
                    WCMDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-lg",
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
                    WCMDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-lg",
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
        debugger;
        $.ajax({
            url: defaults.getSectionListURL,
            cache: false,            
            data: { formID: defaults.formID },
            success: function (result) {
                debugger;
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
        var ajx = $.ajax({
            type: 'POST',
            cache: false,
            url: defaults.addEditURL,
            dataType: 'JSON',
            data: $('#PermitFormField').serialize(),
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
                { key: false, name: 'fieldName', index: 'fieldName', search: false },
                { key: false, name: 'fieldTypeValue', index: 'fieldTypeValue', search: false },
                { key: false, name: 'section', index: 'section', search: false },
                { key: false, name: 'sequence', index: 'sequence', search: false }
            ],
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
        })

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

    return {
        onInit: onInit,
        fireonClick: fireonClick,
        reloadGridForPositioner: reloadGridForPositioner,
        getSelectedTemplate: getSelectedTemplate,
        saveFormField: saveFormField

    }
}());