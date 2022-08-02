define(["Utilities/Openlayers/OLDefaults",
    "Utilities/Openlayers/Interaction",
    "Utilities/OpenLayers/proj4",
    "dojo/Evented"],
    function (OLDefaults, Interaction, Proj4, Evented) {

        Proj4.defs("EPSG:7932", "+proj=tmerc +lat_0=0 +lon_0=30 +k=1 +x_0=500000 +y_0=0 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs");
        proj4.defs('EPSG:3395', '+proj=merc +lon_0=0 +k=1 +x_0=0 +y_0=0 +datum=WGS84 +units=m +no_defs');
        ol.proj.get('EPSG:3395').setExtent([-20037508.342789244, -20037508.342789244, 20037508.342789244, 20037508.342789244]);
        return dojo.declare([Evented], {

            DragInteraction: null,
            // Private Fields
            _mapEventList: ["moveend", "pointermove"],
            _mapIdentifier: null,
            _interactionList: [],

            // Private Functions
            _initEvents: function (map) {
                var __selfMap = this;
                for (var i = 0; i < __selfMap._mapEventList.length; i++) {
                    map.on(__selfMap._mapEventList[i], function (args) {
                        __selfMap.emit(args.type, args);
                    });
                }
            },
            _createMap: function (options) {
                var opts = $.extend({}, OLDefaults.OLMap, options);
                this.Map = new ol.Map({
                    pixelRatio: 1,
                    view: this._createView(opts.View),
                    layers: opts.Layers,
                    target: opts.Target,
                    controls: opts.Controls,
                    interactions: ol.interaction.defaults().extend([opts.Interactions])
                });
                return this.Map;
            },
            _createView: function (options) {
                var opts = $.extend({}, OLDefaults.OLView, options);
                var result = new ol.View({
                    center: opts.Center,
                    projection: opts.Projection,
                    rotation: opts.Rotation,
                    zoom: opts.Zoom
                });
                return result;
            },

            constructor: function (options) {
                options.Target = this.GetMapCanvas(options.Target);
                options.Controls = this.CreateControls(options.Controls);

                var map = this._createMap(options);
                this._mapIdentifier = options._mapIdentifier;
                this._initEvents(map);
            },

            ChangeBaseLayer: function (options) {
                var opts = $.extend({}, OLDefaults.OLBaseLayer, options);
                var BaseMapList = [
                    { 'Id': 'NONE', 'Source': null },
                    { 'Id': 'GR', 'Source': this.CreateXYZSource({ CrossOrigin: 'Anonymous', Url: 'http://mts{0-3}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}' }) },
                    { 'Id': 'GS', 'Source': this.CreateXYZSource({ CrossOrigin: 'Anonymous', Url: 'http://mts{0-3}.google.com/vt/lyrs=y&x={x}&y={y}&z={z}' }) },
                    { 'Id': 'GP', 'Source': this.CreateXYZSource({ CrossOrigin: 'Anonymous', Url: 'http://mts{0-3}.google.com/vt/lyrs=p&x={x}&y={y}&z={z}' }) },
                    { 'Id': 'BR', 'Source': this.CreateBingSource({ CrossOrigin: 'Anonymous', ImagerySet: 'Road' }) },
                    { 'Id': 'BS', 'Source': this.CreateBingSource({ CrossOrigin: 'Anonymous', ImagerySet: 'Aerial' }) },
                    { 'Id': 'BP', 'Source': this.CreateBingSource({ CrossOrigin: 'Anonymous', ImagerySet: 'AerialWithLabels' }) },
                    { 'Id': 'OR', 'Source': this.CreateOSMSource({ CrossOrigin: 'Anonymous', Url: 'http://{a-c}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png' }) },
                    { 'Id': 'OS', 'Source': this.CreateOSMSource({ CrossOrigin: 'Anonymous', Url: 'http://{a-c}.tile.opencyclemap.org/cycle/{z}/{x}/{y}.png' }) },
                    { 'Id': 'OP', 'Source': this.CreateOSMSource({ CrossOrigin: 'Anonymous', Url: 'http://otile1-s.mqcdn.com/tiles/1.0.0/osm/{z}/{x}/{y}.png' }) },
                    { 'Id': 'BM', 'Source': this.CreateXYZSource({ CrossOrigin: 'Anonymous', Url: 'http://bms.basarsoft.com.tr/Service/api/v1/map/Default?accId=GL8VCF9E5k2AkIroN5fC6w&appCode=qEw539H1eUWYiRXO6LhB1w&x={x}&y={y}&z={z}' }) },
                    { 'Id': 'YM', 'Source': this.CreateYandexSource({ CrossOrigin: 'Anonymous', Url: 'http://vec0{1-4}.maps.yandex.net/tiles?l=map&x={x}&y={y}&z={z}&lang=tr_TR' }) },
                    { 'Id': 'YU', 'Source': this.CreateYandexSource({ CrossOrigin: 'Anonymous', Url: 'http://sat0{1-4}.maps.yandex.net/tiles?l=sat&x={x}&y={y}&z={z}&lang=tr_TR' }) },
                    { 'Id': 'YH', 'Source': this.CreateYandexSource({ CrossOrigin: 'Anonymous', Url: 'http://vec0{1-4}.maps.yandex.net/tiles?l=skl&x={x}&y={y}&z={z}&lang=tr_TR' }) },

                    //{ 'Id': 'BM', 'Source': this.CreateXYZSource({ CrossOrigin: 'Anonymous', Url: 'http://bms.uatisbank/Service/api/v1/map/Default?accId=0&appCode=0&x={x}&y={y}&z={z}' }) }
                ];

                var BaseMapLayer = this.FindLayer({ Title: opts.LayerTitle, Name: opts.LayerName });

                if (BaseMapLayer) {
                    var Source = $.grep(BaseMapList, function (e) { return e.Id == opts.BaseMapName; })[0]['Source'];

                    if (['BR', 'BS', 'BP'].indexOf(opts.BaseMapName) >= 0) {
                        var prop = BaseMapLayer.getProperties();
                        prop.visible = true;
                        prop.preload = Infinity;
                        prop.transitionEffect = null;
                    } else {
                        BaseMapLayer.setProperties(null);
                    }

                    BaseMapLayer.setSource(Source);

                    if (Source != null) {

                        var tile_loading = 0, tile_loaded = 0, run = 0;
                        Source.on('tileloadstart', function () {
                            ++tile_loading;
                        });
                        Source.on('tileloadend', function () {
                            ++tile_loaded;
                            if (tile_loaded == tile_loading) {
                                if (run <= 0) {
                                    run++;

                                }
                            }
                        });
                    }
                }
            },
            GetMapCanvas: function (options) {
                var opts = $.extend({}, OLDefaults.OLMapCanvas, options);
                return document.getElementById(opts.CanvasId);
            },
            CreateControls: function (options) {
                var opts = $.extend({}, OLDefaults.OLControl, options);

                var result = new ol.control.defaults({
                    attribution: opts.Attribution,
                    rotate: opts.Rotate,
                    zoom: opts.Zoom
                });

                return result;
            },
            CreateStyle: function (options) {
                var opts = $.extend({}, OLDefaults.OLStyle, options);
                var result = new ol.style.Style({
                    image: opts.Image,
                    fill: opts.Fill,
                    stroke: opts.Stroke,
                    text: opts.Text,

                });
                return result;
            },
            CreateStyleIcon: function (options) {

                var result = new ol.style.Icon({
                    anchor: options.anchor,
                    anchorXUnits: options.anchorXUnits,
                    anchorYUnits: options.anchorYUnits,
                    opacity: options.opacity,
                    src: options.src

                });
                return result;
            },
            CreateStyleText: function (options) {

                var result = new ol.style.Text({
                    font: options.font,
                    fill: new ol.style.Fill({ color: '#000' }),
                    stroke: new ol.style.Stroke({
                        color: '#fff', width: 5
                    }),
                    text: options.text
                });
                return result;
            },
            CreateStyleFill: function (options) {

                var result = new ol.style.Fill({ color: options.color });

                return result;
            },
            CreateStyleStroke: function (options) {

                var result = new ol.style.Stroke({ color: options.color, width: options.width });

                return result;
            },

            CreateVectorSource: function (options) {
                var opts = $.extend({}, OLDefaults.OLVectorSource, options);
                var result = new ol.source.Vector({
                    format: opts.Format,
                    loader: opts.Loader,
                    projection: opts.Projection
                });
                return result;
            },
            CreateTileSource: function (options) {
                var opts = $.extend({}, OLDefaults.OLTileWMSSource, options);
                var result = new ol.source.TileWMS({
                    url: opts.Url,
                    crossOrigin: opts.CrossOrigin,
                    params: opts.Params,
                    format: opts.Format,
                    width: 256,
                    height: 256,
                    gutter: opts.Gutter,
                    serverType: 'geoserver',
                    tileLoadFunction: opts.TileLoadFunction
                });
                return result;
            },
            CreateImageSource: function (options) {
                var opts = $.extend({}, OLDefaults.OLTileWMSSource, options);
                var result = new ol.source.ImageWMS({
                    url: opts.Url,
                    crossOrigin: opts.CrossOrigin,
                    params: opts.Params,
                    format: opts.Format,
                    gutter: opts.Gutter,
                    serverType: 'geoserver',
                    imageLoadFunction: opts.ImageLoadFunction
                });
                return result;
            },
            CreateXYZSource: function (options) {
                var opts = $.extend({}, OLDefaults.OLXYZSource, options);
                var result = new ol.source.XYZ({
                    url: opts.Url,
                    crossOrigin: opts.CrossOrigin,
                    tileLoadFunction: opts.TileLoadFunction
                });
                return result;
            },
            CreateYandexSource: function (options) {
                var opts = $.extend({}, OLDefaults.OLXYZSource, options);
                var result = new ol.source.XYZ({
                    url: opts.Url,
                    projection: 'EPSG:3395',
                    crossOrigin: opts.CrossOrigin


                });

                return result;

            },
            CreateBingSource: function (options) {
                var opts = $.extend({}, OLDefaults.OLBingSource, options);
                var result = new ol.source.BingMaps({
                    crossOrigin: opts.CrossOrigin,
                    key: opts.Key,
                    imagerySet: opts.ImagerySet
                });
                return result;
            },
            CreateOSMSource: function (options) {
                var opts = $.extend({}, OLDefaults.OLOSMSource, options);
                var result = new ol.source.OSM({
                    url: opts.Url,
                    crossOrigin: opts.CrossOrigin
                });
                return result;
            },
            ClearLayerSource: function (options) {
                this.GetLayerSource({ Title: options.Title }).clear();
            },
            SetLayerSourceNull: function (options) {
                this.FindLayer({ Title: options.Title }).setSource();
            },
            CreateVectorLayer: function (options) {
                var opts = $.extend({}, OLDefaults.OLVectorLayer, options);
                var result = new ol.layer.Vector({
                    source: opts.Source,
                    style: opts.Style,
                    //style: new ol.style.Style({
                    //    fill: new ol.style.Fill({
                    //        color: 'rgba(255, 255, 255, 0.2)'
                    //    }),
                    //    stroke: new ol.style.Stroke({
                    //        color: '#0D47A1',
                    //        width: 3
                    //    })
                    //}),

                    title: opts.Title,
                    name: opts.Name,
                    opacity: opts.Opacity
                });
                return result;
            },
            CreateTileLayer: function (options) {
                var opts = $.extend({}, OLDefaults.OLTileLayer, options);
                var result = new ol.layer.Tile({
                    title: opts.Title,
                    source: opts.Source,
                    name: opts.Name
                });
                return result;
            },
            CreateImageLayer: function (options) {
                var opts = $.extend({}, OLDefaults.OLTileLayer, options);
                var result = new ol.layer.Image({
                    title: opts.Title,
                    source: opts.Source,
                    name: opts.Name
                });
                return result;
            },
            CreateWKTFormat: function () {
                return new ol.format.WKT();
            },
            CreateKMLFormat: function (options) {
                return new ol.format.KML(options);
            },

            GetMap: function () {
                return this.Map;
            },
            GetView: function () {
                return this.Map.getView();
            },
            GetAllLayers: function () {
                var result = [];
                result = this.GetMap().getLayers();
                return result;
            },
            GetAllLayersTitles: function () {
                var result = [];
                $.each(this.GetMap().getLayers(), function (index, item) {
                    result.push(item.title);
                });
                return result;
            },
            FindLayer: function (options) {
                var opts = $.extend({}, OLDefaults.OLFindLayer, options);
                var searchByTitle = true; var result;

                if (opts.Name != undefined && opts.Name != '') { searchByTitle = false; }

                this.GetAllLayers().forEach(function (l) {
                    if (searchByTitle) {
                        if (l.get('title') == opts.Title) {
                            result = l;
                            return false;
                        }
                    } else {
                        if (l.get('name') == opts.Name) {
                            result = l;
                            return false;
                        }
                    }
                });

                if (!searchByTitle && result == undefined) {
                    this.GetAllLayers().forEach(function (l) {
                        if (l.get('title') == opts.Title) {
                            result = l;
                        }
                    });
                }

                return result;
            },

            ClearLayers: function (options) {
                var _self = this;
                var opts = $.extend({}, OLDefaults.OLClearLayer, options);
                $.each(opts.TitleList, function (index, item) {
                    var layer = _self.FindLayer({ Title: item });
                    var source = layer == null ? null : layer.getSource();
                    var clear = source == null ? null : source.clear();
                });
            },
            ClearAllLayer: function () {
                $.each(GetAllLayers(), function (index, item) {
                    item.getSource().clear();
                });
            },
            GetLayerSource: function (options) {
                var opts = $.extend({}, OLDefaults.OLGetLayerSource, options);
                return this.FindLayer({ Title: opts.Title, Name: opts.Name }).getSource();
            },
            UpdateLayerSource: function (options) {
                var opts = $.extend({}, OLDefaults.OLUpdateLayerSource, options);
                this.FindLayer({ Title: opts.Title, Name: opts.Name }).setSource(opts.Source);
                this.GetMap().render();
            },
            UpdateLayerSourceParams: function (options) {
                var opts = $.extend({}, OLDefaults.OLUpdateLayerSourceParams, options);
                this.FindLayer({ Title: opts.Title, Name: opts.Name }).getSource().updateParams(opts.Params);
                this.GetMap().render();
            },
            RefreshMap: function () {
                this.GetMap().render();
            },
            RefreshLayer: function (options) {
                var source = this.GetLayerSource(options);
                var params = source.getParams();
                params.t = new Date().getMilliseconds();
                source.updateParams(params);
            },

            AddLayer: function (layer) {
                this.GetMap().addLayer(layer);
            },
            AddLayers: function (layers) {
                for (var i = 0; i < layers.length; i++) {
                    this.AddLayer(layers[i]);
                }
            },
            GetLayerExtent: function (options) {
                return this.GetLayerSource({ Title: options.Title }).getExtent();
            },

            CreateFeature: function (options) {

                return new ol.Feature({

                    name: options.Name,
                    geometry: options.Geometry,
                    style: options.Style

                });
            },
            GetFeatureCount: function (options, onComplate) {

                var result = this.GetLayerSource({ Title: options.Title }).getFeatures().length;

                if (onComplate) {
                    onComplate(result);
                } else {
                    return result;
                }
            },
            RemoveLayer: function (layer) {
                this.GetMap().removeLayer(layer);
            },
            RemoveFeatureByID: function (options, onComplate) {
                var features = this.GetLayerSource({ Title: options.Title, Name: options.Name }).getFeatures();
                var self = this;
                $.each(features, function (index, item) {
                    var fId = self.GetFeatureID({ Feature: item });
                    if (fId === options.FeatureID) {
                        self.RemoveFeature({ Title: options.Title, Name: options.Name, Feature: item });
                        return false;
                    }
                });

                if (onComplate) {
                    onComplate();
                }
            },
            RemoveFeature: function (options) {
                var layerSource = this.GetLayerSource({ Title: options.Title, Name: options.Name });
                if (layerSource != null) {
                    layerSource.removeFeature(options.Feature);
                }
            },
            RemoveFeatureByGeometry: function (options) {
                var deletedRecords = 0;
                var format = new ol.format.WKT();
                var layerSource = this.GetLayerSource({ Title: options.Title });
                var layerFeatures = layerSource.getFeatures();

                if (typeof options.Geometry === 'string') {
                    options.Geometry = format.readGeometry(options.Geometry, ['EPSG', options.DataEPSG].join(':'), ['EPSG', options.FeatureEPSG].join(':'));
                } else {
                    options.Geometry = this.ChangeGeometryProjection({ Geometry: options.Geometry, DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG });
                }

                $.each(layerFeatures, function (index, item) {
                    if (item.getGeometry() == options.Geometry) {
                        layerSource.removeFeatures(item);
                        deletedRecords++;
                    }
                });

                return deletedRecords;
            },
            FindFeatureById: function (options, onComplate) {
                var result = undefined;

                var features = this.GetLayerSource({ Title: options.Title, Name: options.Name }).getFeatures();
                var self = this;

                $.each(features, function (index, item) {
                    var fId = self.GetFeatureID({ Feature: item });
                    if (fId === options.FeatureID) {
                        result = item;
                        return false;
                    }
                });

                if (onComplate) {
                    onComplate(result);
                } else {
                    return result;
                }
            },
            GetFeatureID: function (options, onComplate) {

                var result = options.Feature.getProperties().id;

                if (onComplate) {
                    onComplate(result);
                } else {
                    return result;
                }
            },
            SetFeatureID: function (options, onComplate) {

                options.Feature.setProperties({ 'id': options.FeatureID });

                if (onComplate) {
                    onComplate(options.Feature);
                } else {
                    return options.Feature;
                }

            },
            SetLayerIndex: function (options) {
                this.FindLayer({ Title: options.Title, Name: options.Name }).setZIndex(options.ZIndex);
            },
            GetFeatureExtent: function (options, onComplate) {
                return options.Feature.getGeometry().getExtent();
            },
            ViewLayerExtent: function (options, onComplate) {
                var Layer = this.FindLayer({ Title: options.Title });
                if (Layer.getSource().getFeatures().length > 0) {

                    this.GetView().fit(this.GetLayerExtent({ Title: options.Title }), this.GetMap().getSize());
                    if (options.ZoomLevel != undefined && options.ZoomLevel != null) {

                        this.SetZoomLevel({ ZoomLevel: options.ZoomLevel });
                    }

                }
                if (onComplate) {
                    onComplate();
                }
            },
            ViewFeatureExtent: function (options, onComplate) {

                if (options.Feature.getGeometry().getType() === 'Point') {
                    this.SetCenterPoint({ CenterPoint: this.GetGeometryCoordinates({ Geometry: options.Feature.getGeometry() }) });
                    this.SetZoomLevel({ ZoomLevel: (options.ZoomLevel == undefined || options.ZoomLevel != null) ? 16 : options.ZoomLevel });
                } else {
                    this.GetView().fit(this.GetFeatureExtent({ Feature: options.Feature }), this.GetMap().getSize());
                }

                if (onComplate) {
                    onComplate();
                }
            },
            SetLayerOpacity: function (options) {
                this.FindLayer({ Title: options.Title, Name: options.Name }).setOpacity(options.OpacityValue / 100);
            },
            UpdateSize: function () {
                this.GetMap().updateSize();
            },
            AddControl: function (options) {
                this.GetMap().addControl(options.Control);
            },
            CreateMousePositionControl: function (options) {
                return new ol.control.MousePosition({
                    coordinateFormat: function (coordinate) {
                        return ol.coordinate.format(coordinate, '{y} - {x}', 6);
                    },
                    projection: options.Projection,
                    target: document.getElementById(options.TargetElementID),
                    undefinedHTML: options.EmptyString
                });
            },
            AddFeature: function (options) {
                var opts = $.extend({}, OLDefaults.OLAddFeature, options);
                this.GetLayerSource({ Title: opts.Title }).addFeature(opts.Feature);
            },
            CreateGeoJSONFormat: function () {
                return new ol.format.GeoJSON();
            },
            AddGeoJsonFeatureToLayer: function (options) {
                var opts = $.extend({}, OLDefaults.OLAddFeature, options);

                var Format = this.CreateGeoJSONFormat();
                var Feature = Format.readFeature(opts.GeoJSONString, {
                    dataProjection: opts.DataProjection,
                    featureProjection: opts.FeatureProjection
                });

                var Layer = this.FindLayer({ Title: opts.LayerTitle });
                Layer.getSource().clear();
                Layer.getSource().addFeature(Feature);
                this.GetMap().getView().fit(Layer.getSource().getExtent(), this.GetMap().getSize());
            },
            GetFeatureFromGeoJson: function (options) {
                var opts = $.extend({}, OLDefaults.OLAddFeature, options);
                if (options.GeoJSONString) {
                    var Format = this.CreateGeoJSONFormat();
                    var Feature = Format.readFeature(opts.GeoJSONString, {
                        dataProjection: opts.DataProjection,
                        featureProjection: opts.FeatureProjection
                    });

                    return Feature;
                }
            },
            GetMapExtent: function (options, onComplate) {
                var extent = this.GetView().calculateExtent(this.GetMap().getSize());
                //if (options.DataEPSG != undefined && options.FeatureEPSG != undefined) {
                //    extent = ol.proj.transformExtent(extent, ['EPSG', options.FeatureEPSG].join(':'), ['EPSG', options.DataEPSG].join(':'));
                //}

                if (onComplate) {
                    onComplate(extent);
                } else {
                    return extent;
                }
            },
            GetProjection: function () {
                return this.GetView().getProjection();
            },
            GetScale: function () {
                return this.GetResolution() * ol.proj.METERS_PER_UNIT[this.GetProjection().getUnits()] * 39.37 * (25.4 / 0.28);
            },
            SetScale: function (options) {
                var opts = $.extend({}, OLDefaults.OLScale, options);
                this.SetResolution({ ResolutionValue: (opts.ScaleValue / (ol.proj.METERS_PER_UNIT[this.GetProjection().getUnits()] * 39.37 * (25.4 / 0.28))) });
                return true;
            },
            GetResolution: function (options) {
                var opts = $.extend({}, OLDefaults.OLResolution, options);
                return this.GetView().getResolution();
            },
            SetResolution: function (options) {
                var opts = $.extend({}, OLDefaults.OLResolution, options);
                this.GetView().setResolution(opts.ResolutionValue);
            },
            SetLayerMinResolution: function (options) {
                this.FindLayer({ Title: options.Title, Name: options.Name }).setMinResolution(this.GetResolutionFromZoom({ ZoomLevel: options.MinResolutionValue }));
            },
            SetLayerMaxResolution: function (options) {
                this.FindLayer({ Title: options.Title, Name: options.Name }).setMaxResolution(this.GetResolutionFromZoom({ ZoomLevel: options.MaxResolutionValue }));
            },
            GetZoomLevel: function (options) {
                var opts = $.extend({}, OLDefaults.OLZoom, options);
                return this.GetView().getZoom();
            },
            SetZoomLevel: function (options) {
                var opts = $.extend({}, OLDefaults.OLZoom, options);
                this.GetView().setZoom(opts.ZoomLevel);
                return true;
            },
            GetRotationDegree: function (options) {
                var opts = $.extend({}, OLDefaults.OLRotation, options);
                return this.GetView().getRotation();
            },
            SetRotationDegree: function (options) {
                var opts = $.extend({}, OLDefaults.OLRotation, options);
                this.GetView().setRotation(opts.RotationDegree);
                return true;
            },
            GetCenterPoint: function (options) {

                var opts = $.extend({}, OLDefaults.OLCenter, options);
                return this.GetView().getCenter();
            },
            SetCenterPoint: function (options) {
                var opts = $.extend({}, OLDefaults.OLCenter, options);
                this.GetView().setCenter(opts.CenterPoint);
                return true;
            },
            GetZoomFromResolution: function (options) {
                var opts = $.extend({}, OLDefaults.OLResolution, options);
                return Math.log(156543.04 / opts.ResolutionValue) / Math.log(2);
            },
            GetResolutionFromZoom: function (options) {
                var opts = $.extend({}, OLDefaults.OLZoom, options);
                return 156543.04 / Math.pow(2, opts.ZoomLevel);
            },
            AddOverlay: function (options) {
                var opts = $.extend({}, OLDefaults.OLOverlay, options);
                return this.GetMap().addOverlay(opts.Overlay);
            },
            RemoveMapEvent: function (options) {
                var opts = $.extend({}, OLDefaults.OLMapEvent, options);
            },
            GetInteractionList: function () {
                return this.GetMap().getInteractions();
            },
            AddInteraction: function (options) {
                this._interactionList.push(options);
                if (options._interactionElement != undefined) {
                    this.GetMap().addInteraction(options._interactionElement);
                }
                else {
                    this.GetMap().addInteraction(options);
                }
            },

            CreatePoint: function (options) {
                return new ol.geom.Point(ol.proj.transform(options.Coords, options.FeatureProjection, options.DataProjection));
            },
            CreateOverlay: function (options) {
                return new ol.Overlay(options);
            },
            CreateDrawInteraction: function (name, title, type, optional) {
                var options = { type: (type) };

                options.source = this.GetLayerSource({ Title: title });

                $.each(optional, function (key, value) {
                    options[key] = value;
                });

                var interaction = new Interaction(name, "Draw", options);
                //this.SetActiveInteraction(false, interaction.GetName());
                return interaction;
            },


            CreateDragInteraction: function (_layerName) {

                if (this.DragInteraction == null) {
                    var dragFeature = null;
                    var dragCoordinate = null;
                    var dragCursor = 'move';
                    var dragPrevCursor = null;

                    var _process = null;

                    var _self = this;
                    this.DragInteraction = new ol.interaction.Pointer({
                        handleDownEvent: function (event) {
                            var feature = _self.GetMap().forEachFeatureAtPixel(event.pixel,
                                function (feature, layer) {

                                    if (layer.getProperties().title == _layerName) {
                                        _process = true;
                                        return feature;
                                    }

                                    _process = null;

                                }
                            );

                            if (feature) {
                                dragCoordinate = event.coordinate;
                                dragFeature = feature;
                                return true;
                            }

                            return false;
                        },
                        handleDragEvent: function (event) {

                            if (_process) {
                                var deltaX = event.coordinate[0] - dragCoordinate[0];
                                var deltaY = event.coordinate[1] - dragCoordinate[1];

                                var geometry = dragFeature.getGeometry();
                                geometry.translate(deltaX, deltaY);


                                dragCoordinate[0] = event.coordinate[0];
                                dragCoordinate[1] = event.coordinate[1];
                            }

                        },
                        handleMoveEvent: function (event) {
                            if (dragCursor) {
                                var map = event.map;

                                var feature = map.forEachFeatureAtPixel(event.pixel,
                                    function (feature, layer) {
                                        if (layer.getProperties().title == _layerName) {
                                            _process = true;
                                            return feature;
                                        }

                                        _process = null;
                                    });

                                if (_process) {
                                    var element = event.map.getTargetElement();
                                    var _currentCursor = $(element).
                                        find('canvas')[0].style.cursor;

                                    if (feature) {
                                        if (_currentCursor != dragCursor) {
                                            dragPrevCursor = _currentCursor;

                                            $(element).
                                                find('canvas')[0].style.cursor = dragCursor;
                                        }
                                    } else if (dragPrevCursor !== undefined) {
                                        $(element).
                                            find('canvas')[0].style.cursor = dragPrevCursor;
                                        dragPrevCursor = undefined;
                                    }
                                }

                            }
                        },
                        handleUpEvent: function (event) {
                            if (_process) {
                                dragCoordinate = null;
                                dragFeature = null;
                                return false;
                            }
                        }
                    });
                    _self.GetMap().addInteraction(this.DragInteraction);
                }
            },

            ActiveDragInteraction: function (_isActive) {
                if (this.DragInteraction != null) {
                    this.DragInteraction.setActive(_isActive);
                }
            },

            CreateDragAndDropInteraction: function (name, title, options) {
                var options = {
                    formatConstructors: [
                        ol.format.GPX,
                        ol.format.GeoJSON,
                        ol.format.IGC,
                        ol.format.KML,
                        ol.format.TopoJSON
                    ]
                };

                var interaction = new Interaction(name, "DragAndDrop", options);
                return interaction;
            },


            CreatePointer: function () {
                return new ol.interaction.Pointer();
            },

            GetActiveInteraction: function () {
                var activeInteraction = null;
                if (this._interactionList) {
                    this._interactionList.forEach(function (interaction) {
                        if (interaction.GetActive()) {
                            activeInteraction = interaction;
                            return false;
                        }
                    });
                }
                return activeInteraction;
            },
            GetInteractionByName: function (name) {
                var result = null;
                this._interactionList.forEach(function (interaction) {
                    if (interaction.GetName() === name) {
                        result = interaction;
                    }
                });

                return result;
            },
            SetActiveInteraction: function (isActive, interactionName) {
                var interaction = this.GetInteractionByName(interactionName);
                try {
                    setTimeout(function () {
                        interaction.SetActive(isActive);
                    }, 10);
                } catch (e) {
                    console.log(e);
                }
            },
            SetActiveAllInteractions: function (isActive) {
                this._interactionList.forEach(function (interaction) {
                    setTimeout(function () { interaction.SetActive(isActive); }, 10);
                });

                this.ActiveDragInteraction(isActive);
            },
            SetFeatureGeometry: function (options) {
                var tempFeature = options.Feature;
                tempFeature.getGeometry().setCoordinates(options.Geometry.getCoordinates());
                return tempFeature;
                //return options.Feature.setGeometry(options.Geometry);
            },
            SetGeometryCoordinates: function (options) {
                return options.Geometry.setCoordinates(options.Coordinates);
            },
            GetFeatureGeometries: function (options) {
                var geomArray = [];
                $.each(options.Feature, function (index, item) {
                    geomArray.push(item.getGeometry());
                });
                return geomArray;
            },
            GetWKTFromFeature: function (options, onComplate) {
                if (options.Feature) {
                    var format = new ol.format.WKT();
                    var result = format.writeGeometry(options.Feature.getGeometry(),
                        {
                            dataProjection: options.DataProjection,
                            featureProjection: options.FeatureProjection
                        });

                    if (onComplate) {
                        onComplate(result);
                    } else {
                        return result;
                    }
                }
            },
            GetFeatureFromWKT: function (options, onComplate) {

                var opts = $.extend({}, OLDefaults.OLAddFeature, options);
                if (opts.WKTString) {
                    var Format = this.CreateWKTFormat();
                    var Feature = Format.readFeature(opts.WKTString, {
                        dataProjection: opts.DataProjection,
                        featureProjection: opts.FeatureProjection
                    });

                    if (onComplate) {
                        onComplate(Feature);
                    } else {
                        return Feature;
                    }
                }
            },
            GetFeatureFromKML: function (options, onComplate) {
                var result = new ol.format.KML({
                    extractAttributes: true
                }).readFeatures(options.KMLText, {
                    featureProjection: options.FeatureProjection
                });

                if (onComplate) {
                    onComplate(result);
                } else {
                    return result;
                }
            },
            GetFeatureInfoUrl: function (options) {
                //return this.GetLayerSource({ Title: options.LayerName }).getGetFeatureInfoUrl(options.Coordinates, this.GetResolution(), this.GetProjection(), {
                //    'INFO_FORMAT': 'application/json',
                //    'PIXELRADIUS': options.PixelRadius,
                //    'FEATURE_COUNT': options.FeatureCount
                //});

                var s = this.GetLayerSource({ Title: options.LayerName }).getGetFeatureInfoUrl(options.Coordinates, this.GetResolution(), this.GetProjection(), {
                    'INFO_FORMAT': 'application/json',
                    'PIXELRADIUS': options.PixelRadius,
                    'FEATURE_COUNT': options.FeatureCount
                });
                return s;
            },
            GetMapProjection: function () {
                return this.GetView().getProjection();
            },
            ChangeFeatureProjection: function (options) {

            },
            CreatePointBuffer: function (options, onComplate) {
                var EPSGGeom;
                var pointCoord = options.Feature.getGeometry().getCoordinates();
                var feature = new ol.Feature({
                    geometry: new ol.geom.Circle(pointCoord, (ol.proj.METERS_PER_UNIT[this.GetView().getProjection().getUnits()] * parseInt(options.Radius)))
                });
                var point = options.Feature.getGeometry();
                var circlePoint = feature.getGeometry();

                var newLowPoly = ol.geom.Polygon.fromCircle(circlePoint, 96);

                EPSGGeom = this.ChangeGeometryProjection({ Geometry: newLowPoly, DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG });

                var newFeature = new ol.Feature({
                    geometry: newLowPoly,
                    name: options.FeatureName,
                    id: MapManager.Map.GetFeatureID({ Feature: options.Feature })
                });

                this.AddFeature({ Title: options.Title, Feature: newFeature });
                var format = new ol.format.WKT({
                    dataProjection: ['EPSG', options.DataEPSG].join(':'),
                    featureProjection: ['EPSG', options.FeatureEPSG].join(':')
                });

                var result = {
                    WKTString: format.writeGeometry(EPSGGeom),
                    PointWKT: format.writeGeometry(this.ChangeGeometryProjection({ Geometry: point, DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG }))
                };

                if (onComplate) {
                    onComplate(result);
                } else {
                    return result;
                }

            },
            CreateLineBuffer: function (options, onComplate) {
                var EPSGGeom;
                if (options.Feature) {
                    var parser = new jsts.io.OL3Parser();
                    var feature = options.Feature;
                    var line = feature.getGeometry();
                    var jstsGeom = parser.read(feature.getGeometry());
                    var buffered = jstsGeom.buffer(options.Radius);
                    feature.setGeometry(parser.write(buffered));

                    EPSGGeom = this.ChangeGeometryProjection({ Geometry: feature.getGeometry(), DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG });

                    this.AddFeature({ Title: options.Title, Feature: feature });
                    var format = new ol.format.WKT();
                    var result = {
                        WKTString: format.writeGeometry(EPSGGeom),
                        LineWKT: format.writeGeometry(this.ChangeGeometryProjection({ Geometry: line, DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG }))
                    };

                    if (onComplate) {
                        onComplate(result);
                    } else {
                        return result;
                    }
                }
            },
            CreatePolygonBuffer: function (options, onComplate) {
                var EPSGGeom;
                if (options.Feature) {

                    EPSGGeom = this.ChangeGeometryProjection({ Geometry: options.Feature.getGeometry(), DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG });

                    this.AddFeature({ Title: options.Title, Feature: options.Feature });
                    var format = new ol.format.WKT();
                    var result = {
                        WKTString: format.writeGeometry(EPSGGeom),
                        PolygonWKT: format.writeGeometry(EPSGGeom)
                    };
                    if (onComplate) {
                        onComplate(result);
                    } else {
                        return result;
                    }
                }
            },
            CreateCircleBuffer: function (options, onComplate) {
                var EPSGGeom;
                if (options.Feature) {

                    var circle = options.Feature.getGeometry();

                    var newLowPoly = ol.geom.Polygon.fromCircle(circle, 96);
                    var newFeature = new ol.Feature({
                        geometry: newLowPoly,
                        name: options.FeatureName,
                        id: MapManager.Map.GetFeatureID({ Feature: options.Feature })
                    });

                    EPSGGeom = this.ChangeGeometryProjection({ Geometry: newLowPoly, DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG });

                    this.AddFeature({ Title: options.Title, Feature: newFeature });
                    var format = new ol.format.WKT();
                    var result = format.writeGeometry(EPSGGeom);

                    if (onComplate) {
                        onComplate(result);
                    } else {
                        return result;
                    }
                }
            },
            CreateBoxBuffer: function (options, onComplate) {
                var EPSGGeom;
                if (options.Feature) {
                    var format = new ol.format.WKT();
                    EPSGGeom = this.ChangeGeometryProjection({ Geometry: options.Feature.getGeometry(), DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG });
                    var result = format.writeGeometry(EPSGGeom);

                    if (onComplate) {
                        onComplate(result);
                    } else {
                        return result;
                    }
                }
            },
            CreateExtentBuffer: function (options, onComplate) {
                var EPSGGeom;
                if (options.Extent) {
                    var extentGeom = new ol.geom.Polygon.fromExtent(options.Extent);
                    var geoJsonFormatter = new ol.format.GeoJSON();
                    var geoJson = geoJsonFormatter.writeGeometry(extentGeom, { rightHanded: true });
                    var rightHandCorrectedFeature = geoJsonFormatter.readGeometry(geoJson);
                    var format = new ol.format.WKT();

                    EPSGGeom = this.ChangeGeometryProjection({ Geometry: rightHandCorrectedFeature, DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG });
                    var result = format.writeGeometry(EPSGGeom);

                    if (onComplate) {
                        onComplate(result);
                    } else {
                        return result;
                    }
                }
            },
            ChangeGeometryProjection: function (options) {
                var newGeom;
                var newCoordinateList = [];
                var oldCoordinateList;
                var cloneGeometry = options.Geometry.clone();
                var geom;

                if (options.FeatureEPSG) {
                    switch (cloneGeometry.getType()) {
                        case 'Point': geom = cloneGeometry.transform(['EPSG', options.DataEPSG].join(':'), ['EPSG', options.FeatureEPSG].join(':')); /* oldCoordinateList = options.Geometry.getCoordinates();*/ break;
                        case 'LineString': geom = cloneGeometry.transform(['EPSG', options.DataEPSG].join(':'), ['EPSG', options.FeatureEPSG].join(':'));/* oldCoordinateList = options.Geometry.getCoordinates();*/ break;
                        case 'Polygon': geom = cloneGeometry.transform(['EPSG', options.DataEPSG].join(':'), ['EPSG', options.FeatureEPSG].join(':'));/* oldCoordinateList = options.Geometry.getCoordinates()[0];*/ break;
                        case 'Circle': geom = this.CreatePoint({ Coords: cloneGeometry.getCenter(), DataProjection: ['EPSG', options.DataEPSG].join(':'), FeatureProjection: ['EPSG', options.FeatureEPSG].join(':') }); break;
                        default: geom = cloneGeometry.transform(['EPSG', options.DataEPSG].join(':'), ['EPSG', options.FeatureEPSG].join(':'));/* oldCoordinateList = options.Geometry.getCoordinates()[0];*/
                    }
                    if (newGeom) {
                        geom = newGeom;
                    }
                    //if (cloneGeometry.getType() == 'Point') {
                    //    var c = ol.proj.transform(oldCoordinateList, ['EPSG', options.DataEPSG].join(':'), ['EPSG', options.FeatureEPSG].join(':'));
                    //    newCoordinateList.push(c);
                    //    cloneGeometry.setCoordinates(newCoordinateList);
                    //} else {
                    //    $.each(oldCoordinateList, function (index, item) {
                    //        var c = ol.proj.transform(item, ['EPSG', options.DataEPSG].join(':'), ['EPSG', options.FeatureEPSG].join(':'));
                    //        newCoordinateList.push(c);
                    //    });

                    //    if (newGeom) {
                    //        cloneGeometry = newGeom;
                    //    } else {
                    //        cloneGeometry.setCoordinates([newCoordinateList]);
                    //    }
                    //}
                }

                return geom;
            },
            GetGeometryCoordinates: function (options, onComplate) {
                var result;

                if (options.DataEPSG) {
                    if (options.Geometry.getType() === 'Point') {
                        result = this.ChangeGeometryProjection({ Geometry: options.Geometry, DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG }).getCoordinates();
                    } else {
                        result = this.ChangeGeometryProjection({ Geometry: options.Geometry, DataEPSG: options.DataEPSG, FeatureEPSG: options.FeatureEPSG }).getCoordinates()[0];
                    }

                } else {
                    if (options.Geometry.getType() === 'Point') {
                        result = options.Geometry.getCoordinates();
                    } else {
                        result = options.Geometry.getCoordinates()[0];
                    }
                }
                return result;
            },
            GetGeometryFromFeature: function (options, onComplate) {
                var result = options.Feature.getGeometry();

                if (onComplate) {
                    onComplate(result);
                } else {
                    return result;
                }
            },
            SetZoomAnimate: function (options) {

                this.GetView().animate({

                    duration: options.duration,
                    zoom: options.zoom

                });
            },
            SetZoomGotoCoordinatAnimate: function (options) {

                this.GetView().animate({
                    center: options.CenterPoint,
                    duration: options.duration,
                    zoom: options.zoom

                });
            },
            GetTransformfunction: function (options) {

                return ol.proj.transform(options.Coordinate, options.Current, options.To);

            },
            CreateArcgisXYZLayer: function (options) {

                var opts = $.extend({}, OLDefaults.OLTileLayer, options);
                var result = new ol.layer.Tile({
                    title: opts.Title,
                    source: opts.Source,
                    name: opts.Name
                });
                return result;

            },
            CreateXYZLayerTileLoadFunction: function (src) {

                var splitted = src.split("/");
                var z = splitted[splitted.length - 3].substr(1);
                var y = splitted[splitted.length - 2].substr(1);
                var x = splitted[splitted.length - 1].substr(1);
                var ft = x.substr(x.indexOf("."));
                x = x.substr(0, x.indexOf("."));
                y = (parseInt(y) + 0x0).toString(16).toLowerCase().padStart(8, "0");
                x = (parseInt(x) + 0x0).toString(16).toLowerCase().padStart(8, "0");
                var newSRC = "";
                for (var i = 0; i < splitted.length - 3; i++)
                    newSRC += "/" + splitted[i];
                return newSRC.substr(1) + "/L" + z + "/R" + y + "/C" + x + ft;

            }




        });
    });