window.PermitFormTemplateList = (function () {
    var defaults = {
        addEditPopupTitle: "Form",
        addEditURL: '',
        deleteURL: ''
    }

    var checkBoxSelectionData = [];
    var selectedRow = [];

    var onInit = function (obj) {
        $.extend(defaults, obj);
        InitiliazeToolbarScrollBar('FormTemplateFlexi');
        reloadGridForPositioner();
        $("#RefreshFormTemplate").bind('click', function () {
            checkBoxSelectionData = [];
            selectedRow = [];
            reloadGridForPositioner();
        });

        $('#AddFormTemplate').off('click').on('click', function () {
            fireonClick('add');
        });

        $('#EditFormTemplate').off('click').on('click', function () {
            fireonClick('edit', true);
        });

        $('#DeleteTaskFeedbackTemplate').off('click').on('click', function () {
            fireonClick('del', true);
        });
    }

    var fireonClick = function (clickFor, selectionRequired) {        
        var isValid = true;
        if (selectionRequired) {
            if (checkBoxSelectionData.length <= 0 && selectedRow.length <= 0) {
                WCMDialog.openOkBtnDialog({
                    requiredDialogTitle: "Entity not selected",
                    requiredDialogMessage: "Select entity"
                });
                isValid = false;
            }
        }

        if (isValid) {
            var surrogateDate = checkBoxSelectionData.length > 0 ? checkBoxSelectionData : selectedRow;
            switch (clickFor) {
                case 'del':
                    WCMDialog.openConfirmationDialogWithAJAX({
                        url: defaults.deleteURL,
                        formData: { formID: surrogateDate },
                        traditional: true,
                        onSuccess: function (result) {
                            if (!result.Success) {
                                showAndDismissAlert('danger', wcmVariables.dataSaveErrMsg);
                            }
                            else {
                                checkBoxSelectionData = [];
                                selectedRow = [];
                                reloadGridForPositioner(true);
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
                        data: { formID: 0 },
                        buttons: [
                            {
                                Button: 'save', onClick: function () {
                                    saveDesignTemplate();
                                }
                            },
                            {
                                Button: 'details', onClick: function () {
                                    saveDesignTemplate(true);
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
                        data: { formID: surrogateDate[0] },
                        buttons: [
                            {
                                Button: 'save', onClick: function () {
                                    saveDesignTemplate();
                                }
                            },
                            {
                                Button: 'details', onClick: function () {
                                    saveDesignTemplate(true);
                                }
                            }
                        ]
                    });
                    break;
            }
        }
    }

    var saveDesignTemplate = function (openDetailDialog) {
        var ajx = $.ajax({
            type: 'POST',
            cache: false,
            url: defaults.addEditURL,
            dataType: 'JSON',
            data: $('#PermitDesignTemplateForm').serialize(),
            success: function (result) {               
                if (result.success) {
                    if (result.id != undefined && result.id > 0) {
                        selectedRow = [];
                        selectedRow.push(result.id);
                        $("#FormID").val(result.id);
                    }
                    reloadGridForPositioner();
                    if (openDetailDialog != undefined && openDetailDialog) {
                        openDetailPopup();
                    }
                }
                else if (result.Success != undefined && !result.Success) {
                    showAndDismissAlert('danger', wcmVariables.dataSaveErrMsg);
                }
            },
            error: function () {
            },
            beforeSend: function () { $("#wait").css("display", "block"); },
            complete: function () {
                if ((openDetailDialog === undefined || !openDetailDialog) || (openDetailDialog && $('#dvTaskFeedbackDesignTemplateValMsg').html() != '')) {
                    $("#wait").css("display", "none");
                }
            }
        });
        return ajx;
    }

    var openDetailPopup = function () {
        var surrogateDate = checkBoxSelectionData.length > 0 ? checkBoxSelectionData : selectedRow;
        WCMDialog.RenderPageInDialogAndOpen(
            {
                title: "Screen Design Details",
                url: defaults.detailURL,
                data: { formID: surrogateDate[0] },
                modalDialogClass: "modal-lg",
                buttons: [
                    {
                        Button: 'next', onClick: function () {                        
                            var dialogID = $(this).attr("aria-modalID");
                            currentDialogID = $("#" + dialogID);
                            onNextClick(currentDialogID);
                        }
                    }
                ],
                onOpen: function () {
                    $("#wait").css("display", "none");
                }
            });
    }

    var onNextClick = function (currentDialogID) {
        $(currentDialogID).modal("hide");
        openTaskFeedbackScreenLayout();
        //var postData = $('#TaskFeedBackTemplateDetailsForm').serialize();
        //$.ajax({
        //    type: 'POST',
        //    cache: false,
        //    url: defaults.detailURL,
        //    data: postData,
        //    success: function (result) {
        //        if (result.Success != undefined && !result.Success) {
        //            showAndDismissAlert('danger', wcmVariables.dataSaveErrMsg);
        //            $("#wait").css("display", "none");
        //        }
        //        else {
        //            $(currentDialogID).dialog('close');
        //            openTaskFeedbackScreenLayout();
        //        }
        //    },
        //    error: function () {
        //    },
        //    beforeSend: function () { $("#wait").css("display", "block"); }
        //});
    }

    var openTaskFeedbackScreenLayout = function () {
        WCMDialog.RenderPageInDialogAndOpen(
            {
                title: "Permit Form Layout",
                url: defaults.screenLayoutURL,
                data: { formID: $("#FormID").val() },
                modalDialogClass: "modal-xl",
                buttons: [
                    {
                        Button: 'prev', onClick: function () {
                            var dialogID = $(this).attr("aria-modalID");
                            currentDialogID = $("#" + dialogID);
                            $(currentDialogID).modal("hide");
                            openDetailPopup();
                        }
                    }
                ]
            });
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
        $("#tblFormDesigner").jqGrid("GridUnload");
        $("#tblFormDesigner").jqGrid({
            url: defaults.jqGridDataURL,
            datatype: 'json',
            mtype: 'Post',
            colNames: formDesignerColumnNames,
            colModel: [
                //{
                //    name: 'Select', width: 5, index: 'SelectForm', editable: false, sortable: false, edittype: 'checkbox', align: 'center', search: false, editoptions: { value: "True:False" },
                //    formatter: function (cellvalue, options, rowObject) {
                //        return '<input id="chk_' + rowObject.FormID + '" name="chkForm" onclick="checkFormEvent(this);" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') + '/>';
                //    },
                //    formatoptions: { disabled: false }
                //},
                { key: false, name: 'formID', index: 'formID', search: false, hidden: true },
                { key: false, name: 'design', index: 'design', search: false },
                { key: false, name: 'description', index: 'description', search: false },
                { key: false, name: 'active', index: 'active', search: false },
                { key: false, name: 'createdDateTime', index: 'createdDateTime', search: false },
                { key: false, name: 'lastUpdatedDateTime', index: 'lastUpdatedDateTime', search: false },
                { key: false, name: 'createdBy', index: 'createdBy', search: false },
                { key: false, name: 'updatedBy', index: 'updatedBy', search: false },
            ],
            pager: jQuery('#pagerFormDesigner'),
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
                        searchInput: $('#txtSearchFormDesignTemplate').val() != null ? $('#txtSearchFormDesignTemplate').val().trim() : null
                    }
                });
            },
            gridComplete: function (e) {
            },
        }).navGrid('#pagerFormDesigner', { edit: false, add: false, del: false, search: false, refresh: false })

        $("#tblFormDesigner").jqGrid('setLabel', 'Select', '', { 'text-align': 'center' });
    }

    var getSelectedTemplate = function () {      
        var gr = $("#tblFormDesigner").getGridParam('selrow');
        if (gr != null) {
            var surrogate = $("#tblFormDesigner").getRowData(gr).formID;
            selectedRow = [];
            selectedRow.push(surrogate);
        }
    }

    return {
        onInit: onInit,
        fireonClick: fireonClick,        
        reloadGridForPositioner: reloadGridForPositioner,
        getSelectedTemplate: getSelectedTemplate,        
        saveDesignTemplate: saveDesignTemplate,
        openDetailPopup: openDetailPopup,        
        openTaskFeedbackScreenLayout: openTaskFeedbackScreenLayout
    }
}());