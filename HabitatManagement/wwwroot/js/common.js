//----------------- Common dialogs ----------------

(function () {
    var dialogID = 1;
    var dialogs = {};

    var defaults = {
        // For required dialog
        requiredDialogTitle: '',
        requiredDialogMessage: '',

        // For confirmation dialog
        confirmationDialogTitle: '',
        confirmationDialogMessage: '',
        confirmationDeletionMessage: '',

        // For warning dialog
        warningDialogTitle: '',
        warningDialogMessage: '',

        // Common
        okButtonTitle: '',
        yesButtonTitle: '',
        noButtonTitle: '',
        exitButtonTitle: '',
        saveButtonTitle: '',

        //import Error dialog
        errorDialogTitle: '',
        errorDialogMessage: '',

        //Printer Dialog
        printUrl: '',
        printerValidateUrl: '',
        lblPrint: '',
        lblPrinter: '',
        printErrorMsg: '',

        //confirm exit
        confirmExitMsg: ''
    };
    //private functions
    var getDialog = function (objs) {

        var dialog = {};

        var dialogButtons =
        {
            ExitBtn: {
                text: "",
                id: "btnExit",
                title: "Exit",
                click: function () {

                },
                icons: "glyphicons glyphicons-exit",
                "class": "btn btn-info",
                type: "button"
            },
            SelectBtn: {
                text: "",
                id: "btnSelect",
                title: "Select",
                click: function () {

                },
                icons: "glyphicons glyphicons-check",
                "class": "btn btn-info",
                type: "button"
            },
            SaveBtn: {
                text: "",
                id: "btnSave",
                title: "Save",
                click: function () {

                },
                icons: "glyphicons glyphicons-floppy-disk",
                "class": "btn btn-info",
                type: "button"
            },
            ClearBtn: {
                text: "",
                id: "btnClear",
                /*title: wcmVariables.lblClear,*/
                click: function () {

                },
                icons: "glyphicons glyphicons-delete",
                "class": "btn btn-info",
                type: "button"
            },
            AddBtn: {
                text: "",
                id: "btnAddRow",
                /*title: wcmVariables.lblAdd,*/
                click: function () {

                },
                icons: "glyphicons glyphicons-plus",
                "class": "btn btn-info",
                type: "button"
            },
            NextBtn: {
                text: "",
                id: "btnNext",
                /*title: wcmVariables.lblNext,*/
                click: function () {


                },
                icons: "glyphicons glyphicons-chevron-right",
                "class": "btn btn-info",
                type: "button"
            },
            PrevBtn: {
                text: "",
                id: "btnPrevious",
                /*  title: wcmVariables.lblPrev,*/
                click: function () {

                },
                icons: "glyphicons glyphicons-chevron-left",
                "class": "btn btn-info",
                type: "button"
            },
            CreateBtn: {
                text: "",
                id: "btnCreate",
                title: "Create",
                click: function () {

                },
                "class": "btn btn-primary x1 btn-text",
                type: "button"
            },
            DetailBtn: {
                text: "",
                id: "btnDetail",
                title: "Details",
                click: function () {

                },
                icons: "glyphicons glyphicons-list",
                "class": "btn btn-info",
                type: "button"
            },
            CompleteBtn: {
                text: "",
                id: "btnCompleteRow",
                title: "Complete",
                click: function () {

                },
                icons: "glyphicons glyphicons-flag",
                "class": "btn btn-info",
                type: "button"
            },
        };

        dialog.dialogDefaults = {
            title: '',
            modalDialogClass: '',
            width: Math.min(700, $(window).width() - 50),
            height: ($(window).height() - 20),
            autoOpen: false,
            buttons: [],
            onExit: undefined,
            onOpen: undefined
        };

        $.extend(dialog.dialogDefaults, objs);
        var buttons = [];
        var isExistBtn = false;
        var modalID = 'model-id-' + dialogID;
        $.each(dialog.dialogDefaults.buttons, function (i, value) {
            switch (value.Button) {
                case 'exit':
                    isExistBtn = true;
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.ExitBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.ExitBtn);
                    break;
                case 'save':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.SaveBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.SaveBtn);
                    break;
                case 'add':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.AddBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.AddBtn);
                    break;
                case 'clear':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.ClearBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.ClearBtn);
                    break;
                case 'next':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.NextBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.NextBtn);
                    break;
                case 'prev':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.PrevBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.PrevBtn);
                    break;
                case 'create':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.CreateBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.CreateBtn);
                    break;
                case 'replace':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.ReplaceBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.ReplaceBtn);
                    break;
                case 'taskComplete':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.CompleteWOBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.CompleteWOBtn);
                    break;
                case 'singleLabelPrint':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.SingleLabelPrintBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.SingleLabelPrintBtn);
                    break;
                case 'multipleLabelPrint':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.MultipleLabelPrintBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.MultipleLabelPrintBtn);
                    break;
                case 'print':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.PrintBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.PrintBtn);
                    break;
                case 'select':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.SelectBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.SelectBtn);
                    break;
                case 'details':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.DetailBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.DetailBtn);
                case 'complete':
                    if (typeof (value.onClick) == 'function') {
                        dialogButtons.CompleteBtn.click = value.onClick;
                    }
                    buttons.push(dialogButtons.CompleteBtn);
                    break;
                    break;
            }
        });

        if (!isExistBtn) {
            dialogButtons.ExitBtn.click = function () {
                dialog.Close();
            }
            buttons.push(dialogButtons.ExitBtn);
        }
        var divDetail = "<div class='modal fade' role='dialog' data-backdrop = 'static' id='" + modalID + "'><div class='modal-dialog'><div class='modal-content'>" +
            "<div class='modal-header'><h4 class='modal-title'></h4><button type='button' class='close'>&times;</button></div>" +
            "<div class='modal-body'></div><div class='modal-footer'></div></div></div></div>";

        dialog.Div = $(divDetail);
        var buttonHtml = "";
        if (buttons != undefined && buttons.length > 0) {
            for (var i = 0; i < buttons.length; i++) {
                buttonHtml += "<button type='button' class='btn " + (buttons[i].class || "") + "' aria-modalID = '" + modalID + "' name='btn" + modalID + "' id = '" + buttons[i].id + dialogID + "' title = '" + buttons[i].title + "'>" + (buttons[i].text || "<span class='" + buttons[i].icons + "'></span>") + "</button>";
            }
        }        
        dialog.Div.find('.modal-footer').html(buttonHtml);
        dialog.Div.find('.modal-dialog').addClass(dialog.dialogDefaults.modalDialogClass);
        dialog.Div.find('.modal-title').text(dialog.dialogDefaults.title);
        dialog.Div.find('.modal-header').find('.close').on("click", function () {
            dialog.Close();
        });

        document.body.append(dialog.Div[0]);
        var btns = $("button[name='btn" + modalID + "']");
        for (var i = 0; i < btns.length; i++) {
            $("#" + btns[i].id).on("click", buttons[i].click);
        }

        dialog.Open = function () {
            dialog.Div.modal();
            dialog.Div.on('hidden.bs.modal', function () {
                $(this).remove();
            });
            ++dialogID;
        }
        dialog.Close = function () {
            dialog.Div.modal("hide");
            --dialogID;
        }
        return dialog;
    }
    //end private functions

    //call this function to open pop up dialog to populate any view
    dialogs.RenderPageInDialogAndOpen = function (obj) {
        $("#wait").css("display", "block");
        var defaults = {
            title: '',
            modalDialogClass: '',
            width: Math.min(900, $(window).width() - 50),
            height: ($(window).height() - 20),
            traditional: false,
            position: { my: "top", at: "top+5", of: window },
            url: '',
            data: undefined,
            onExit: undefined,
            onComplete: undefined,
            onSuccess: undefined,
            onError: undefined,
            onOpen: undefined,
            buttons: undefined,
            html: undefined,
            userDefinedSuccess: undefined
        };

        $.extend(defaults, obj);
        var dialog = getDialog(defaults);
        if (defaults.url != undefined && defaults.url != '') {
            $.ajax({
                url: defaults.url,
                type: 'get',
                data: defaults.data,
                traditional: defaults.traditional,
                cache: false,
                success: function (result) {
                    if (defaults.userDefinedSuccess != undefined && typeof (defaults.userDefinedSuccess) == "function") {
                        defaults.userDefinedSuccess(dialog, result);
                    } else {
                        dialog.Div.find('.modal-body').html(result);
                        dialog.Open();
                    }
                    if (defaults.onSuccess != undefined && typeof (defaults.onSuccess) == "function")
                        defaults.onSuccess();
                },
                beforesend: function () { $("#wait").css("display", "block"); },
                onError: function () {
                    if (defaults.onError != undefined && typeof (defaults.onError) == "function")
                        defaults.onError();
                },
                complete: function () {
                    $("#wait").css("display", "none");
                    if (defaults.onComplete != undefined && typeof (defaults.onComplete) == "function")
                        defaults.onComplete();
                }
            });
        }
        else if (defaults.html != undefined && defaults.html != '') {
            dialog.Div.empty().html($.parseHTML(defaults.html));
            dialog.Open();
        }
    }

    // Use this function for open any confirmation dialog that perform AJAX call on ok.
    dialogs.openConfirmationDialogWithAJAX = function (obj) {

        var dialog = {};

        var confirmationData = {
            url: '',
            type: 'POST',
            timeout: 0,
            formData: {},
            cache: false,
            traditional: false,
            onSuccess: undefined,
            onError: undefined,
            onFail: undefined,
            confirmationDialogTitle: 'Confirm',
            confirmationDeletionMessage: 'Confirm deletion',
            yesButtonTitle: 'Yes',
            noButtonTitle: 'No'
        }

        $.extend(confirmationData, obj);

        var _confirmationDialogMessage = (confirmationData.confirmationDeletionMessage == '' || confirmationData.confirmationDeletionMessage == undefined) ?
            defaults.confirmationDeletionMessage : confirmationData.confirmationDeletionMessage;

        var _confirmationDialogTitle = (confirmationData.confirmationDialogTitle == '' || confirmationData.confirmationDialogTitle == undefined) ?
            defaults.confirmationDialogTitle : confirmationData.confirmationDialogTitle;

        var _yesButtonTitle = (confirmationData.yesButtonTitle == '' || confirmationData.okButtonTitle == undefined) ?
            defaults.yesButtonTitle : confirmationData.yesButtonTitle;

        var _noButtonTitle = (confirmationData.noButtonTitle == '' || confirmationData.noButtonTitle == undefined) ?
            defaults.noButtonTitle : confirmationData.noButtonTitle;


        var dialogButtons =
        {

            SelectBtn: {
                text: "",
                id: "btnSelect",
                title: _yesButtonTitle,
                click: function () {
                    dialog.Close();

                    $.ajax({
                        type: confirmationData.type,
                        cache: confirmationData.cache,
                        data: confirmationData.formData,
                        url: confirmationData.url,
                        traditional: confirmationData.traditional,
                        success: function (result) {
                            if (confirmationData.onSuccess != undefined) {
                                confirmationData.onSuccess(result);
                            }
                        },
                        error: function (result) {
                            if (confirmationData.onError != undefined) {
                                confirmationData.onError(result);
                            }
                        },
                        fail: function (result) {
                            if (confirmationData.onFail != undefined) {
                                confirmationData.onFail(result);
                            }
                        },
                        beforeSend: function () { $("#wait").css("display", "block"); },
                        complete: function () { $("#wait").css("display", "none"); }
                    });

                    if (confirmationData.onSelect != undefined) {
                        confirmationData.onSelect();
                    }
                },
                icons: "glyphicons glyphicons-check",
                "class": "btn btn-info",
                type: "button"
            },
            CloseBtn: {
                text: "",
                id: "btnClose",
                title: _noButtonTitle,
                click: function () {
                    dialog.Close();
                },
                icons: "glyphicons glyphicons-remove",
                "class": "btn btn-danger",
                type: "button"
            }
        };

        var buttons = [];
        var modalID = 'model-id-' + dialogID;
        $.each(dialogButtons, function (i) {
            buttons.push(dialogButtons[i]);
        });
        var divDetail = "<div class='modal fade' role='dialog' data-backdrop = 'static' id='" + modalID + "'><div class='modal-dialog'><div class='modal-content'>" +
            "<div class='modal-header'><h4 class='modal-title'>" + _confirmationDialogTitle + "</h4><button type='button' class='close'>&times;</button></div>" +
            "<div class='modal-body'>" + _confirmationDialogMessage + "</div><div class='modal-footer'></div></div></div></div>";

        dialog.Div = $(divDetail);
        var buttonHtml = "";
        if (buttons != undefined && buttons.length > 0) {
            for (var i = 0; i < buttons.length; i++) {
                buttonHtml += "<button type='button' class='btn " + (buttons[i].class || "") + "' aria-modalID = '" + modalID + "' name='btn" + modalID + "' id = '" + buttons[i].id + dialogID + "' title = '" + buttons[i].title + "'>" + (buttons[i].text || "<span class='" + buttons[i].icons + "'></span>") + "</button>";
            }
        }
        dialog.Div.find('.modal-footer').html(buttonHtml);

        dialog.Div.find('.modal-header').find('.close').on("click", function () {
            dialog.Close();
        });

        document.body.append(dialog.Div[0]);
        var btns = $("button[name='btn" + modalID + "']");
        for (var i = 0; i < btns.length; i++) {
            $("#" + btns[i].id).on("click", buttons[i].click);
        }

        dialog.Open = function () {
            dialog.Div.modal();
            dialog.Div.on('hidden.bs.modal', function () {
                $(this).remove();
            });
            ++dialogID;
        }
        dialog.Close = function () {
            dialog.Div.modal("hide");
            --dialogID;
        }
        dialog.Open();
    }

    // Use this function for required or no record found dialog. 
    dialogs.openOkBtnDialog = function (obj) {
        var dialog = {};

        var dialogInits = {
            requiredDialogTitle: '',
            requiredDialogMessage: '',
            okButtonTitle: 'Ok',
            onClose: undefined,
        }

        $.extend(dialogInits, obj);

        var _requiredDialogMessage = (dialogInits.requiredDialogMessage == '' || dialogInits.requiredDialogMessage == undefined) ?
            defaults.requiredDialogMessage : dialogInits.requiredDialogMessage;

        var _requiredDialogTitle = (dialogInits.requiredDialogTitle == '' || dialogInits.requiredDialogTitle == undefined) ?
            defaults.requiredDialogTitle : dialogInits.requiredDialogTitle;

        var _okButtonTitle = (dialogInits.okButtonTitle == '' || dialogInits.okButtonTitle == undefined) ?
            defaults.okButtonTitle : dialogInits.okButtonTitle;

        var buttons = [{
            text: "",
            id: "btnSelect",
            title: _okButtonTitle,
            click: function () {
                dialog.Close();
            },
            icons: "glyphicons glyphicons-ok",
            "class": "btn btn-primary",
            type: "button"
        }];
      
        var modalID = 'model-id-' + dialogID;
        var divDetail = "<div class='modal fade' role='dialog' data-backdrop = 'static' id='" + modalID + "'><div class='modal-dialog'><div class='modal-content'>" +
            "<div class='modal-header'><h4 class='modal-title'>" + _requiredDialogTitle + "</h4><button type='button' class='close'>&times;</button></div>" +
            "<div class='modal-body'>" + _requiredDialogMessage + "</div><div class='modal-footer'></div></div></div></div>";

        dialog.Div = $(divDetail);
        var buttonHtml = "";
        if (buttons != undefined && buttons.length > 0) {
            for (var i = 0; i < buttons.length; i++) {
                buttonHtml += "<button type='button' class='btn " + (buttons[i].class || "") + "' aria-modalID = '" + modalID + "' name='btn" + modalID + "' id = '" + buttons[i].id + dialogID + "' title = '" + buttons[i].title + "'>" + (buttons[i].text || "<span class='" + buttons[i].icons + "'></span>") + "</button>";
            }
        }
        dialog.Div.find('.modal-footer').html(buttonHtml);

        dialog.Div.find('.modal-header').find('.close').on("click", function () {
            dialog.Close();
        });

        document.body.append(dialog.Div[0]);
        var btns = $("button[name='btn" + modalID + "']");
        for (var i = 0; i < btns.length; i++) {
            $("#" + btns[i].id).on("click", buttons[i].click);
        }

        dialog.Open = function () {
            dialog.Div.modal();
            dialog.Div.on('hidden.bs.modal', function () {
                $(this).remove();
            });
            ++dialogID;
        }
        dialog.Close = function () {
            dialog.Div.modal("hide");
            --dialogID;
        }
        dialog.Open();
    }

    //call this function on exit of page
    dialogs.confirmExit = function (obj) {
        var isFormDirty = false;

        var confirmationData = {
            onYesSelect: undefined,
            onNoSelect: undefined,
            onClose: undefined,
            onExit: undefined,
            onSave: undefined,
            popupWidth: 0,
            formId: '',
        }

        $.extend(confirmationData, obj);

        if (confirmationData.formId != '') {
            $(confirmationData.formId).find('input[type="text"],textarea,input[type="checkbox"],input[type="radio"]').on('change',
                function () {
                    isFormDirty = true;
                });

            $(confirmationData.formId).find('.dateTextBox').on('iDateSelected change',
                function () {
                    isFormDirty = true;
                });

            $(confirmationData.formId).find('.dateTextBox').next().next().on('change', function () {
                isFormDirty = true;
            });

            $('.ac_input').on('ac_change', function () { isFormDirty = true; });
        }

        var _confirmationDialogMessage = defaults.confirmExitMsg;

        var _confirmationDialogTitle = wcmVariables.lblConfirm;

        var _yesButtonTitle = defaults.yesButtonTitle;

        var _noButtonTitle = defaults.noButtonTitle;

        var overlayChange = $('.ui-widget-overlay').not('.jqgrid-overlay').length;

        var selectionDiv = $("<div></div>");

        if ($(selectionDiv).dialog('instance') != undefined && $(selectionDiv).dialog('instance').element != undefined) {
            $(selectionDiv).dialog('destroy');
        }

        var dialog = function () {
            $(selectionDiv).text(_confirmationDialogMessage);

            selectionDiv.dialog({
                title: _confirmationDialogTitle,
                modal: true,
                autoOpen: false,
                width: confirmationData.popupWidth != 0 ? confirmationData.popupWidth : Math.min($(window).width() - 30, 300),
                buttons: [
                    {
                        text: "",
                        id: "btnClose",
                        title: _noButtonTitle,
                        click: function () {
                            $(this).dialog("close");
                            if (confirmationData.onExit != undefined) {
                                confirmationData.onExit();
                            }
                            if (confirmationData.onNoSelect != undefined) {
                                confirmationData.onNoSelect();
                            }
                        },
                        icons: { primary: "glyphicons glyphicons-remove" },
                        "class": "btn btn-danger dialog-btn-x1",
                        type: "button"
                    },
                    {
                        text: "",
                        id: "btnSelect",
                        title: _yesButtonTitle,
                        click: function () {
                            $(this).dialog("close");
                            if (confirmationData.onYesSelect != undefined) {
                                confirmationData.onYesSelect();
                            }

                            if (confirmationData.onSave != undefined) {
                                $.when(confirmationData.onSave()).done(function () {
                                    isFormDirty = false;
                                    confirmationData.onExit();
                                }).fail(function () {

                                });
                            }

                        },
                        icons: { primary: "glyphicons glyphicons-tick" },
                        "class": "btn btn-success dialog-btn-x1",
                        type: "button"
                    },
                ],
                create: function () {
                    // Remove default css from jQuery dialog.
                    $('.ui-dialog-buttonset').children('button').removeClass("ui-button ui-widget ui-state-default ui-corner-all ui-button-text-icon-primary").
                        mouseover(function () { $(this).removeClass('ui-state-hover'); }).
                        mousedown(function () { $(this).removeClass('ui-state-active'); }).
                        focus(function () { $(this).removeClass('ui-state-focus'); });
                    $('.ui-dialog-buttonset button').children('span').removeClass("ui-button-icon-primary ui-icon ui-button-text");
                },
                open: function () {
                    // Remove default button focus on opening.
                    $('.ui-dialog-buttonset').children('button').blur();
                    $('.ui-widget-overlay').not('.jqgrid-overlay').last().css("z-index", parseFloat(100) + overlayChange);
                },
                close: function () {
                    if (confirmationData.onClose != undefined) {
                        confirmationData.onClose();
                    }
                    $(this).empty();
                    $(this).dialog('destroy');
                }
            });
        }

        var setFormDirtyFlag = function (flag) {
            isFormDirty = flag;
        }

        var call = function () {
            if (isFormDirty) {
                dialog();
                $(selectionDiv).dialog('open');
                adjustDialog();
            } else {
                confirmationData.onExit();
            }
        }

        var checkFormDirty = function () {
            return isFormDirty;
        }

        return {
            setFormDirtyFlag: setFormDirtyFlag,
            call: call,
            checkFormDirty: checkFormDirty,
        }

    }

    dialogs.default = defaults;

    // Reuseable dialog object
    window.WCMDialog = dialogs;

}());