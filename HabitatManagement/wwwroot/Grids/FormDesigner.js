$(function () {

    var formDesignerColumnNames = [];
    $.ajax({
        type: 'GET',
        cache: false,
        url: "Home/GetFormDesignerColumnNames",
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
});
function formDesignerGrid(formDesignerColumnNames) {
    $("#tblFormDesigner").jqGrid("GridUnload");
    $("#tblFormDesigner").jqGrid({
        url: "Home/GetFormDesignerData",
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
        beforeRequest: function () {
            $(this).jqGrid('setGridParam', {
                postData: {
                    searchInput: $('#txtSearchFormDesignTemplate').val() != null ? $('#txtSearchFormDesignTemplate').val().trim() : null
                }
            });
        },
        gridComplete: function (e) {
            //gridCompleteAgreements(e);
        },
    }).navGrid('#pagerFormDesigner', { edit: false, add: false, del: false, search: false, refresh: false })

    $("#tblFormDesigner").jqGrid('setLabel', 'Select', '', { 'text-align': 'center' });
}