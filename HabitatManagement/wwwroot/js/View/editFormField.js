window.EditFormField = (function () {

    var defaults = {
        tableFieldType: 0
    }
    var columnNames = new Array();

    var onInit = function (obj) {
        $.extend(defaults, obj);

        $('#FieldType').on('change', function () {
            if ($(this).val() == defaults.tableFieldType) {
                $("#tblFieldTypeSection").show();
            }
            else {
                $("#tblFieldTypeSection").hide();
                columnNames = new Array();
                $("#dvTable").hide();
            }
        });

        $('#btnAddTableColumn').on('click', function () {
            addTableColumn();
        });
    }

    var $table = $('<table class=\"tableType\">');
    var addTableColumn = function () {

        if ($('#TableColumn').val() != "") {
            $("#dvTable").show();
            $("#dvTable").find('table th').remove();
            $("#dvTable").find('table tr').remove();
            $("#dvTable").find('table tbody').remove();
            columnNames.push({ 'TableColumn': $('#TableColumn').val(), 'ColumnFieldType': $("#ColumnFieldType").val() });

            if (columnNames.length > 0) {

                $.each(columnNames, function (index, value) {
                    $table.append('<th ColumnFieldType=' + value.ColumnFieldType + '>' + value.TableColumn + '</th>');
                });

                var tableRowCount = $("#TableRowCount").val();
                if (tableRowCount == 0) {
                    tableRowCount = 4;
                    $("#TableRowCount").val(tableRowCount);
                }
                var tbodyString = ''
                for (var i = 0; i < tableRowCount; i++) {
                    tbodyString = tbodyString + '<tr>';
                    $.each(columnNames, function (index, value) {
                        tbodyString = tbodyString + "<td style=\"height:20px;\"></td>";
                    });
                    tbodyString = tbodyString + '</tr>';
                }
                $table.append(tbodyString);
            }
        }
        else {
            alert('Enter Text');
        }
    };
    $table.appendTo('#dvTable');

    return {
        onInit: onInit,
        addTableColumn: addTableColumn
    }
}());