define([], function () {
    return {
        PermalinkBackList: [],
        PermalinkForwardList: [],
        ShouldUpdate: false,
        MapExplorerDefaults: {
            BackButtonIdentifier: '',
            ForwardButtonIdentifier: ''
        },
        ResetMapDefaults: {
            CenterPoint: [0, 0],
            ZoomLevel: 3
        },
        ClearMapDefaults: {
            TitleList: [],
            RemoveObjectIdentifierList: []
        },
        GetInfoDefaults: {},
        PanMapDefaults: {},
        LengthMeasurement: {},
        AreaMeasurement: {}
    };
});