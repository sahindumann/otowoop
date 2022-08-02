define(["Utilities/JSPanel/JSPanelDefaults", "Utilities/Util"], function (JSPanelDefaults, Util) {
    /*
        Bağımlılıklar
        -------------------
        1 - M Custom Scroll Bar (https://github.com/malihu/malihu-custom-scrollbar-plugin)
    
    */
    var ShowJSPanel = function (options) {



        var opts = $.extend({}, JSPanelDefaults, options);

        if ($("#" + opts.Id).length < 1) {

            opts.HeaderTitle = opts.HeaderTitle.toLocaleUpperCase();
            if (opts.HeaderTitle == 'HAT SORGULA' || opts.HeaderTitle == 'GÜZERGAH SORGULA')
            {
        
                opts.ContentSize='auto ' + (window.innerHeight-200)+"px"

            }

            $.jsPanel({
                autoclose: opts.Autoclose,
                border: opts.Border,
                callback: opts.Callback,
                container: opts.Container,
                content: opts.Content,
                contentAjax: opts.ContentAjax,
                contentIframe: opts.ContentIframe,
                contentOverflow: opts.ContentOverflow,
                contentSize: opts.ContentSize,
                custom: opts.Custom,
                dblclicks: opts.Dblclicks,
                delayClose: opts.DelayClose,
                draggable: {
                    handle: opts.Draggable.Handle,
                    opacity: opts.Draggable.Opacity
                },
                dragit: {
                    axis: opts.Dragit.Axis,
                    containment: opts.Dragit.Containment,
                    handles: opts.Dragit.Handles,
                    opacity: opts.Dragit.Opacity,
                    start: opts.Dragit.Start,
                    drag: opts.Dragit.Drag,
                    stop: opts.Dragit.Stop,
                    disableui: opts.Dragit.Disableui
                },
                footerToolbar: opts.FooterToolbar,
                headerControls: {
                    close: opts.HeaderControls.Close,
                    maximize: opts.HeaderControls.Maximize,
                    minimize: opts.HeaderControls.Minimize,
                    normalize: opts.HeaderControls.Normalize,
                    smallify: opts.HeaderControls.Smallify,
                    controls: opts.HeaderControls.Controls,
                    iconfont: opts.HeaderControls.Iconfont
                },
                headerRemove: opts.HeaderRemove,
                headerTitle: opts.HeaderTitle,
                headerToolbar: opts.HeaderToolbar,
                id: opts.Id,
                maximizedMargin: {
                    top: opts.MaximizedMargin.Top,
                    right: opts.MaximizedMargin.Right,
                    bottom: opts.MaximizedMargin.Lottom,
                    left: opts.MaximizedMargin.Left
                },
                minimizeTo: opts.MinimizeTo,
                onbeforeclose: opts.OnBeforeClose,
                onbeforemaximize: opts.OnBeforeMaximize,
                onbeforeminimize: opts.OnBeforeMinimize,
                onbeforenormalize: opts.OnBeforeNormalize,
                onbeforesmallify: opts.OnBeforeSmallify,
                onbeforeunsmallify: opts.onBeforeUnsmallify,
                onclosed: opts.OnClosed,
                onmaximized: opts.OnMaximized,
                onminimized: opts.OnMinimized,
                onnormalized: opts.OnNormalized,
                onbeforeresize: opts.OnBeforeResize,
                onresized: opts.OnResized,
                onsmallified: opts.OnSmallified,
                onunsmallified: opts.OnUnsmallified,
                onfronted: opts.OnFronted,
                onwindowresize: opts.OnWindowResize,
                paneltype: opts.PanelType,
                position: opts.Position,
                resizable: {
                    handles: opts.Resizable.Handles,
                    autoHide: opts.Resizable.AutoHide,
                    minWidth: opts.Resizable.MinWidth,
                    minHeight: opts.Resizable.MinHeight
                },
                resizeit: {
                    containment: opts.ResizeIt.Containment,
                    handles: opts.ResizeIt.Handles,
                    minWidth: opts.ResizeIt.MinWidth,
                    minHeight: opts.ResizeIt.MinHeight,
                    maxWidth: opts.ResizeIt.MaxWidth,
                    maxHeight: opts.ResizeIt.MaxHeight,
                    start: opts.ResizeIt.Start,
                    resize: opts.ResizeIt.Resize,
                    stop: opts.ResizeIt.Stop,
                    disableui: opts.ResizeIt.Disableui
                },
                rtl: opts.Rtl,
                setstatus: opts.SetStatus,
                show: opts.Show,
                template: opts.Template,
                theme: opts.Theme,

            });
        } else {
            if ($(window).width() < 768) {
                jsPanel.activePanels.getPanel(opts.Id).maximize();
            }
            else {
                jsPanel.activePanels.getPanel(opts.Id).normalize();
            }
        }

        jsPanel.activePanels.getPanel(opts.Id).content.mCustomScrollbar({ theme: 'minimal-dark', scrollButtons: { enable: true } });
        if (opts.Comment != undefined || opts.Comment) {

            $("#" + opts.Id + " .jsPanel-controlbar #commentClick").remove();
            $("#" + opts.Id + " .jsPanel-controlbar").prepend("<span id='commentClick' style='color:white;margin-left:8px;cursor:pointer' class='fa fa-comment-o'></span>");

            $("#commentClick").on("click", function () {


                GetCommentCurrent(opts.TableName, opts.TableID);

            });
        }

        if (opts.QuerySave != undefined && opts.QuerySave == true) {
            $("#" + opts.Id + " .jsPanel-controlbar").prepend("<span id='querySaveClick' style='color:white;margin-left:8px;cursor:pointer' class='fa fa-save'></span>");

            $("#querySaveClick").on("click", function () {

                SaveQuery();

            });
        }
        if (opts.QuerySaveStop != undefined && opts.QuerySaveStop == true) {
            $("#" + opts.Id + " .jsPanel-controlbar").prepend("<span id='saveQueryStop' style='color:white;margin-left:8px;cursor:pointer' class='fa fa-save'></span>");

            $("#saveQueryStop").on("click", function () {

                SaveQueryStop();

            });
        }

        if (opts.QuerySaveLine != undefined && opts.QuerySaveLine == true) {
            $("#" + opts.Id + " .jsPanel-controlbar").prepend("<span id='saveQueryLine' style='color:white;margin-left:8px;cursor:pointer' class='fa fa-save'></span>");

            $("#saveQueryLine").on("click", function () {

                SaveQueryLine();

            });
        }

        if (opts.QuerySaveRoute != undefined && opts.QuerySaveRoute == true) {
            $("#" + opts.Id + " .jsPanel-controlbar").prepend("<span id='saveQueryRoute' style='color:white;margin-left:8px;cursor:pointer' class='fa fa-save'></span>");

            $("#saveQueryRoute").on("click", function () {

                SaveQueryRoute();

            });
        }
        if (opts.QuerySaveGarage != undefined && opts.QuerySaveGarage == true) {
            $("#" + opts.Id + " .jsPanel-controlbar").prepend("<span id='saveQueryGarage' style='color:white;margin-left:8px;cursor:pointer' class='fa fa-save'></span>");

            $("#saveQueryGarage").on("click", function () {

                SaveQueryGarage();

            });
        }
        if (opts.QuerySaveStation != undefined && opts.QuerySaveStation == true) {
            $("#" + opts.Id + " .jsPanel-controlbar").prepend("<span id='saveQueryStation' style='color:white;margin-left:8px;cursor:pointer' class='fa fa-save'></span>");

            $("#saveQueryStation").on("click", function () {

                SaveQueryStation();

            });
        }
        if (opts.QuerySavePlatform != undefined && opts.QuerySavePlatform == true) {
            $("#" + opts.Id + " .jsPanel-controlbar").prepend("<span id='saveQueryPlatform' style='color:white;margin-left:8px;cursor:pointer' class='fa fa-save'></span>");

            $("#saveQueryPlatform").on("click", function () {

                SaveQueryPlatform();

            });
        }
        //$('#dynamic-query-builder input').keypress(function () {
        //    $(this).val($(this).val().toLocaleUpperCase());
        //});
        $('input[type=text]').keyup(function () {
            // $(this).val($(this).val().buyukHarf());
            $(this).val($(this).val().toLocaleUpperCase());
        });
        $("textarea").attr('maxlength', '254');

        $('textarea').keyup(function () {
            $(this).val($(this).val().toLocaleUpperCase());
        });
        //Sadece numara girme decimal


        $('.number').keypress(function (eve) {


            if ((eve.keyCode == 44 || $(this).val().indexOf(",") >= 0) && $(this).val().length >= 10) {

                if ($(this).val().split(',').length > 1) {
                    var len_first = $(this).val().split(',')[0];
                    var len_last = $(this).val().split(',')[1];
                    if (len_first.length > 10 || len_last.length > 7) {
                        eve.preventDefault();
                    }
                }
            }
            else {
                if (eve.keyCode == 46) {

                    if ($(this).val().length > 11) {
                        eve.preventDefault();
                    }

                }
                else {
                    if ($(this).val().length > 9) {
                        eve.preventDefault();
                    }
                }


            }
            if ($(this).val() + "".indexOf(".") >= 0 && eve.keyCode == 190) {

                eve.preventDefault();
            }

            if ((eve.which != 44 || $(this).val().indexOf(',') != -1) && (eve.which < 48 || eve.which > 57) || (eve.which == 46)) {
                eve.preventDefault();
            }

            $('.number').keyup(function (eve) {

                if ($(this).val().indexOf(",") >= 0 && eve.keyCode == 44) {

                    eve.preventDefault();
                }


                if ($(this).val().indexOf(',') == 0) {
                    $(this).val($(this).val().substring(1));
                }
            });
        });
        //Sadece numara girme integer
        $('.integer').keypress(function (eve) {
            if ($(this).val().length > 8) {
                eve.preventDefault();
            }

            else {
                if (eve.keyCode == 44 || eve.keyCode == 46) {

                    eve.preventDefault();
                }

                if ((eve.which != 46 || $(this).val().indexOf('.') != -1) && (eve.which < 48 || eve.which > 57) || (eve.which == 46)) {
                    eve.preventDefault();
                }
            }
            $('.integer').keyup(function (eve) {
                if ($(this).val().length > 8) {
                    eve.preventDefault();
                }
                else {
                    if (eve.keyCode == 44 || eve.keyCode == 46) {

                        eve.preventDefault();
                    }


                    if ($(this).val().indexOf('.') == 0) {
                        $(this).val($(this).val().substring(1));
                    }
                }
            });
        });

        $('.number , .integer').on('paste', function () {
            var element = this;
            setTimeout(function () {
                var text = $(element).val();
                //11111111,1

                var a = parseInt(text, 0);
                $(element).val($(element).val().replace(".", ","));
                if (isNaN(a)) {
                    $(element).val('');
                }
                else {
                    if (element.value.indexOf(",") >= 0 && element.value.split(',').length > 1) {
                        var len_first = element.value.split(',')[0];
                        var len_last = element.value.split(',')[1];
                        if (len_first.length > 10 || len_last.length > 7) {
                            $(element).val('');
                        }

                    }
                }

            }, 100);
        });
        //This block resolves bootstrap table's header and column adjustment issue when panel resizing!
        $('#' + opts.Id).resizable({
            resize: function (event, ui) { $('#' + opts.Id + ' table').bootstrapTable('resetView'); }
        });
        if (opts.HeaderTitle.toLocaleUpperCase().indexOf("SORGU")>=0) {
            $(document).bind('keypress', function (e) {
                if (e.keyCode == 13) {

                    $("#" + opts.Id + " .pull-right .btn-primary").trigger('click');
                }
            });
        }

   

    };

    String.prototype.buyukHarf = function () {

        var str = [];
        for (var i = 0; i < this.length; i++) {
            var ch = this.charCodeAt(i);
            var c = this.charAt(i);
            if (ch == 105) str.push('İ');
            else if (ch == 305) str.push('I');
            else if (ch == 287) str.push('Ğ');
            else if (ch == 252) str.push('Ü');
            else if (ch == 351) str.push('Ş');
            else if (ch == 246) str.push('Ö');
            else if (ch == 231) str.push('Ç');
            else if (ch >= 97 && ch <= 122) str.push(c.toUpperCase());
            else str.push(c);
        }
        return str.join('');
    };
    return {
        Show: ShowJSPanel
    };

});