window.EditFormField = (function () {

    var defaults = {
        tableFieldType:0
    }

    var onInit = function (obj) {
        $.extend(defaults, obj);

        $('#FieldType').on('change', function () {            
            if ($(this).val() == defaults.tableFieldType) {
                $("#tblFieldTypeSection").show();
            }
            else {
                $("#tblFieldTypeSection").hide();
            }
        });

        $('#btnAddTableColumn').on('click', function () {

        });
    }


    var addTableColumn = function () {
        alert('added');
    }

    return {
        onInit: onInit,
        addTableColumn: addTableColumn
    }
}());