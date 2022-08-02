define(["Management/DrawToolsDefaults"
], function (DrawToolsDefaults) {
    /*
        Bağımlılıklar
        --------------------------------------
        - Openlayers 4
    
    */
    var Sketch, HelpToolTipElement, HelpToolTip, MeasureToolTipElement, MeasureToolTip, ActiveType;
    var ContinueMessage = 'Ölçümü Bitirmek İçin Çift Tıklayınız', HelpMsg = 'Ölçüme Başlamak İçin Tıklayınız';

    return dojo.declare(null, {
        constructor: function (map) {
            this.Map = map;
            //this.Draw = this.Map.CreateDrawEquipment();
            //this.Modify = this.Map.CreateDrawModify();
        },
        FormatLength: function (lineGeometry) {
            var sphereDistance = new ol.Sphere(6378137);
            var sphericalLength = 0;
            var cartesianLength = 0;
            var sphericalOutput;

            try {
                if (lineGeometry.getType() == 'Circle') {
                    var units = this.Map.GetView().getProjection().getUnits();
                    var radius = lineGeometry.getRadius() * ol.proj.METERS_PER_UNIT[units];

                    var startPoint = ol.proj.transform(lineGeometry.getFirstCoordinate(), 'EPSG:3857', 'EPSG:4326');
                    var lastPoint = ol.proj.transform(lineGeometry.getLastCoordinate(), 'EPSG:3857', 'EPSG:4326');

                    sphericalLength = sphereDistance.haversineDistance(startPoint, lastPoint);

                    if (sphericalLength > 1000) {
                        sphericalOutput = (Math.round(sphericalLength / 1000 * 100) / 100) +
                            ' ' + 'km';
                    } else {
                        sphericalOutput = (Math.round(sphericalLength * 100) / 100) +
                            ' ' + 'm';
                    }

                } else {
                    var coordinates = lineGeometry.getCoordinates();

                    for (var i = 0, ii = coordinates.length - 1; i < ii; ++i) {
                        var c1 = ol.proj.transform(coordinates[i], 'EPSG:3857', 'EPSG:4326');
                        var c2 = ol.proj.transform(coordinates[i + 1], 'EPSG:3857', 'EPSG:4326');
                        sphericalLength += sphereDistance.haversineDistance(c1, c2);
                    }

                    cartesianLength = Math.round(lineGeometry.getLength() * 100) / 100;

                    var cartesianOutput;
                    if (cartesianLength > 1000) {
                        cartesianOutput = (Math.round(cartesianLength / 1000 * 100) / 100) +
                            ' ' + 'km';
                    } else {
                        cartesianOutput = (Math.round(cartesianLength * 100) / 100) +
                            ' ' + 'm';
                    }

                    if (sphericalLength > 1000) {
                        sphericalOutput = (Math.round(sphericalLength / 1000 * 100) / 100) +
                            ' ' + 'km';
                    } else {
                        sphericalOutput = (Math.round(sphericalLength * 100) / 100) +
                            ' ' + 'm';
                    }
                }
            } catch (e) {
                alert('FormatLength');
            }

            return sphericalOutput;
        },
        FormatArea: function (polygonGeometry) {
            var sphereArea = new ol.Sphere(6378137);
            var sphericalArea = 0;
            var sphericalOutput;

            try {
                var copyPolygon = polygonGeometry.clone(); //new ol.geom.Polygon(polygon.getCoordinates()[0]);
                var polygonCoordinates = copyPolygon.transform('EPSG:3857', 'EPSG:4326').getLinearRing(0).getCoordinates();
                sphericalArea = Math.abs(sphereArea.geodesicArea(polygonCoordinates));

                var cartesianArea = polygonGeometry.getArea();
                var cartesianOutput;

                if (cartesianArea > 1000000) {
                    cartesianOutput = (Math.round(cartesianArea / 1000000 * 100) / 100) +
                        ' ' + 'km2';
                } else {
                    cartesianOutput = (Math.round(cartesianArea * 100) / 100) +
                        ' ' + 'm2';
                }
                if (sphericalArea > 1000000) {
                    sphericalOutput = (Math.round(sphericalArea / 1000000 * 100) / 100) +
                        ' ' + 'km2';
                } else {
                    sphericalOutput = (Math.round(sphericalArea * 100) / 100) +
                        ' ' + 'm2';
                }
            } catch (e) {
                alert('FormatArea');
            }

            return sphericalOutput;
        },
        MeasureMoveHandler: function (Event) {
            //var opts = $.extend({}, DrawToolsDefaults.MoveHandlerDefaults, options);
            if (Event == undefined) return;

            if (Event.dragging) {
                return;
            }

            if ($('.tooltip-message-measure').length <= 0) {
                this.CreateHelpToolTip();
                this.CreateMeasureToolTip();
            }

            //var HelpMsg = 'Ölçüme Başlamak İçin Tıklatın';
            var TooltipCoord = Event.coordinate;

            if (this.Sketch) {
                var Output;
                var Geom = (this.Sketch.getGeometry());
                if (Geom instanceof ol.geom.Polygon) {
                    Output = this.FormatArea(/** @type {ol.Geom.Polygon} */(Geom));
                    HelpMsg = ContinueMessage;
                    TooltipCoord = Geom.getInteriorPoint().getCoordinates();
                } else if (Geom instanceof ol.geom.LineString) {
                    Output = this.FormatLength( /** @type {ol.Geom.LineString} */(Geom));
                    HelpMsg = ContinueMessage;
                    TooltipCoord = Geom.getLastCoordinate();
                } else if (Geom instanceof ol.geom.Circle) {
                    Output = this.FormatLength( /** @type {ol.Geom.Circle} */(Geom));
                    HelpMsg = ContinueMessage;
                    TooltipCoord = Geom.getLastCoordinate();
                }
                this.MeasureToolTipElement.innerHTML = Output;
                this.MeasureToolTip.setPosition(TooltipCoord);
            }

            this.HelpToolTipElement.innerHTML = HelpMsg;
            this.HelpToolTip.setPosition(Event.coordinate);
        },
        CreateHelpToolTip: function (options) {
            if (this.HelpToolTipElement) {
                if (this.HelpToolTipElement.parentNode != null)
                    this.HelpToolTipElement.parentNode.removeChild(this.HelpToolTipElement);
            }
            this.HelpToolTipElement = document.createElement('div');
            this.HelpToolTipElement.setAttribute('id', 'measurement-comment');
            this.HelpToolTipElement.className = 'tooltip-measure';
            this.HelpToolTip = this.Map.CreateOverlay({
                element: this.HelpToolTipElement,
                offset: [15, 0],
                positioning: 'center-left'
            });
            this.Map.AddOverlay({ Overlay: this.HelpToolTip });
        },
        CreateMeasureToolTip: function (options) {
            $(".tooltip-message-measure").remove();
            if (this.MeasureToolTipElement) {
                if (this.MeasureToolTipElement.parentNode != null)
                    this.MeasureToolTipElement.parentNode.removeChild(this.MeasureToolTipElement);
            }
            this.MeasureToolTipElement = document.createElement('div');
            this.MeasureToolTipElement.setAttribute('id', 'measurement-value');
            this.MeasureToolTipElement.className = 'tooltip-measure tooltip-message-measure';
            this.MeasureToolTip = new ol.Overlay({
                element: this.MeasureToolTipElement,
                offset: [0, -15],
                positioning: 'bottom-center'
            });
            HelpMsg = 'Ölçüme Başlamak İçin Tıklayınız';
            this.Map.AddOverlay({ Overlay: this.MeasureToolTip });
        }
    });
});