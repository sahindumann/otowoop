define([], function () {
    return dojo.declare(null, {

        constructor: function (map) {
            this.Map = map;
            this.select = new ol.interaction.Select({});
            this.modify = new ol.interaction.Modify({
                features: this.select.getFeatures()
            });
            this.SetEvents();
        },

        SetEvents: function () {
            var selectedFeatures = this.select.getFeatures();

            this.select.on('change:active', function (evt) {

            });

            this.modify.on('modifystart', function (evt) {

            });

            this.modify.on('modifyend', function (evt) {
            });
        },

        SetActive: function (active) {
            this.select.SetActive(active);
            this.modify.SetActive(active);
        }
    });
});