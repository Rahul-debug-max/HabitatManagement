window.TaskFeedbackTemplateFields = (function () {
    var defaults = {
        renderForDragnDrop: false,
        templateID: 0
    }
    var onInit = function (obj) {
        $.extend(defaults, obj);

       // if (defaults.renderForDragnDrop) {
            makeDivSortable();
        //}
    }

    var makeDivSortable = function () {
        $('#dvTopSection').sortable({
            items: $("#dvTopSection div.form-group"),
            appendTo: $('#dvTopSection'),
            placeholder: "ui-state-highlight",
            forcePlaceholderSize: true,
            stop: function (event, ui) {
                saveTemplateFieldDetails(ui);
            },
            containment: 'document',
            cursor: "move"
        });

        $(".sortable_list").sortable({
            connectWith: ".connectedSortable",
            containment: 'document',
            placeholder: "ui-state-highlight",   
            forcePlaceholderSize: true ,
            cursor: "move",
            stop: function (event, ui) {
                saveTemplateFieldDetails(ui);
            }
        });
    }

    var saveTemplateFieldDetails = function (ui) {
        var currentSection = $(ui.item).parent().attr('data-section');
        var newSequence = $(ui.item).index() + 1;
        var field = $(ui.item).attr('data-field');
        var data = {
            TemplateID: defaults.templateID,
            Field: field,
            Section: currentSection,
            Sequence: newSequence,
        };
        $('#wait').show();
        $.post(defaults.saveDataURL, data, function (result) {
            if (!result.Success) {
                showAndDismissAlert('danger', wcmVariables.dataSaveErrMsg);
            }
            $('#wait').hide();
        });
    }


    return {
        onInit: onInit,
        makeDivSortable: makeDivSortable
    }
}());