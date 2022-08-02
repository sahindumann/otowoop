define(["dojo/Evented"], function (Evented) {
    return dojo.declare([Evented], {

        _name: null,
        _interactionElement: null,
        _interactionEventList: ["drawstart", "drawend", "drawing", "addfeatures", "handleDownEvent", "handleDragEvent", "handleMoveEvent", "handleUpEvent"],

        // Private Functions

        _initEvents: function (interactionElement) {
            var _self = this;
            for (var i = 0; i < _self._interactionEventList.length; i++) {
                interactionElement.on(_self._interactionEventList[i], function (args) {
                    _self.emit(args.type, args);
                });
            }
        },

        constructor: function (name, interactionType, options) {
            var interactionFunction = ol.interaction[interactionType];
            var interactionElement = new interactionFunction(options);
            this._name = name;
            this._interactionElement = interactionElement;
            this._initEvents(this._interactionElement);
        },

        GetName: function () {
            return this._name;
        },

        SetActive: function (IsActive) {
            this._interactionElement.setActive(IsActive);
        },

        GetActive: function () {
            return this._interactionElement.getActive();
        },

        GetInteraction: function () {
            return this._interactionElement;
        }
    });
});