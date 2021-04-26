window.FormTemplateFields = (function () {
    var defaults = {
        renderForDragnDrop: false,
        formId: 0
    }
    var onInit = function (obj) {
        $.extend(defaults, obj);
        makeDivSortable();
    }

    var makeDivSortable = function () {

        $(".sortable_list").sortable({
            items: $(".dvFormFeedbackData div.form-group"),
            connectWith: ".connectedSortable",
            containment: 'document',
            placeholder: "ui-state-highlight",   
            forcePlaceholderSize: true,
            cursor: "move",
            stop: function (event, ui) {
                saveFormFieldDetails(ui);
            }
        });
    }

    var saveFormFieldDetails = function (ui) {
        var currentSection = $(ui.item).parent().attr('data-section');
        var newSequence = $(ui.item).index() + 1;
        var field = $(ui.item).attr('data-field');
        var data = {
            FormID: defaults.formId,
            Field: field,
            Section: currentSection,
            Sequence: newSequence,
        };

        $.post(defaults.saveDataURL, data, function (result) {
            if (!result.success) {
                alert('Unable to save. Please contact administrator.')
            }
        });
    }      

    return {
        onInit: onInit,
        makeDivSortable: makeDivSortable
    }
}());