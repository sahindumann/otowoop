define(["Utilities/OpenLayers/Map",
        "Management/MapTools",
        "Management/DrawTools",
        "dojo/Evented"
], function (Map, MapTools, DrawTools, Evented) {
    return dojo.declare([Evented], {

        Map: null,
        MapTools: null,
        DrawTools: null,

        constructor: function (options) {
            this.Map = new Map(options);
            this.MapTools = new MapTools(this.Map);
            this.DrawTools = new DrawTools(this.Map);
        },

        Init: function () {
        }
    });
});