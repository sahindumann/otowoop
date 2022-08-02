define([], function () {
    return dojo.declare(null, {

        /*
            Properties
        */

        constructor: function (map) {
            this.Map = map;
        },

        Init: function () {
            this.Map.AddInteraction(this.GetInteractionPoint({ Title: 'VectorLayer', Type: 'Point' }));
            this.GetInteractionPoint({ Title: 'VectorLayer', Type: 'Point' }).setActive(false);

            this.Map.AddInteraction(this.GetInteractionInfo({ Title: 'InfoLayer', Type: 'Point' }));
            this.GetInteractionInfo({ Title: 'InfoLayer', Type: 'Point' }).setActive(false);

            this.Map.AddInteraction(this.GetInteractionLineString({ Title: 'VectorLayer', Type: 'LineString' }));
            this.GetInteractionLineString({ Title: 'VectorLayer', Type: 'LineString' }).setActive(false);

            this.Map.AddInteraction(this.GetInteractionPolygon({ Title: 'VectorLayer', Type: 'Polygon' }));
            this.GetInteractionPolygon({ Title: 'VectorLayer', Type: 'Polygon' }).setActive(false);

            this.Map.AddInteraction(this.GetInteractionDistance({ Title: 'MeasurementLayer', Type: 'LineString' }));
            //this.SetActive(false, this.GetInteractionDistance);
            this.GetInteractionDistance({ Title: 'MeasurementLayer', Type: 'LineString' }).setActive(false);

            this.Map.AddInteraction(this.GetInteracionArea({ Title: 'MeasurementLayer', Type: 'Polygon' }));
            //this.SetActive(false, this.GetInteracionArea);
            this.GetInteracionArea({ Title: 'MeasurementLayer', Type: 'Polygon' }).setActive(false);

            //this.Map.AddInteraction(GetInteractionPoint({ Title: '', Type: '' }));
            //this.Map.AddInteraction(GetInteractionPoint({ Title: '', Type: '' }));

            this.GetInteractionDistance({ Title: 'MeasurementLayer', Type: 'LineString' }).on('drawstart', function (event) {

            });

            this.GetInteractionDistance({ Title: 'MeasurementLayer', Type: 'LineString' }).on('drawend', function (event) {

            });
            this.SetActive(false);
        },

        GetInteractionPoint: function (options) {
            return this.Map.InteractionDraw({
                source: { Title: options.Title },
                type: (options.Type),
                GeometryFunction: function (coords, Geom) {
                    if (!Geom) {
                        Geom = new ol.geom.LineString(null);
                    }
                    if (undo) { //Ctrl + Z
                        if (coords.length > 1) {
                            coords.pop();
                        }
                        undo = false;
                    }
                    Geom.setCoordinates(coords);
                    return Geom;
                }
            });
        },

        GetInteractionInfo: function (options) {
            return this.Map.InteractionDraw({
                source: { Title: options.Title },
                style: new ol.style.Style({
                    fill: null,
                    stroke: null
                }),
                type: (options.Type)
            });
        },

        GetInteractionLineString: function (options) {
            return this.Map.InteractionDraw({
                source: { Title: options.Title },
                type: (options.Type),
                GeometryFunction: function (coords, Geom) {
                    if (!Geom) {
                        Geom = new ol.geom.LineString(null);
                    }
                    if (undo) { //Ctrl + Z
                        if (coords.length > 1) {
                            coords.pop();
                        }
                        undo = false;
                    }
                    Geom.setCoordinates(coords);
                    return Geom;
                }
            });
        },

        GetInteractionPolygon: function (options) {
            return this.Map.InteractionDraw({
                source: { Title: options.Title },
                type: (options.Type),
                GeometryFunction: function (coords, Geom) {
                    if (!Geom) {
                        Geom = new ol.geom.Polygon(null);
                    }
                    if (undo) { //Ctrl + Z
                        if (coords[0].length > 1) {
                            coords[0].pop();
                        }
                        undo = false;
                    }
                    Geom.setCoordinates(coords);
                    return Geom;
                }
            });
        },

        GetInteractionDistance: function (options) {
            return this.Map.InteractionDraw({
                source: { Title: options.Title },
                type: (options.Type)
            });
        },

        GetInteracionArea: function (options) {
            return this.Map.InteractionDraw({
                source: { Title: options.Title },
                type: (options.Type)
            });
        },

        GetActive: function () {
            if (this.activeType == undefined) return true;
            return this.activeType ? this[this.activeType].GetActive() : false;
        },

        SetActive: function (active, type) {
            if (active) {
                try {
                    this.activeType && this[this.activeType].SetActive(false);
                    this[type].SetActive(true);
                    this.activeType = type;
                } catch (e) {
                }

            } else {
                this.activeType && this[this.activeType].SetActive(false);
                this.activeType = null;
            }
        }
    });
});