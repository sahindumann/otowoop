define(["Utilities/AjaxTools/AjaxTools", "Utilities/JSPanel/JSPanel"], function (AjaxTools, JSPanel) {
    return {
        ImageExtensions: ["png", "jpg", "jpeg", "svg", "bmp", "ico"],
        IconExtensions: ["doc", "docx", "kml", "kmz", "odt", "pdf", "rar", "tiff", "txt", "xls", "xlsx", "zip"],
        SymbolPath: "/Images/file_symbols/",
        UserPemissions: [],
        MapExportServisUrl: "http://172.16.11.122:7070/api/download?fileName=",
        ReturnStringFunction: function (FunctionName, Parameters) {
            /*Adı ve Parametreleri Verilen Fonksiyonu Döndürür*/
            var fn = window[this.FunctionName];
            if (typeof fn == 'function') {
                if (Parameters) {
                    return fn(Parameters);
                } else {
                    return fn();
                }
            }
        },
        DifferentFieldColor: function (table1, table2) {
            $(table1 + " tbody tr td").each(function (key) {
                var logthis = this;
                var loghtml = this.innerHTML;
                var thlog = $(this).parent().parent().parent().find("th")[key];
                logthis.setAttribute("data-field", $(thlog).data("field"));
            });

            $(table2 + " tbody tr td").each(function (key) {
                var logthis = this;
                var loghtml = this.innerHTML;
                var thlog = $(this).parent().parent().parent().find("th")[key];
                logthis.setAttribute("data-field", $(thlog).data("field"));
            });

            $(table2 + " tbody tr td").each(function () {

                var durak = $(this);
                var a = this.innerHTML;

                $(table1 + " tbody tr td").each(function () {
                    var b = this.innerHTML;
                    if (durak.data("field") == $(this).data("field") && durak.html() != $(this).html() && durak.data('field') != 'state') {
                        durak.css("background-color", "#dc3545");
                        durak.css("color", "white");
                        durak.find('i').css("color", "white");
                    }
                });

            });
        },
        ClosePanels: function (_panels) {
            for (var i = 0; i < _panels.length; i++) {
                if ($("#" + _panels[i]).length > 0) {
                    var _panel = jsPanel.activePanels.getPanel(_panels[i]);
                    if (_panel != undefined && _panel != null) {
                        _panel.close();
                    }
                }
            }
        },

        NormalizePanels: function (_panels) {
            for (var i = 0; i < _panels.length; i++) {
                if ($("#" + _panels[i]).length > 0) {
                    var _panel = jsPanel.activePanels.getPanel(_panels[i]);
                    if (_panel != undefined && _panel != null) {
                        _panel.normalize();
                    }
                }
            }
        },

        MinimizePanels: function (_panels) {
            for (var i = 0; i < _panels.length; i++) {
                if ($("#" + _panels[i]).length > 0) {
                    var _panel = jsPanel.activePanels.getPanel(_panels[i]);
                    if (_panel != undefined && _panel != null) {
                        _panel.minimize();
                    }
                }
            }
        },
        ShowBusPopup: function (data, coordinate) {
            $(".popupdiv").remove();
            var keys = Object.keys(data);
            var str = "";
            var elem = document.createElement("div");
            elem.id = "popup";
            elem.setAttribute("class", "ol-popup popupdiv");
            elem.innerHTML = document.getElementById("popup1").innerHTML;
            document.getElementById("popup-content").innerHTML = "";
            var overlay = new ol.Overlay({
                name: 'overlay-popup',
                element: elem,
                positioning: 'bottom-center',
                offset: [0, -10]
            });
            MapManager.Map.GetMap().addOverlay(overlay);
            overlay.setPosition(coordinate);
            for (var i = 0; i < keys.length; i++) {
                $("#popup-content").addClass("busPopup");

                if (i % 2 == 0)
                    str += "<div class='row' style='margin-bottom:5px'></div>";

                str += "<div class=\"col-md-4 col-xs-24\"><label>" + keys[i] + "</label> </div>";

                str += "<div class=\'col-md-8 col-xs-24\'><input  type=text class='form-control' value=\'" + data[keys[i]] + "\'></div>";

            }
            document.getElementById("popup-content").innerHTML += str + "</div>";
            $("#popup").css("display", "block");
            $("#popup").css("z-index", "999");
            $("#popup").css("height", (keys.length * 60) + "px");
            $("#popup").html($("#popup1").html());
            $("#popup input").prop('disabled', true);
            $(".ol-popup-closer").click(function () {
                $("#popup").remove();
            });
        },
        ReaderExcel: function (options, onComplete) {

            //return object
            //name:ilk kolon değeri başlık
            //propertyName hucre adı A1,B1
            //values:alt hücreler
            //{name:'ID',propertyName:'A1',values:[1,2,3,4,5]}

            //Get the files from Upload control
            var files = options.e.target.files;
            var i, f;
            var columns = [];
            //Loop through files
            for (i = 0, f = files[i]; i != files.length; ++i) {
                var reader = new FileReader();
                var name = f.name;
                reader.onload = function (e) {
                    var data = e.target.result;

                    var result;
                    var workbook = XLSX.read(data, { type: 'binary' });
                    var sayfa1 = workbook.Sheets.Sayfa1;
                    columns = [];
                    rows = [];
                    for (var propertyName in sayfa1) {
                        var prop = sayfa1[propertyName];
                        var iscolumn = false;
                        if (propertyName.length == 2) {
                            if (propertyName[1] == '1') {
                                iscolumn = true;
                                columns.push({ propertyName: propertyName, name: prop.h, values: [] });
                            }
                            else {
                                for (var i = 0; i < columns.length; i++) {
                                    if (columns[i].propertyName[0] == propertyName[0]) {
                                        var val;
                                        try {

                                            val = parseInt(prop.h, 0);

                                            val = prop.v;
                                        } catch (e) {

                                            val = prop.h;
                                        }

                                        columns[i].values.push(val);

                                        break;
                                    }
                                }
                            }
                        }
                    }

                    onComplete(columns);

                };
                reader.readAsArrayBuffer(f);
            }
        },
        GetSerializeAllArray: function (_formName) {
            var _myForm = $('#' + _formName);
            var disabled = _myForm.find(':input:disabled').removeAttr('disabled');
            var _data = _myForm.serializeArray();
            var _obj = {};

            for (var i = 0; i < _data.length; i++) {
                try {

                    _obj[_data[i].name] = _data[i].value;
                } catch (e) {

                }
            }
            //var obj = {};
            //var data = _myForm.serialize().replace(/\+/g, ' ').split("&");
            //for (var key in data) {
            //    console.log(data[key]);
            //    obj[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
            //}
            disabled.attr('disabled', 'disabled');

            var keys = Object.keys(_obj);
            for (var i = 0; i < keys.length; i++) {

                var val = _obj[keys[i]];
                var israido = this.isRadioButton(_formName, keys[i]);
                if (val == "on" && israido) {
                    _obj[keys[i]] = 1;
                }
                else if (val == "off" && israido) {
                    _obj[keys[i]] = 0;
                }

                if (this.isNumberClass(_formName, keys[i]) == true) {
                    _obj[keys[i]] = _obj[keys[i]].replace(".", ",");
                }



                var ischeckbox = this.isCheckbox(_formName, keys[i]);
                if (val == "on" && ischeckbox) {
                    _obj[keys[i]] = 1;
                }
                else if (val == "off" && ischeckbox) {
                    _obj[keys[i]] = 0;
                }


            }



            return _obj;
        },

        TruncateTaletd: function (id) {

            $(id + " td").each(function () {
                if (this.innerText.length > 40) {
                    this.setAttribute("data-fulltext", this.innerText);
                    if (!$.browser.mobile) {
                        this.setAttribute("title", this.innerText);
                    } else {
                        this.addEventListener("click", function () {
                            alert(this.getAttribute("data-fulltext"));
                        });
                    }
                    this.innerText = this.innerText.substr(0, 25) + "...";
                }
            });
        },
        isRadioButton: function (_formName, name) {
            var result = false;

            var ctrl = $('#' + _formName + " input[name=" + name + "]");
            if (ctrl != undefined && ctrl.length > 1) {
                $(ctrl).each(function () {



                    if (ctrl.attr('type') == "radio" && ctrl.attr("value") == "1" && ctrl.data("text") == "Aktif") {
                        return true;

                    }
                });
            }

            return false;

        },

        isCheckbox: function (_formName, name) {
            var result = false;

            var ctrl = $('#' + _formName + " input[name=" + name + "]");
            if (ctrl != undefined) {
                if (ctrl.attr('type') == "checkbox") {
                    return true;

                }
            }

            return false;

        },

        isNumberClass: function (_formName, name) {
            var result = false;

            var ctrl = $('#' + _formName + " input[name=" + name + "]");
            if (ctrl != undefined) {
                if (ctrl.hasClass("number")) {
                    return true;

                }
            }

            return false;

        },

        GetCookieSetting: function (name) {


            var cook = getCookie(name);

            if (cook != null && cook != '') {
                var musterisetting = JSON.parse(cook);

                return musterisetting;
            }
            return null;

        },

        setCookie: function (cname, cvalue, exdays) {
            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toUTCString();
            document.cookie = "";
            document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
        },

        getCookie: function (cname) {
            var name = cname + "=";
            var decodedCookie = decodeURIComponent(document.cookie);
            var ca = decodedCookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        },

        GetFeature: function (layername, featurename) {
            var features = MapManager.Map.FindLayer({ Title: layername }).getSource().getFeatures();

            for (var i = 0; i < features.length; i++) {

                if (features[i].get("name") == featurename) {
                    return features[i];
                }
            }


        },

        jsShowHideProgress: function (visible) {
            var self = this;

            setTimeout(function () { self.showLoadingPanel(true); }, 200);
            self.deleteCookie();

            var timeInterval = 500; // milliseconds (checks the cookie for every half second )

            var loop = setInterval(function () {
                if (self.IsCookieValid()) { self.showLoadingPanel(false); clearInterval(loop); }

            }, timeInterval);
        },

        // cookies
        deleteCookie: function () {
            var self = this;
            var cook = self.getCookie('ExcelDownloadFlag');
            if (cook != "") {
                document.cookie = "ExcelDownloadFlag=;Path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC";
            }
        },

        IsCookieValid: function () {
            var self = this;
            var cook = self.getCookie('ExcelDownloadFlag');
            return cook != '';
        },

        // getCookie: function (cname) {
        // var name = cname + "=";
        // var ca = document.cookie.split(';');
        // for (var i = 0; i < ca.length; i++) {
        // var c = ca[i];
        // while (c.charAt(0) == ' ') {
        // c = c.substring(1);
        // }
        // if (c.indexOf(name) == 0) {
        // return c.substring(name.length, c.length);
        // }
        // }
        // return "";
        // },

        showLoadingPanel: function (visible) {


            if (visible == true) {
                $("#loader").animate({
                    "opacity": "1",
                    "z-index": "9999"
                }, 10, function () {
                    $("#overlay").show();
                });
            }
            else {

                $("#loader").animate({
                    "opacity": "0",
                    "z-index": "-1"
                }, 10, function () {
                    $("#overlay").hide();
                });


            }



        },

        SetupViews: function (isPanoramaVisible) {



        },

        LoadDropdown: function (options) {
            var valueKey = 'value', textKey = 'text', counter = 0;
            $(options.DropdownIdentifier).find('option').remove();

            if (options.ValueKey && options.TextKey) {
                valueKey = options.ValueKey;
                textKey = options.TextKey;
            }

            if (!options.ShowTick && options.JSONData.length >= 1 && options.JSONData[0][valueKey] != "-1" && $(options.DropdownIdentifier).attr("multiple") == undefined) {

                $(options.DropdownIdentifier).append("<option value='" + "-1" + "'>" + "Seçiniz" + "</option>");
            }
            $.each(options.JSONData, function (index, item) {
                var value, text, dataParams = [];
                counter++;

                if (valueKey == 'AutoInc') {
                    value = counter;
                } else {
                    value = item[valueKey];
                }
                text = item[textKey];

                $.each(item.data, function (_index, _item) {
                    dataParams.push([['data', _index].join('-'), '"' + _item + '"'].join('='));
                });


                dataParams = dataParams.join(' ');

                $(options.DropdownIdentifier).append("<option " + dataParams + " value='" + value + "'>" + text + "</option>");
            });

            $(options.DropdownIdentifier).selectpicker({
                style: 'btn-default',
                styleBase: 'btn input-sm',
                size: 10,
                noneSelectedText: 'Lütfen Seçim Yapınız',
                showTick: options.ShowTick,
                liveSearch: true,
                liveSearchPlaceholder: 'Ara',
                showSubtext: options.ShowSubtext,
                width: options.Width,
                container: options.Container != '' ? options.Container : 'body'
            });

            if ($('button[data-id="' + options.DropdownIdentifier.substring(1) + '"]').length > 0) {
                $(options.DropdownIdentifier).selectpicker('refresh');

            } else {

                $(options.DropdownIdentifier).selectpicker({
                    style: 'btn-default',
                    styleBase: 'btn input-sm',
                    size: 10,
                    noneSelectedText: 'Lütfen Seçim Yapınız',
                    showTick: options.ShowTick,
                    liveSearch: true,
                    liveSearchPlaceholder: 'Ara',
                    showSubtext: options.ShowSubtext,
                    width: options.Width,
                    container: options.Container != '' ? options.Container : 'body'
                });
            }
            $(options.DropdownIdentifier).trigger('loaded');
        },

        PrepareDropdownData: function (options) {

            var objectArray = [];

            $.each($(options.Identifier), function (index, item) {
                var _dataParams = {};
                var _self = $(this);
                $.each(options.DataObject, function (_index, _item) {
                    _dataParams[_index] = _self.data(_item);
                });

                objectArray.push({
                    text: $(this).closest(options.ClosestElement).get(0).text.trim(),
                    value: $(this).data(options.ValueSelector),
                    data: _dataParams
                    //entityName: $(this).data(options.EntityName),
                    //cqlFilter: $(this).data(options.FilterName)
                });
            });

            return objectArray;
        },

        CreateDynamicBufferTab: function (options, onComplate) {

            if (jQuery.type(options.JSONData) == 'string') {
                options.JSONData = JSON.parse(options.JSONData);
            }

            $(options.TabIdentifier + " ul").empty();
            $(options.TabIdentifier + " div.tab-content").empty();

            if (options.Title) {
                var entityNames = options.EntityName.split(',');
                var titleNames = options.Title.split(',');
                var count = "";
                $.each(entityNames, function (index, item) {


                    var name = item.replace(/\s/g, '');
                    if (name == "DONATI") {
                        if (count != 0) {
                            name = name + "_" + count + "_";
                        }

                        count++;
                    }


                    $(options.TabIdentifier + " ul").append("<li data-index=" + index + " data-entity-name=" + entityNames[index] + "><a data-toggle=\"tab\" href=\"#tab-" + name + "\">" + titleNames[index] + "</a></li>");
                    $(options.TabIdentifier + " div.tab-content").append("<div id=\"tab-" + name + "\" class=\"tab-pane fade\"><table id=\"tbl-" + name + "\"></table></div>");
                });
                //$(options.TabIdentifier).tabs("refresh");
            } else {
                $.each(options.JSONData, function (index, item) {
                    $(options.TabIdentifier + " ul").append("<li><a data-toggle=\"tab\" href=\"#tab-" + index + "\">" + index + "</a></li>");
                    $(options.TabIdentifier + " div.tab-content").append("<div id=\"tab-" + index + "\" class=\"tab-pane fade\"><table id=\"tbl-" + index + "\"></table></div>");
                    //$(options.TabIdentifier).tabs("refresh");
                });
            }

            if (onComplate) {
                onComplate();
            }

        },

        JoinArrayWithServerVariable: function (options, onComplate) {
            /*
            ListObject      :
            ServerVariable  :
            Operator        :
            Sperator        :
            */
            var result = [];

            $.each(options.ListObject, function (index, item) {
                result.push([options.ServerVariable, item].join(options.Operator));
            });

            return result.join(options.Sperator);

        },

        _convertValidRowsToObject: function (options) {
            var result;

            if (options.Class == undefined) {
                result = {};
            } else {
                result = options.Class;
            }
            var data;
            if (JSON.parse(options.JSONObject)["Values"] == undefined)
                data = JSON.parse(options.JSONObject);
            else
                data = JSON.parse(options.JSONObject).Values;

            if (result[options.EntityName] == undefined) {
                if (JSON.parse(options.JSONObject)["Values"] != undefined)
                    result[options.EntityName] = data;
                else
                    result[options.EntityName] = [data];
            } else {
                result[options.EntityName].push(data);
            }

            return result;
        },

        CreateValidServerSaveAreaClass: function (options, onComplate) {
            var self = this;
            var classObject = {
                Name: options.Name

            };
            $.each(options.Rows, function (index, item) {
                classObject = self._convertValidRowsToObject({ Class: classObject, EntityName: item['EntityName'], JSONObject: item['JSONObject'] });
            });

            return classObject;
        },

        CreateValidServerSavePOIClass: function (options, onComplate) {
            var self = this;
            var classObject = {
                Name: options.Name,
                Poi: []
            };
            $.each(options.Rows, function (index, item) {
                classObject.Poi.push(JSON.parse(item['JSONObject']));
            });

            return classObject;
        },



        ConvertToInteger: function (value, onComplate) {
            var result;

            if (value.constructor == Array) {

                var newArray = [];
                $.each(value, function (index, item) {
                    newArray.push(parseInt(item.replace(/[^0-9\.]+/g, '')));
                });

                result = newArray;
            } else {
                result = parseInt(value.replace(/[^0-9\.]+/g, ''));
            }

            if (onComplate) {
                onComplate(result);
            } else {
                return result;
            }


        },

        //Kampanya Alanları Çizim sonrası Temizleme
        CleanPoiCampaign: function () {
            $("#poi-virtual-name").val('');
            $("#poi-buffer-point-radius-input").val('');
            $(".areaselect .selectpicker").selectpicker('deselectAll');
            $("#poi-basarsoft-collapse  .selectpicker").selectpicker("deselectAll");


        },

        //Kampanya Alanları Çizim sonrası Temizleme
        CleanManagerialCampaign: function (options) {
            $("#managerial-virtual-name").val('');
            switch (options.Id) {
                case 'managerial':

                    $(".areatab li").each(function () {

                        var href = $(this).find("a").attr("href");

                        $(href + " .selectpicker").each(function () {

                            $(this).selectpicker('deselectAll');
                        });
                    });
                    break;

                case 'catchment':

                    $("#managerial-catchment-distance-input").val('');

                    break;

                case 'buffer':
                    $("#managerial-buffer-radius-input").val('');
                    break;
            }
        },

        //Çizim sonrası harita üzerindeki tooltipleri gizler
        OverlayDisplay: function (value) {

            setTimeout(function () {
                //$(".ol-overlaycontainer").css("display", "none");
                $("#measurement-comment").css("display", value);

                //$("#measurement-value").css("display", value);
                $(".tooltip-measure").css("diplay", value);
            }, 10);
        },

        /*Katman Arama Fonksiyonu*/
        LayerSearchByName: function (options) {

            var katmanlar = $(".layer-search");
            var val = options.value;

            if (val == "") {
                $(".layer-search").css("display", "block");
                return;
            }

            for (var i = 0; i < katmanlar.length; i++) {
                var katman = katmanlar[i];
                katman.id = "layer_" + i;


                var katmantext = this.GetLowercaseTurkishCharacter(katman.getAttribute("data-quick-search"));
                val = this.GetLowercaseTurkishCharacter(val);

                if (katmantext.replace(" ", "").indexOf(val.replace(" ", "")) >= 0) {
                    $(katmanlar[i]).css("display", "block");
                    $("#" + katman.id).parent().css("display", "block");

                }
                else {
                    $(katmanlar[i]).css("display", "none");

                }
            }

        },

        GetLowercaseTurkishCharacter: function (val) {

            var string = val;
            var letters = {
                "İ": "i", "I": "ı", "Ş": "ş", "Ğ": "ğ", "Ü": "ü", "Ö": "ö", "Ç": "ç"
            };
            string = string.replace(/(([İIŞĞÜÇÖ]))/g, function (letter) {
                return letters[letter];
            });
            return string.toLowerCase();
        },

        //Açık katman sayısı yazdırma
        ShowOpenedLayerCount: function () {




            var isallclose = true;
            $(".cd-accordion-menu .layer-quick-search").each(function () {

                var len = $(this).find(".material-switch").length;

                var lenchecked = $(this).find(".material-switch input:checked").length;





                if (len == lenchecked) {

                    $(this).find(".chk")[0].checked = true;
                }
                else {
                    $(this).find(".chk")[0].checked = false;
                }







            });

            $(".cd-accordion-menu  ul").each(function (index, item) {



                var ischeckedlayercount = $(this.parentNode).children().find("[class='sublayer']:checked").length;

                var totalcount = $(this.parentNode).children().find("[class='sublayer']").length;
                var text = item.parentNode.getElementsByClassName("layer-menu")[1].innerHTML.split('<')[0];
                if (ischeckedlayercount >= 1) {

                    item.parentNode.getElementsByClassName("layer-menu")[1].innerHTML = text + "<span class='badge badge-success pull-right'>" + ischeckedlayercount + "</span>";
                }
                else {
                    item.parentNode.getElementsByClassName("layer-menu")[1].innerHTML = text;
                }


                //if (ischeckedlayercount == totalcount)
                //{

                //    $(this.parentNode).find(".allcheck").iCheck('check');
                //}
                //else {
                //    isallclose = false;
                //    $(this.parentNode).find(".allcheck").iCheck('uncheck');
                //    return;
                //}


            });







            $('.allcheck input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue',
                increaseArea: '20%'
            });


            $('.allcheck input').on('ifChecked', function (event) {

                //if (!isallclose)
                //    return;



                var level = $(this).data("level");

                //alert($(".level-" + level + " input").length);
                $(".level-" + level + " input").each(function () {

                    if (this.checked == false) {
                        $(this).click();
                    }

                });

            });

            $('.allcheck input').on('ifUnchecked', function (event) {


                var level = $(this).data("level");
                $(".level-" + level + " input").each(function () {

                    if (this.checked == true) {
                        $(this).click();
                    }

                });

            });
        },

        MapIcons: function () {

            $('.map-icons.destkop').removeClass("tt");
            $('.map-icons.destkop').removeClass("tt2");
            var a = $('#map-content').width();
            var b = $('.map-altlik').width();
            var d = $('.map-icons.destkop').width();
            c = a - b - 70;
            if (d > c) {
                $('.map-icons.destkop').addClass("tt");
                setTimeout(function () {
                    var e = $('.tt').height();
                    var g = $('#map-content').height() - 100;
                    f = (-e / 2) + 'px';
                    if (g > e) {
                        $('.tt').css("margin-top", f);

                    }
                    else {
                        $('.map-icons.destkop').addClass("tt2");
                        e = $('.tt').height();
                        f = (-e / 2) + 'px';
                        $('.tt').css("margin-top", f);
                    }
                }, 500);
            }
            else {
                $('.map-icons.destkop').removeClass("tt");
                $('.map-icons.destkop').css("margin-top", "");
            }


        },

        MySplitterim: function () {
            var _self = this;
            var w = window,
                d = document,
                e = d.documentElement,
                g = d.getElementsByTagName('body')[0],
                x = w.innerWidth || e.clientWidth || g.clientWidth,
                y = w.innerHeight || e.clientHeight || g.clientHeight;
            $('#map-splitter').height(y - 55).enhsplitter({
                minSize: 50, vertical: true,
                onDrag: function (event) {
                    _self.MapIcons();
                    google.maps.event.trigger(googleStreetView.panorama, "resize");
                    MapManager.Map.UpdateSize();
                }
            });

            $('#map-content').css("width", "100%");
            $('#street-view').css("width", "100%");

        },
        //Side Menu Açma Kapama
        ToggleButton: function (opts) {
            var _self = this;
            var tooglebtn = opts.Id;
            $("#map-canvas").css("overflow", "hidden");
            if ($("#main-content").hasClass("aktif")) {
                var genislik = '0';
                var yukseklik = '0';
                $(tooglebtn).addClass("open");
                if ($("#map-splitter").hasClass("splitter_container") == true) {
                    _self.MySplitterim();
                }
                $("#main-content").removeClass("aktif");
                $("#scale-control").addClass("scale-control-side");
                $("#scale-control").removeClass("scale-control-noside");
                google.maps.event.trigger(googleStreetView.panorama, "resize");
                if (googleStreetView.panorama.getVisible() == true) {
                    setTimeout(function () {
                        $('#map-splitter').enhsplitter('refresh');
                    }, 500);
                }
                setTimeout(function () {
                    MapManager.Map.UpdateSize();
                }, 500);
            }
            else {

                $(tooglebtn).removeClass("open");
                $("#scale-control").removeClass("scale-control-side");
                $("#scale-control").addClass("scale-control-noside");
                if (googleStreetView.panorama.getVisible() == true) {
                    setTimeout(function () {
                        $('#map-splitter').enhsplitter('refresh');
                    }, 500);
                }
                setTimeout(function () {
                    google.maps.event.trigger(googleStreetView.panorama, "resize");
                    MapManager.Map.UpdateSize();

                }, 500);

                var genislik = '-300';
                var yukseklik = '0';
                $("#main-content").addClass("aktif");

            }
            setTimeout(function () {

                $("body").css("overflow", "hidden");
                setTimeout(function () {
                    MapManager.Map.UpdateSize();
                }, 500);
            }, 500);
            var translate = 'translate(' + genislik + 'px, ' + yukseklik.toString() + 'px)';
            $('#main-content').css({
                '-webkit-transform': translate,
                '-ms-transform': translate,
                '-o-transform': translate,
                'transform': translate
            });
            _self.MapIcons();
        },

        UpdateLayersProgressBar: function (layerList) {


            function Progress(el) {
                this.el = el;
                this.loading = 0;
                this.loaded = 0;
            }


            /**
             * Increment the count of loading tiles.
             */
            Progress.prototype.addLoading = function () {
                if (this.loading == 0) {
                    this.show();
                }
                ++this.loading;
                this.update();
            };


            /**
             * Increment the count of loaded tiles.
             */
            Progress.prototype.addLoaded = function () {
                var this_ = this;
                setTimeout(function () {
                    ++this_.loaded;
                    this_.update();
                }, 100);
            };


            /**
             * Update the progress bar.
             */
            Progress.prototype.update = function () {
                var width = (this.loaded / this.loading * 100).toFixed(1) + '%';
                this.el.style.width = width;
                if (this.loading == this.loaded) {
                    this.loading = 0;
                    this.loaded = 0;
                    var this_ = this;
                    setTimeout(function () {
                        this_.hide();
                    }, 500);
                }
            };


            /**
             * Show the progress bar.
             */
            Progress.prototype.show = function () {
                this.el.style.visibility = 'visible';
            };


            /**
             * Hide the progress bar.
             */
            Progress.prototype.hide = function () {
                if (this.loading == this.loaded) {
                    this.el.style.visibility = 'hidden';
                    this.el.style.width = 0;
                }
            };

            var progress = new Progress(document.getElementById('progress'));

            var self = this;
            var layers = layerList;

            $.each(layers.getArray(), function (obj, key) {

                var source = key.getSource();

                if (source != null) {
                    if (source instanceof ol.source.ImageWMS) {

                        source.on('imageloadstart', function () {
                            //   progress.addLoading();

                            document.getElementById("loading-tematik").style.display = "block";
                            self.showLoadingPanel(true);

                        });

                        source.on('imageloadend', function () {
                            //progress.addLoaded();
                            self.showLoadingPanel(false);
                            document.getElementById("loading-tematik").style.display = "none";
                        });
                        source.on('imageloaderror', function () {
                            //progress.addLoaded();
                            document.getElementById("loading-tematik").style.display = "none";
                            self.showLoadingPanel(false);
                        });

                    }
                    else {

                        source.on('tileloadstart', function () {
                            progress.addLoading();
                        });

                        source.on('tileloadend', function () {
                            progress.addLoaded();
                        });
                        source.on('tileloaderror', function () {
                            progress.addLoaded();
                        });
                    }

                }
            });


        },

        GetSQLFromQueryBuilder: function (options, onComplate) {
            var resultObject = {};
            if (!$(options.Identifier).queryBuilder('getRules')) {
                resultObject.Error = { Message: 'Lütfen Önce Kural Oluşturunuz !', Title: 'Uyarı!', Type: 'warning' };
            } else {
                if (options.Encode) {
                    resultObject.SQL = encodeURIComponent($(options.Identifier).queryBuilder('getSQL', false).sql);
                } else {
                    resultObject.SQL = $(options.Identifier).queryBuilder('getSQL', false).sql;
                }
            }

            if (onComplate) {
                onComplate(resultObject);
            } else {
                return resultObject;
            }

        },

        PrepareTableToFormView: function (options) {

            var id = options.Identifier;

            var total = Object.getOwnPropertyNames(options.Object).length;
            var itemCount = 5;
            var divCount = (total % itemCount > 0) ? Math.ceil(total % itemCount) : (total / itemCount);
            var str = "";
            var width = (divCount % 2 == 0 ? "width:30%" : "width:40%");
            if (total <= options.MinItemCount) {
                width = "width:90%";

            }
            $.each(options.Object, function (key, element) {
                if (options.InvalidColumns.indexOf(key) <= -1) {
                    var text = options.Object[key] + "";
                    str += "<div  style='float:left;" + width + ";margin:5px'>";
                    str += "<span style='width:50%;float:left;font-weight:bold;margin-top:5px;margin-top:7px'>" + (key.length >= 20 ? key.substring(0, 20) : key) + " </span>";
                    if (text.length >= 20) {

                        str += "<textarea  class='form-control' style='width:50%;float:right'>" + (options.Object[key] == null ? '--' : options.Object[key]) + "".replace(".", ",") + "</textarea>";
                    }
                    else {

                        str += "<input  type=text class='form-control' style='width:50%;float:right' value='" + (options.Object[key] == null ? '--' : options.Object[key]) + "".replace(".", ",") + "' />";
                    }
                    str += "</div>";
                }
            });

            $(id).append(str);

            if (options.scroll) {
                setTimeout(function () {
                    $(id).mCustomScrollbar({
                        theme: "minimal-dark",

                        scrollInertia: 500,
                        autoHideScrollbar: true,
                        advanced: {
                            updateOnContentResize: true
                        }

                    });

                    $(id).css("max-height", (window.innerHeight / 2));
                }, 500);
            }

        },

        ReplaceCharecter: function (invalidchacaters, text, newcharecter) {

            for (var i = 0; i < invalidchacaters.length; i++) {

                text.replace(invalidchacaters[i], "");
            }

            return text;
        },

        GetPropertyValue: function (options) {
            var result = '';
            $.each(options.Object, function (key, element) {
                if (key == options.PropertyName) {
                    result = element;
                }
            });
            return result;
        },

        GetPropertiesObject: function (options) {
            var list = [];
            $.each(options.Object, function (key, element) {
                list.push(key);
            });
            return list;
        },

        GetFieldBootStrapTableShowColumns: function (options) {
            // $(options.Identifier).bootstrapTable(options.Method);;
            var datacolumn = undefined;
            options.FieldName = 'field';
            var columns = [];
            if (datacolumn == undefined) {
                var cols = $(options.Identifier + " thead tr th").each(function () {
                    columns.push(this.innerText.trim());
                });
            }
            else {
                $.each(datacolumn, function (i, element) {

                    if (options.FieldName == "name")
                        columns.push(element.name);
                    else if (options.FieldName == "field")
                        columns.push(element.field);

                });
            }




            return columns;
        },

        ObjectToQueryString: function (object, progress) {
            var self = this;
            var query = "s=1";
            if (progress == undefined)
                self.jsShowHideProgress();
            if (object.filterValues != undefined) {

                for (var s = 0; s < object.filterValues.length; s++) {
                    object.filterValues[s] = object.filterValues[s].replace(",", ".");
                }


                for (var s = 0; s < object._propertyNames.length; s++) {

                    if (object._propertyValues[s] != undefined) {
                        object._propertyValues[s] = object._propertyValues[s].replace(",", ".");

                        object.filterValues.push(object._propertyValues[s]);
                        object.filterColumns.push(object._propertyNames[s]);
                    }

                }
            }
            for (var i in object) {
                if (object[i] == null || object[i] == [] || object[i].length == 0)
                    continue;










                var split = object[i].toString().split(',');
                if (split.length >= 1) {
                    for (var s = 0; s < split.length; s++) {
                        query += i + "=" + split[s] + "&";
                    }
                    query = query.substr(0, query.length - 1);
                    query += "&";
                }
            }
            return query;

        },
        SegmentObjectToQueryString: function (object, progress) {
            var self = this;
            var query = "s=1";
            if (progress == undefined)
                self.jsShowHideProgress();
            if (object.filterValues != undefined) {

                for (var s = 0; s < object.filterValues.length; s++) {
                    object.filterValues[s] = object.filterValues[s].replace(",", ".");
                }
            }
            for (var i in object) {
                if (object[i] == null || object[i] == [] || object[i].length == 0)
                    continue;

                var split = object[i].toString().split(',');
                if (split.length >= 1) {
                    for (var s = 0; s < split.length; s++) {
                        query += i + "=" + split[s] + "&";
                    }
                    query = query.substr(0, query.length - 1);
                    query += "&";
                }
            }
            return query;

        },
        GetBeatifulNamesForm: function (data, names) {
            var keys = names;
            var str = "";
            for (var i = 0; i < keys.length; i++) {

                var val = data[keys[i].Value];
                if (val == null)
                    continue;

                if (keys[i].Value.indexOf("TARIH") >= 0) {
                    val = moment(val).format('DD.MM.YYYY');

                }
                if (val == "Invalid date") {
                    val = "Belirtilmemiş";
                }

                if (i % 2 == 0)
                    str += "<div class='row' style='margin-bottom:8px'></div>";

                str += "<div class=\"col-xs-12\"><label style='font-size:8pt'>" + keys[i].Text + "</label> </div>";
                if ((keys[i].Type == "checkbox" || keys[i].Type == "radio")) {

                    var s = "<div class=\"col-xs-12\"><input  type='checkbox' class='form-control' @d  /></div>";
                    s = s.replace("@d", (val != null && val == 0) ? "" : "checked");
                    str += s;
                }
                else {
                    if (val == null)
                        val = "Belirtilmemiş";

                    str += "<div class=\"col-xs-12\"><input  type=text class='form-control' value='" + val + "' /></div>";
                }


            }

            return str;
        },
        /*Verilen Belirteçteki Elemenlar Arasında Döner ve Verileri Dizi veya Sıralı Olarak Döndürür*/
        GetEachData: function (object) {
            var result = [];

            $.each($(object.Identifier), function (index, item) {
                if (object.DataIdentifier == 'val') {
                    result.push($(this).val());
                } else if (object.DataIdentifier == 'text') {
                    result.push($(this).text());
                } else {
                    result.push($(this).data(object.DataIdentifier));
                }
            });

            if (object.Sperator) {
                result = result.join(object.Sperator);
            }

            return result;
        },

        CreateTableColumns: function (table, count, checkbox, formatter) {
            var inner = "<thead><tr>";
            //if (checkbox != undefined && checkbox == true) {
            //    inner += "<th data-field='SELECTED'>&nbsp;</th>";
            //}

            for (var i = 0; i < count; i++)
                inner += "<th>&nbsp;</th>";


            //if (formatter != undefined && formatter == true)
            //    inner += "<th data-field='ISLEM'>&nbsp;</th>";

            inner += "</tr></thead><tbody></tbody>";
            table.html(inner);
        },

        ServerSideTableShowFiled: function (tableID, container, headers) {

            var len = $(tableID + " tr th").toArray();

            $(container + " .keep-open ul li").each(function (index, key) {

                if (headers[index] != undefined && headers[index].visible == false) {
                    $(key).remove();
                    $(len[index]).remove();
                }
                if (headers[index] != undefined) {
                    $(key).find("label").append(headers[index].title);
                    $(key).find("input[type=checkbox]").attr("data-field", headers[index].field);
                }

            });
        },

        ServerSideTableColumnStyle: function (id, aktifColumn) {

            var index = 0;
            $(id + " thead tr th").each(function () {


                if (this.innerText.indexOf(aktifColumn) >= 0) {
                    index = index;
                    return false;

                }
                index++;
            });
            $(id + " tbody tr").each(function () {

                var tr = this;

                var index2 = 0;
                if ($(this).find("td").each(function () {

                    if (index == index2) {
                        if (this.innerText == "1") {

                            //$(tr).addClass("success");


                        }
                        else if (this.innerText == "0") {
                            //$(tr).addClass("danger");
                        }
                        else {
                            //$(tr).addClass("warning");
                        }

                    }
                    else {

                        if (this.innerText.indexOf("Date") >= 0) {

                            this.innerText = dateFormat(this.innerText);
                        }
                    }

                    index2++;

                }));


                function dateFormat(value) {
                    return moment(value).format('DD.MM.YYYY');
                }


            });

        },

        infoInteractionMethod: function () {

            event = _Self._infoInteractionFeature;
            event.preventDefault();
            var infoTable = [];
            var activeLayers = [];
            var MapZoomLevel = MapManager.Map.GetZoomLevel();
            $.each($('input[id^="layer-item-"][type="checkbox"]:checked'), function (index, item) {
                var layerIsVisible = $(this).parent().parent().data('min-visible-range') <= MapZoomLevel && $(this).parent().parent().data('max-visible-range') >= MapZoomLevel;
                if (layerIsVisible) {
                    var layerObject = {
                        LayerName: $(this).data('layer-name'),
                        LayerType: $(this).data('service-type'),
                        DisplayName: $(this).closest('a').text().trim() /* IE için düzeltildi*/
                    };
                    activeLayers.push(layerObject);
                }
            });

            if (activeLayers.length > 0) {
                var requestURL = MapManager.Map.GetFeatureInfoUrl({
                    LayerName: 'WMSLayer', Coordinates: event.feature.getGeometry().getCoordinates(), PixelRadius: 5, FeatureCount: 10
                });

                AjaxTools.CallStandartAjax(requestURL, 'GET', 'json', false, null, function (data) {
                    if (data == undefined || data == null || data.length <= 0) {
                        MessageBox.Show({
                            Type: 'OK', Title: 'Hata!', MessageText: 'Üzgünüm, İşlem Aşağıdaki Sebeplerden Dolayı Devam Edemiyor!<br/><br/>', MessageType: 'danger'
                        });
                        return;
                    } else {
                        $.each(data, function (index, featItem) {
                            var propertiesList = _Self.ReadProperties(featItem['properties']);
                            if (propertiesList.trim().length <= 0) {
                                infoTable.push({
                                    TableName: featItem.layerName, DisplayName: featItem.name, ID: featItem.id
                                });
                            } else {
                                infoTable.push({
                                    TableName: featItem.layerName, DisplayName: featItem.name + ' (' + propertiesList + ')', ID: featItem.id
                                });
                            }
                        });
                    }





                });
            }


            var infopanel = JSPanel.Show({
                Id: 'Get-Info',
                ContentSize: (window.innerWidth > 1024 ? 890 : window.innerWidth - 50) + ' auto',
                ContentAjax: {
                    url: 'Partial/CallPartial',
                    data: {
                        PartialName: 'Info_List'
                    },
                    async: false,
                    done: function (data, textStatus, jqXHR, panel) {
                        Util.showLoadingPanel(false);
                        this.content.empty();
                        this.content.css('padding', '5px').append(data);
                    },
                    fail: function (jqXHR) {
                        this.setTheme('danger')
                            .headerTitle('<i class="fa fa-exclamation-triangle"></i> İstek Başarısız')
                            .content.append(jqXHR.responseText).css("padding", "20px");
                    },
                    always: function (arg1, textStatus, arg3, panel) {
                        Util.showLoadingPanel(false);

                        var _columns = [
                            {
                                field: 'ID',
                                title: 'ID',
                                align: 'left',
                                valign: 'middle',
                                sortable: true
                            },
                            {
                                field: 'DisplayName',
                                title: 'Tablo Adı',
                                align: 'left',
                                valign: 'middle',
                                sortable: true
                            }

                            //    {
                            //        field: 'TableName',
                            //        title: 'Tablo',
                            //        align: 'left',
                            //        visible: false
                            //}
                        ];

                        CreateReadOnlyDynamicTable({
                            TableIdentifier: '#info-list-table', TableData: infoTable, ColumnNames: _columns, CardView: false, onClickRow: function (row, $element, field) {
                                if (row.TableName == "DURAK") {



                                    _Stop._StopID = row.ID;

                                    var _feature = MapManager.Map.GetWKTFromFeature({
                                        Feature: event.feature,
                                        DataProjection: 'EPSG:7932', FeatureProjection: 'EPSG:3857'
                                    });

                                    _Stop._WktString = _feature;
                                    _Stop.GetStop();






                                }

                            }
                        });

                        $('#info-list-table').on('dbl-click-row.bs.table', function (row, $element, field) {

                            var headerTitle = $element.DisplayName;


                        });

                        MapManager.Map.SetActiveInteraction(false, InfoInteraction.GetName());

                        this.resize({
                            width: "auto", height: "auto", maxwidth: $(window).width() / 2
                        }).reposition();
                    }
                },
                HeaderTitle: 'BİLGİ ALMA'
            });

            //MapManager.Map.RemoveFeature({Title:'InfoLayer',Feature:event.feature});
            MapManager.Map.SetLayerSourceNull({
                Title: 'InfoLayer'
            });
        },

        ReadProperties: function (data) {
            var result = [];
            var str = "";
            $.each(Object.keys(data), function (index, item) {

                str += item + " : " + data[item] + " ";





            });
            result.push(str);

            return result.join();
        },

        CreateReadOnlyDynamicTable: function (options, onItemClick) {

            $(options.TableIdentifier).bootstrapTable('destroy');
            if (options.ColumnNames == null) {
                options.ColumnNames = this.ReturnDataColumnNames({
                    TableData: options.TableData
                });
            }

            $(options.TableIdentifier).bootstrapTable({
                data: options.TableData,
                columns: options.ColumnNames,
                sortable: true,
                showColumns: true,
                search: true,
                locale: 'tr-TR',
                showRefresh: false,
                showExport: this.GetExportPermission(),
                showFooter: true,
                exportOptions: {
                    fileName: 'Tablo-Cikti',
                    pdfFontSize: 12,
                    pdfLeftMargin: 20,
                    htmlContent: true
                },
                exportDataType: 'all',
                exportTypes: ['excel', 'csv', 'pdf', 'doc', 'xml', 'txt'],
                showToggle: true,
                mobileResponsive: true,
                cache: false,
                striped: true,
                pagination: true,
                pageSize: 10,
                pageList: [10, 25, 50, 100, 200],
                cardView: options.CardView,
                onClickRow: options.onClickRow


            });



        },

        ReturnDataColumnNames: function (options) {
            var ColumnNames = [];
            for (var key in options.TableData[0]) {


                var obj = {
                };
                obj.field = key;
                obj.title = key;
                obj.align = 'left';
                obj.valign = 'middle';
                ColumnNames.push(obj);
            }
            return ColumnNames;
        },

        IsArrayContains: function (array, field, value) {

            var result = true;
            for (var i = 0; i < array.length; i++) {

                if (array[i][field] == value) {
                    result = false;
                    break;
                }

            }
            return result;
        },
        IsArrayContains2: function (array, value) {

            var result = true;
            for (var i = 0; i < array.length; i++) {

                if (array[i] == value) {
                    result = i;
                    break;
                }

            }
            return result;
        },

        SetArrayValue: function (array, field, arrayfield, value, valueArray) {


            for (var i = 0; i < array.length; i++) {

                if (array[i][field] == value) {

                    array[i][arrayfield] = valueArray;
                }

            }


            return array;
        },

        GetArrayValues: function (array, field, fieldname, arrayfield) {
            var arrayresult = [];

            for (var i = 0; i < array.length; i++) {

                if (array[i][field] == fieldname) {

                    arrayresult = array[i][arrayfield];
                    break;
                }

            }

            return arrayresult;
        },

        DeleteArrayValue: function (array, field, value) {


            for (var i = 0; i < array.length; i++) {

                if (array[i][field] == value) {


                    array.splice(i, (i + 1));
                    break;
                }

            }

            return array;
        },

        ExportKML: function (data) {
            var features = [];
            for (var i = 0; i < data.length; i++) {
                var feature = MapManager.Map.GetFeatureFromWKT({
                    WKTString: data[i]["GEOLOCWKT"],
                    DataProjection: 'EPSG:7932',
                    FeatureProjection: 'EPSG:4326'
                });
                var style = new ol.style.Style({
                    //I don't know how to get the color of your kml to fill each room
                    //fill: new ol.style.Fill({ color: '#000' }),
                    stroke: new ol.style.Stroke({ color: '#000' }),
                    text: new ol.style.Text({
                        text: data[i]["ADI"],
                        font: '12px Calibri,sans-serif',
                        fill: new ol.style.Fill({ color: '#000' }),
                        stroke: new ol.style.Stroke({
                            color: '#fff', width: 2
                        })
                    })
                });
                feature.setStyle(style);
                features.push(feature);
            }

            var string = new ol.format.KML().writeFeatures(features);
            var encodedString = btoa(string);

            document.getElementById("frameexport").src = 'data:application/vnd.google-earth.kml+xml;base64,' + encodedString;



        },

        LoadUnSavedObjectDetail: function (_data) {
            $.each(_data, function (_inputName, _inputValue) {
                var _input = $('input[name=' + _inputName + ']');
                if ((_input.prop("type")).toLowerCase() == "radio") {
                    $("#" + $('input[name=' + _inputName + '][value = ' + _inputValue + ']').prop("id")).iCheck('check');
                }
                else
                    _input.val(_inputValue);

                delete _input;
            });
        },

        GetStopFilePath: function () {
            var _filePath;
            AjaxTools.GETDataSync("Helper", "GetStopFilePath", function (_path) {
                _filePath = _path;
            });
            return _filePath;
        },

        GetGarageFilePath: function () {
            var _filePath;
            AjaxTools.GETDataSync("Helper", "GetGarageFilePath", function (_path) {
                _filePath = _path;
            });
            return _filePath;
        },

        GetPlatformFilePath: function () {
            var _filePath;
            AjaxTools.GETDataSync("Helper", "GetPlatformFilePath", function (_path) {
                _filePath = _path;
            });
            return _filePath;
        },

        getAutoStopLocationFill: function (action, val, cmbid, selectedVal, data) {

            $.ajax({
                url: "/Address/" + action,
                type: 'POST',

                data: data
            })
                .done(function (data) {


                    $(cmbid).append("<option value=-1>Seçiniz</option>");

                    for (var i = 0; i < data.length; i++) {

                        if (data[i].ID == selectedVal) {
                            $(cmbid).append("<option selected value=" + data[i].ID + ">" + data[i]["ADI"] + "</option>");
                        }
                        else {
                            $(cmbid).append("<option  value=" + data[i].ID + ">" + data[i]["ADI"] + "</option>");
                        }

                    }

                });
        },

        BootStrapTablePageList: function (total, mod, iterator) {
            if (total < 99) {

                iterator = 10;
            }
            else {
                iterator *= 10;
            }
            if (total % mod != 0) {
                while (total % mod != 0) {

                    total++;
                }
            }
            var pageList = [];

            for (var i = 0; i < (total / iterator); i++) {

                pageList.push((i + 1) * iterator);
            }

            return pageList;

        },

        ImageFormatter: function (_id, _file, element, rotate, clickEventName) {
            var ex = _file.FILE_NAME.split('.').pop().toLowerCase();
            var a = this;
            if (_file.ID != null || _file.ID != undefined) {
                //if(rotate==undefined)
                //    return '<a target="_blank" href="' + this.GetStopFilePath() + _file.ID + '">' + this.GetFilePreview(_file) + '</a>';
                //else
                if (ex == "bmp" || ex == "png" || ex == "jpg" || ex == "jpeg") {

                    var str = '';
                    str = '<a target="_blank" href="' + this.GetStopFilePath() + _file.ID + '">' + this.GetFilePreview(_file) + '</a>' + "</br></br>";
                    //if (this.HasPermission("hat_guncelleme")) {
                    str += "<button onclick='return _ImageProcess.ImageProcess(this)' id='image-rotate-button' data-id=" + _file.ID + " class='btn btn-primary'>Resim Düzenle</button>";


                    //}
                    return str;
                }
                else {
                    return '<a target="_blank" href="' + this.GetStopFilePath() + _file.ID + '">' + this.GetFilePreview(_file) + '</a>';
                }

            }

            else if (_file.PREVIEW == null || _file.PREVIEW == undefined) {
                _file.PREVIEW = $("#stop-file-preview-div").clone().html();
                return _file.PREVIEW;
            }
            else {
                return _file.PREVIEW;
            }

        },
        b64toBlob: function (b64Data, contentType, sliceSize) {
            contentType = contentType || '';
            sliceSize = sliceSize || 512;

            var byteCharacters = atob(b64Data);
            var byteArrays = [];

            for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                var slice = byteCharacters.slice(offset, offset + sliceSize);

                var byteNumbers = new Array(slice.length);
                for (var i = 0; i < slice.length; i++) {
                    byteNumbers[i] = slice.charCodeAt(i);
                }

                var byteArray = new Uint8Array(byteNumbers);

                byteArrays.push(byteArray);
            }

            var blob = new Blob(byteArrays, { type: contentType });
            return blob;
        },
        ShowLocalFile: function (_file, _imageId) {
            var _extension = _file.name.split(".").pop().toLowerCase();
            if (this.ImageExtensions.indexOf(_extension) > -1) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $(_imageId).attr('src', e.target.result);
                };
                reader.readAsDataURL(_file);
            }
            else if (this.IconExtensions.indexOf(_extension) > -1) {
                $(_imageId).attr('src', this.SymbolPath + _extension + "-symbol.svg");
            }
            else {
                $(_imageId).attr('src', this.SymbolPath + "blank-symbol.svg");
            }

        },

        GetFilePreview: function (_file) {
            var _src = "";
            if (this.ImageExtensions.indexOf(_file.FILE_EXTENSION.toLowerCase()) > -1) {
                _src = this.GetStopFilePath() + _file.ID;
            }
            else if (this.IconExtensions.indexOf(_file.FILE_EXTENSION.toLowerCase()) > -1) {
                _src = this.SymbolPath + _file.FILE_EXTENSION + "-symbol.svg";
            }
            else {
                _src = this.SymbolPath + "blank-symbol.svg";
            }
            return '<img style="max-width:150px; max-height:75px;" id="' + _file.ID + '" name="' + _file.FILE_NAME + '" src="' + _src + '" />';
        },

        DateFormat: function (_date) {
            return moment(_date).format('DD.MM.YYYY');
        },
        GetBootstrapFieldIdforValue: function (tableId, fieldName) {
            var _index = 0;
            $(tableId + " thead tr th").each(function (index) {

                if ($(this).data("field") == fieldName) {
                    _index = index;
                }

            });

            return _index;


        },
        InsertToArrayToArray: function (datas, data) {
            var clms = [];
            clms.push(data);
            for (var i = 0; i < datas.length; i++) {
                clms.push(datas[i]);
            }
            datas = clms;
            return datas;
        },

        //Permissions
        HasPermission: function (_permission) {
            var _result = false;
            for (var i = 0; i < this.UserPemissions.length; i++) {
                if (this.UserPemissions[i] === _permission) {
                    _result = true;
                }
            }

            return _result;
        },
        GetExportPermission: function () {

            var _result = this.HasPermission("import_export");
            return _result;
        }
    };

});