
define([
    "Management/BasarMapManager",
    "Utilities/Util",
    "Utilities/AjaxTools/AjaxTools",
    "Utilities/JSPanel/JSPanel",
    "dojo/request",
    "dojo/query",
    "dojo/on",
    "dojo/domReady!"], function (
        BasarMapManager,
        Util,
       AjaxTools,
       JSPanel,
        Request,
        DJQuery,
        DJOn) {






        var mapOptions = {
            MapIdentifier: '#map',
            View: {

                Center: [3928251.757632, 4736649.768776],
                Rotation: 0,
                Projection: 'EPSG:3857',
                Zoom: 6
            },
            Controls: {
                Attribution: false, Rotate: true, Zoom: true
            },
            Target: {
                CanvasId: 'map'
            }
        };
    
        //var baseLayer = MapManager.Map.CreateTileLayer({
        //    Title: 'BaseLayer', Source: new ol.source.BingMaps({
        //        key: 'Ag7dnC971UMURHWf2CkE-TZ2h-ByPh7RYrqQxyPwHfGhWaipEw0SAtwExsMrichc',
        //        imagerySet: 'RoadOnDemand'
        //    })
        //});
 


        var ChangeBaseMap = function (BaseLayerType) {

            MapManager.Map.ChangeBaseLayer({ LayerTitle: 'BaseLayer', BaseMapName: BaseLayerType, MapIdentifier: '#map' });
             
         
        };

        var GetIlans = function (_wkt)
        {

            AjaxTools.POSTDataSync("Cbs", "GetIlans", { _wkt: _wkt }, function (data) {



                MapManager.MapTools.ClearMap({ TitleList: ['BufferLayer'] });

                for (var i = 0; i < data.length; i++) {

                    if (data[i].Longtude == "POINT( )")
                        continue;

                    var feature = MapManager.Map.GetFeatureFromWKT({
                        WKTString: data[i].Longtude,
                        DataProjection: 'EPSG:4326',
                        FeatureProjection: 'EPSG:3857'
                    });

                    var style = new ol.style.Style({
                        image: new ol.style.Icon({
                            anchor: [0.5, 25],
                            anchorXUnits: 'fraction',
                            anchorYUnits: 'pixels',

                            src: '../../Content/images/pinol3.png'
                        }),

                        text: new ol.style.Text({


                            text: data[i].Text,
                            scale: 1,
                            textAlign: 'center',
                            offsetY: 15,
                            fill: new ol.style.Fill({
                                color: '#000000'
                            }),
                            stroke: new ol.style.Stroke({
                                color: '#000000',
                                width: 1
                            })
                        }),

                    });


                    if (feature != null) {

                        feature.setStyle(style);



                        MapManager.Map.AddFeature({

                            Title: 'BufferLayer',
                            Feature: feature

                        });


                    }

                }
                MapManager.Map.ViewLayerExtent({ Title: 'BufferLayer' });

            });


        }

        var ClearMap=function()
        {
            MapManager.MapTools.ClearMap({
                TitleList: ['MeasurementLayer', 'StopLayer', 'InfoLayer', 'TaskLayer', 'GarageLayer', 'PlatformLayer', 'GarageLayer', 'GarageAreaLayer', 'StationLayer', 'StationAreaLayer', 'PlatformLayer', 'PlatformAreaLayer', 'RouteLayerModify', 'QuickSearchLayer', 'NoteLayer', 'NoteModifyLayer', 'GarageModifyLayer', 'StationModifyLayer', 'PlatformModifyLayer', 'StopModifyLayer', 'BufferLayer', 'StopRouteLayer', 'GeoQueryLayer', 'SegmentLayer', 'StationLayer', '_LineBus', 'ZoomLayer'], RemoveObjectIdentifierList: ['#measurement-value', '#measurement-comment', '.tooltip-measure']
            });
        }
        $(document).ready(function () {

            MapManager = new BasarMapManager(mapOptions);

            var baseLayer = MapManager.Map.CreateTileLayer({ Title: 'BaseLayer' });
            var _BufferLayer = new MapManager.Map.CreateVectorLayer({

                Title: 'BufferLayer',
                Source: MapManager.Map.CreateVectorSource({})
            });

            MapManager.Init();

            var infoLayer = MapManager.Map.CreateVectorLayer({
                Title: 'InfoLayer',
                Source: MapManager.Map.CreateVectorSource({})

            });
            MapManager.Map.AddLayer(infoLayer);
            MapManager.Map.AddLayer(baseLayer);
            MapManager.Map.AddLayer(_BufferLayer);


            ChangeBaseMap('GR');





            $("a.tools").click(function () {


                var type = $(this).data("tool-type");
                switch (type) {

                    case "clear":
                        MapManager.MapTools.ClearMap({ TitleList: ['BufferLayer'] });
                        break;
                    case "line":
                        MapManager.Map.SetActiveInteraction(true, _BufferLayerInteraction.GetName());
                        break;
                    case "show":
                        GetIlans(null);
                    

                        break;

                    default:

                }



            });


            //Buffer Layer Interaction





            var _BufferLayerInteraction = MapManager.Map.CreateDrawInteraction('BufferLayerInteraction', 'BufferLayer', 'Polygon');
            _BufferLayerInteraction.on("drawend", function (event) {
                ClearMap();
                var _feature = MapManager.Map.GetWKTFromFeature({
                    Feature: event.feature,
                    DataProjection: 'EPSG:4326', FeatureProjection: 'EPSG:3857'
                });

               GetIlans(_feature);
                MapManager.Map.SetActiveInteraction(false, _BufferLayerInteraction.GetName());
               
       
            });






            //Add Interaction
            MapManager.Map.AddInteraction(_BufferLayerInteraction);



            MapManager.Map.SetActiveInteraction(false, _BufferLayerInteraction.GetName());

        });


    
    });