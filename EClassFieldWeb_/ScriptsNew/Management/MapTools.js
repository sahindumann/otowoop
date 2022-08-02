define(["Management/MapToolsDefaults"],
        function (MapToolsDefaults) {

            /*
                Bağımlılıklar Listesi
                -------------------------------
            */

            return dojo.declare(null, {
                constructor: function (map) {
                    this.Map = map;
                  
                },
                /*Harita Üzerinde Önceki Konuma Gider*/
                MapBack: function (options) {
                    var opts = $.extend({}, MapToolsDefaults.MapExplorerDefaults, options);
                    if (MapToolsDefaults.PermalinkBackList.length <= 1 || $(opts.BackButtonIdentifier).parent().hasClass('disabled')) {
                        $(opts.BackButtonIdentifier).parent().addClass('disabled');
                        return;
                    }

                    var MapState = MapToolsDefaults.PermalinkBackList.pop();
                    MapToolsDefaults.PermalinkForwardList.push(MapState);
                    $(opts.BackButtonIdentifier).parent().removeClass('disabled');

                    MapState = MapToolsDefaults.PermalinkBackList[MapToolsDefaults.PermalinkBackList.length - 1];

                    this.Map.SetCenterPoint({ CenterPoint: MapState.CenterPoint });
                    this.Map.SetRotationDegree({ RotationDegree: MapState.RotationDegree });
                    this.Map.SetScale({ ScaleValue: MapState.ScaleValue });

                    MapToolsDefaults.ShouldUpdate = false;

                    if (MapToolsDefaults.PermalinkForwardList.length < 1)
                        $(opts.ForwardButtonIdentifier).parent().addClass('disabled');
                    else
                        $(opts.ForwardButtonIdentifier).parent().removeClass('disabled');

                    if (MapToolsDefaults.PermalinkBackList.length <= 1)
                        $(opts.BackButtonIdentifier).parent().addClass('disabled');
                    else
                        $(opts.BackButtonIdentifier).parent().removeClass('disabled');
                },
                /*Harita Üzerinde Sonraki Konuma Gider*/
                MapForward: function (options) {
                    var opts = $.extend({}, MapToolsDefaults.MapExplorerDefaults, options);
                    if (MapToolsDefaults.PermalinkForwardList.length <= 0 || $(opts.ForwardButtonIdentifier).parent().hasClass('disabled')) {
                        $(opts.ForwardButtonIdentifier).parent().addClass('disabled');
                        return;
                    }

                    var MapState = MapToolsDefaults.PermalinkForwardList.pop();
                    MapToolsDefaults.PermalinkBackList.push(MapState);

                    this.Map.SetCenterPoint({ CenterPoint: MapState.CenterPoint });
                    this.Map.SetRotationDegree({ RotationDegree: MapState.RotationDegree });
                    this.Map.SetScale({ ScaleValue: MapState.ScaleValue });

                    MapToolsDefaults.ShouldUpdate = false;

                    if (MapToolsDefaults.PermalinkForwardList.length < 1)
                        $(opts.ForwardButtonIdentifier).parent().addClass('disabled');
                    else
                        $(opts.ForwardButtonIdentifier).parent().removeClass('disabled');

                    if (MapToolsDefaults.PermalinkBackList.length <= 1)
                        $(opts.BackButtonIdentifier).parent().addClass('disabled');
                    else
                        $(opts.BackButtonIdentifier).parent().removeClass('disabled');
                },
                /*Harita Konum Listelerini Günceller*/
                UpdatePermalink: function (options) {
                    var opts = $.extend({}, MapToolsDefaults.MapExplorerDefaults, options);
                    if (!MapToolsDefaults.ShouldUpdate) {
                        MapToolsDefaults.ShouldUpdate = true;
                        return;
                    }

                    var State = {
                        ScaleValue: this.Map.GetScale(),
                        CenterPoint: this.Map.GetCenterPoint(),
                        RotationDegree: this.Map.GetRotationDegree()
                    };

                    MapToolsDefaults.PermalinkBackList.push(State);
                    MapToolsDefaults.PermalinkForwardList = [];

                    $(opts.ForwardButtonIdentifier).parent().addClass('disabled');

                    if (MapToolsDefaults.PermalinkBackList.length > 1)
                        $(opts.BackButtonIdentifier).parent().removeClass('disabled');
                },
                /*Haritayı İlk Görünümüne Döndürür*/
                ResetMap: function (options) {
                    var opts = $.extend({}, MapToolsDefaults.ResetMapDefaults, options);
                    this.Map.SetCenterPoint({ CenterPoint: opts.CenterPoint });
                    this.Map.SetZoomLevel({ ZoomLevel: opts.ZoomLevel });
                },
                /*Harita Üzerindeki Vektörel Çizimleri Temizler*/
                ClearMap: function (options) {
                    var opts = $.extend({}, MapToolsDefaults.ResetMapDefaults, options);

                    if (opts.TitleList) {
                        this.Map.ClearLayers({ TitleList: opts.TitleList });
                    }
                    $.each(opts.RemoveObjectIdentifierList, function (index, item) {
                        $(item).remove();
                    });
                },
                /*Tıklanan Noktadan Bilgi Döndürür*/
                GetInfo: function (options) {

                },
                /*Haritayı Kaydır*/
                PanMap: function (options) {
                    this.Map.SetActiveAllInteractions(false);
                },
                /*Koordinata Git*/
                GoCoordinate: function (options) {
                    this.Map.SetCenterPoint(options);
                },

                ZoomCoordinate:function(options)
                {
                    this.Map.SetZoomLevel(options.level);

                }
            });
        });