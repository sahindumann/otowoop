define([], function () {
    var BingKey = 'cFtbdF0ydlkdvhXC5wKO~ybf3qUqF_cTjHHB99chFrg~Ahw3Fh7i7XxRxdYhYosI4GLxwKDsTBjC5J8xGXdFxVVxeg7fdlMR5D-rx8gM5e0i';
    return {
        OLBaseLayer: {
            LayerTitle: 'BaseLayer',
            LayerName: '',
            BaseMapName: '',
            FunctionName: ''
        },
        OLMapCanvas: {
            CanvasId: 'map-canvas'
        },
        OLProjection: {
            Code: 'EPSG:3857',
            Units: undefined,
            Extent: undefined
        },
        OLControl: {
            Attribution: false,
            Zoom: true,
            Rotate: true
        },
        OLStyle: {
            Image: new ol.style.Circle({
                fill: new ol.style.Fill({
                    color: 'rgba(255,255,255,0.4)'
                }),
                stroke: new ol.style.Stroke({
                    color: '#3399CC',
                    width: 1.25
                }),
                radius: 5
            }),
            Fill: new ol.style.Fill({
                color: 'rgba(255,255,255,0.4)'
            }),
            Stroke: new ol.style.Stroke({
                color: '#3399CC',
                width: 1.25
            }),
            Text: new ol.style.Text({
                font: '12px Calibri,sans-serif',
                fill: new ol.style.Fill({
                    color: '#000'
                }),
                stroke: new ol.style.Stroke({
                    color: '#fff', width: 5
                }),
                text: ''
            })
        },
        OLView: {
            Projection: 'EPSG:3857',
            Center: [0, 0],
            Zoom: 1,
            Rotation: 0
        },
        OLVectorSource: {
            Format: undefined,
            Loader: function () { return undefined; },
            Projection: new ol.proj.Projection({ code: 'EPSG:3857', units: undefined, extent: undefined })
        },
        OLTileWMSSource: {
            Url: undefined,
            CrossOrigin: 'Anonymous',
            Params: { LAYERS: [] },
            Format: 'image/png8',
            Gutter: undefined,
            TileLoadFunction: undefined
        },
        OLXYZSource: {
            CrossOrigin: 'Anonymous ',
            Url: 'http://mts0.google.com/vt/lyrs=m&x={x}&y={y}&z={z}',
            TileLoadFunction: undefined
        },
        OLBingSource: {
            CrossOrigin: 'Anonymous ',
            ImagerySet: 'Road',
            Key: BingKey
        },

        OLOSMSource: {
            CrossOrigin: 'Anonymous ',
            Url: 'http://{a-c}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png'
        },
        OLVectorLayer: {
            //Source: new ol.source.Vector({}),
            Style: undefined,
            Title: undefined,
            Name: undefined,
            Opacity: 1
        },
        OLTileLayer: {
            Title: '',
            Source: undefined,
            Opacity: 1,
            MinResolution: undefined,
            MaxResolution: undefined
        },
        OLFindLayer: {
            Title: undefined,
            Name: undefined
        },
        OLClearLayer: {
            TitleList: []
        },
        OLGetLayerSource: {
            Title: undefined,
            Name: undefined
        },
        OLUpdateLayerSource: {
            Title: undefined,
            Name: undefined,
            Source: undefined
        },
        OLUpdateLayerSourceParams: {
            Title: undefined,
            Name: undefined,
            Params: {}
        },
        OLAddFeature: {
            WKTString: '',
            DataProjection: 'EPSG:4326',
            FeatureProjection: 'EPSG:3857',
            LayerTitle: '',
        },
        OLMap: {
            View: new ol.View({ center: [0, 0], zoom: 4 }),
            Layers: [],
            Target: '',
            Controls: new ol.control.defaults({ attribution: false, rotate: true, zoom: true }),
            Interactions: new ol.interaction.DragAndDrop({ formatConstructors: [ol.format.GPX, ol.format.GeoJSON, ol.format.IGC, ol.format.KML, ol.format.TopoJSON] })
        },
        OLScale: {
            ScaleValue: 50000
        },
        OLResolution: {
            ResolutionValue: 0
        },
        OLZoom: {
            ZoomLevel: 5
        },
        OLRotation: {
            RotationDegree: 0
        },
        OLCenter: {
            CenterPoint: [0, 0]
        },
        OLOverlay: {
            Overlay: undefined
        },
        OLMapEvent: {
            EventName: ''
        }
    };
});