window.ProjectList = (function () {
    var defaults = {
        addEditPopupTitle: "Project",
        addEditURL: '',
        deleteURL: ''
    }

    var selectedRow = [];

    var onInit = function (obj) {
        $.extend(defaults, obj);
        InitiliazeToolbarScrollBar('ProjectFlexi');
        reloadGridForPositioner();
        $("#RefreshProject").bind('click', function () {
            selectedRow = [];
            reloadGridForPositioner();
        });

        $('#AddProject').off('click').on('click', function () {
            fireonClick('add');
        });

        $('#EditProject').off('click').on('click', function () {
            fireonClick('edit', true);
        });

        $('#AddForm').off('click').on('click', function () {
            fireonClick('addForm', true);
        });
    }

    var fireonClick = function (clickFor, selectionRequired) {
        var isValid = true;
        if (selectionRequired) {
            if (selectedRow.length <= 0) {
                ExtendedDialog.openOkBtnDialog({
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
                    ExtendedDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-xl",
                        url: defaults.addEditURL,
                        data: { projectID: 0},
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
                    ExtendedDialog.RenderPageInDialogAndOpen({
                        title: defaults.addEditPopupTitle,
                        modalDialogClass: "modal-xl",
                        url: defaults.addEditURL,
                        data: { projectID: surrogateDate[0] },
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
                case 'addForm':
                    $.ajax({
                        cache: false,
                        async: false,
                        data: { projectId: surrogateDate[0] },
                        url: defaults.addFormURL,
                        success: function (result) {
                            $(".bodyContainer").html(result);  
                        },
                        error: function () {
                        },
                        beforeSend: function () { $("#wait").css("display", "block"); },
                        complete: function () { $("#wait").css("display", "none"); }
                    });		
                    break;
            }
        }
    }

    var saveDesignTemplate = function (currentDialogID) {
        var ajx = $.ajax({
            type: 'POST',
            cache: false,
            url: defaults.addEditURL,
            dataType: 'JSON',
            data: $('#ProjectForm').serialize(),
            success: function (result) {
                if (result.success) {
                    if (result.id != undefined && result.id > 0) {
                        selectedRow = [];
                        selectedRow.push(result.id);
                        $("#ID").val(result.id);
                    }
                    $(currentDialogID).modal("hide");
                    reloadGridForPositioner();                    
                }
                else if (result.Success != undefined && !result.Success) {
                    alert('Unable to save. Please contact administrator.')
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
        var projectListColumnName = [];
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
                            projectListColumnName.push(data.columnNames[i]);
                        }
                    }
                }
            },
            complete: function () {
                projectListGrid(projectListColumnName);
            }
        });
    }

    var projectListGrid = function (projectListColumnName) {
        $("#tblProjectList").jqGrid("GridUnload");
        $("#tblProjectList").jqGrid({
            url: defaults.jqGridDataURL,
            datatype: 'json',
            mtype: 'Post',
            colNames: projectListColumnName,
            colModel: [
                { key: false, name: 'id', index: 'id', search: false, hidden: true },
                { key: false, name: 'project', index: 'project', search: false },
                { key: false, name: 'projectName', index: 'projectName', search: false, width: 300 },
                { key: false, name: 'description', index: 'description', search: false, width: 300 },
                { key: false, name: 'manager', index: 'manager', search: false },
                { key: false, name: 'client', index: 'client', search: false }, 
                { key: false, name: 'siteAddress', index: 'siteAddress', search: false, width: 300},
                { key: false, name: 'sitePostcode', index: 'sitePostcode', search: false},
                { key: false, name: 'creationDate', index: 'creationDate', search: false }
            ],
            pager: jQuery('#pagerProjectList'),
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
                
            },
            gridComplete: function (e) {
            },
        }).navGrid('#pagerProjectList', { edit: false, add: false, del: false, search: false, refresh: false })

        $("#tblProjectList").jqGrid('setLabel', 'Select', '', { 'text-align': 'center' });
    }

    var getSelectedTemplate = function () {
        var gr = $("#tblProjectList").getGridParam('selrow');
        if (gr != null) {
            var id = $("#tblProjectList").getRowData(gr).id;            
            selectedRow = [];
            selectedRow.push(id);
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