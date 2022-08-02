define(["Utilities/MessageBox/MessageBoxDefaults"], function (MessageBoxDefaults) {

    /*
        Bağımlılık Listesi
        -------------------------------
        1) https://github.com/makeusabrew/bootbox
    
    */


    var showMessageBox = function (options) {
        bootbox.setLocale('tr');

        var opts = $.extend({}, MessageBoxDefaults, options);

        if (opts.MessageIcon == '') {
            switch (opts.MessageType) {
                case 'info': opts.MessageIcon = 'fa fa-info-circle'; break;
                case 'warning': opts.MessageIcon = 'fa fa-exclamation-circle'; break;
                case 'danger': opts.MessageIcon = 'fa fa-times-circle'; break;
                case 'success': opts.MessageIcon = 'fa fa-check-circle'; break;
                case 'prompt': opts.MessageIcon = 'fa fa-question-circle'; break;
                default: opts.MessageIcon = 'fa fa-comment'; break;
            }
        }
        switch (opts.MessageType) {
            case 'info': opts.OkButtonClass = 'primary'; opts.YesButtonClass = 'primary'; break;
            case 'warning': opts.OkButtonClass = 'warning'; opts.YesButtonClass = 'warning'; break;
            case 'danger': opts.OkButtonClass = 'danger'; opts.YesButtonClass = 'danger'; break;
            case 'success': opts.OkButtonClass = 'success'; YesButtonClass = 'success'; break;
            case 'prompt': opts.OkButtonClass = 'primary'; opts.CancelButtonClass = 'default'; break;
            default: opts.OkButtonClass = 'default'; opts.YesButtonClass = 'success'; opts.NoButtonClass = 'danger'; opts.CancelButtonClass = 'warning'; break;
        }


        var message = '<div id="alert-id" class = "alert alert-' + opts.MessageType + '" style = "margin-bottom:0px !important">';
        message += '<div class="row">';
        if (opts.MessageIcon.length > 0) {
            message += '<div id="icon" class="col-sm-4 col-xs-6 vcenter"><span class="' + opts.MessageIcon + ' fa-' + opts.MessageIconSize + 'x"></span></div>';
            message += '<div id="content" class="col-sm-20 col-xs-18 vcenter">' + opts.MessageText + '</div>';
        } else {
            message += '<div id="content" class="col-xs-24">' + opts.MessageText + '</div>';
        }
        message += '</div>'; /* Row */
        message += '</div>'; /* Alert */

        opts.MessageText = message;

        var buttons = {};

        switch (opts.Type) {
            case 'YESNO':
                buttons.YESButton = {
                    label: (opts.YesButtonIcon == "") ? opts.YesButtonText : '<i class = "' + opts.YesButtonIcon + '">&nbsp;</i>' + opts.YesButtonText,
                    className: 'btn btn-' + opts.YesButtonClass,
                    callback: function () {
                        if (opts.YesButtonFunction != "" && opts.YesButtonFunction != null) {
                            var fn = window[opts.YesButtonFunction](opts.YesButtonFunctionParams);
                            if (typeof fn == 'function') {
                                fn();
                            }
                        }
                    }
                };
                buttons.NOButton = {
                    label: (opts.NoButtonIcon == "") ? opts.NoButtonText : '<i class = "' + opts.NoButtonIcon + '">&nbsp;</i>' + opts.NoButtonText,
                    className: 'btn btn-' + opts.NoButtonClass,
                    callback: function () {
                        if (opts.NoButtonFunction != "" && opts.NoButtonFunction != null) {
                            var fn = window[opts.NoButtonFunction](opts.NoButtonFunctionParams);
                            if (typeof fn == 'function') {
                                fn();
                            }
                        }
                    }
                };
                break;
            case 'YESNOCANCEL':
                buttons.YESButton = {
                    label: (opts.YesButtonIcon == "") ? opts.YesButtonText : '<i class = "' + opts.YesButtonIcon + '">&nbsp;</i>' + opts.YesButtonText,
                    className: 'btn btn-' + opts.YesButtonClass,
                    callback: function () {
                        if (opts.YesButtonFunction != "" && opts.YesButtonFunction != null) {
                            var fn = window[opts.YesButtonFunction](opts.YesButtonFunctionParams);
                            if (typeof fn == 'function') {
                                fn();
                            }
                        }
                    }
                };
                buttons.NOButton = {
                    label: (opts.NoButtonIcon == "") ? opts.NoButtonText : '<i class = "' + opts.NoButtonIcon + '">&nbsp;</i>' + opts.NoButtonText,
                    className: 'btn btn-' + opts.NoButtonClass,
                    callback: function () {
                        if (opts.NoButtonFunction != "" && opts.NoButtonFunction != null) {
                            var fn = window[opts.NoButtonFunction](opts.NoButtonFunctionParams);
                            if (typeof fn == 'function') {
                                fn();
                            }
                        }
                    }
                };
                buttons.CANCELButton = {
                    label: (opts.CancelButtonIcon == "") ? opts.CancelButtonText : '<i class = "' + opts.CancelButtonIcon + '">&nbsp;</i>' + opts.CancelButtonText,
                    className: 'btn btn-' + opts.CancelButtonClass,
                    callback: function () {
                        if (opts.CancelButtonFunction != "" && opts.CancelButtonFunction != null) {
                            var fn = window[opts.CancelButtonFunction](opts.CancelButtonFunctionParams);
                            if (typeof fn == 'function') {
                                fn();
                            }
                        }
                    }
                };
                break;
            case 'PROMPT':
                buttons.OKButton = {
                    label: (opts.OkButtonIcon == "") ? opts.OkButtonText : '<i class = "' + opts.OkButtonIcon + '">&nbsp;</i>' + opts.OkButtonText,
                    className: 'btn btn-' + opts.OkButtonClass,
                    callback: function () {
                        if (opts.OkButtonFunction != "" && opts.OkButtonFunction != null) {
                            var fn = window[opts.OkButtonFunction](opts.OkButtonFunctionParams);
                            if (typeof fn == 'function') {
                                fn();
                            }
                        }
                    }
                };
                buttons.CANCELButton = {
                    label: (opts.CancelButtonIcon == "") ? opts.CancelButtonText : '<i class = "' + opts.CancelButtonIcon + '">&nbsp;</i>' + opts.CancelButtonText,
                    className: 'btn btn-' + opts.CancelButtonClass,
                    callback: function () {
                        if (opts.CancelButtonFunction != "" && opts.CancelButtonFunction != null) {
                            var fn = window[opts.CancelButtonFunction](opts.CancelButtonFunctionParams);
                            if (typeof fn == 'function') {
                                fn();
                            }
                        }
                    }
                };
                break;
            default:
                buttons.OKButton = {
                    label: (opts.OkButtonIcon == "") ? opts.OkButtonText : '<i class = "' + opts.OkButtonIcon + '">&nbsp;</i>' + opts.OkButtonText,
                    className: 'btn btn-' + opts.OkButtonClass,
                    callback: function () {
                        if (opts.OkButtonFunction != "" && opts.OkButtonFunction != null) {
                            var fn = window[opts.OkButtonFunction](opts.OkButtonFunctionParams);
                            if (typeof fn == 'function') {
                                fn();
                            }
                        }
                    }
                };
                break;
        }

        if (opts.MessageType == 'prompt') {
            bootbox.prompt({
                title: opts.Title,
                inputType: opts.InputType,
                inputOptions: opts.InputOptions,
                callback: function (result) {
                    if (opts.Callback != "" && opts.Callback != null) {
                        var fn = window[opts.Callback](result);
                        if (typeof fn == 'function') {
                            fn();
                        }
                    }
                }
            });
        }
        else {
            bootbox.dialog({
                title: opts.Title,
                message: opts.MessageText,
                closeButton: opts.CloseButton,
                size: opts.Size,
                buttons: buttons
            });
        }
    };

    return {
        Show: showMessageBox
    };
});