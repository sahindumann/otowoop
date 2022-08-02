define([], function () {
    return {
        Autoclose: false,
        Border: false,
        Callback: false,
        Container: 'body',
        Content: false,
        ContentAjax: false,
        ContentIframe: false,
        ContentOverflow: 'hidden',
        ContentSize: '400 200',
        Custom: false,
        Dblclicks: false,
        DelayClose: 0,
        Draggable: {
            Handle: 'div.jsPanel-hdr, .jsPanel-titlebar',
            Opacity: 0.8
        },
        Dragit: {
            Axis: false,
            Containment: 'window',
            Handles: 'div.jsPanel-hdr, .jsPanel-titlebar',

            Opacity: 0.9,
            Start: false,
            Drag: false,
            Stop: false,
            Disableui: true
        },
        FooterToolbar: false,
        HeaderControls: {
            Close: false,
            Maximize: false,
            Minimize: false,
            Normalize: false,
            Smallify: false,
            Controls: 'all',
            Iconfont: 'jsglyph'
        },
        HeaderRemove: false,
        HeaderTitle: 'Panel',
        HeaderToolbar: false,
        Id: '1',
        MaximizedMargin: {
            Top: 5,
            Right: 5,
            Bottom: 5,
            Left: 5
        },
        MinimizeTo: true,
        OnBeforeClose: false,
        OnBeforeMaximize: false,
        OnBeforeMinimize: false,
        OnBeforeNormalize: false,
        OnBeforeSmallify: false,
        OnBeforeUnsmallify: false,
        OnClosed: false,
        OnMaximized: false,
        OnMinimized: false,
        OnNormalized: false,
        OnBeforeResize: false,
        OnResized: false,
        OnSmallified: false,
        OnUnsmallified: false,
        OnFronted: false,
        OnWindowResize: false,
        Panetlype: false,
        Position: 'center',
        Resizable: {
            Handles: 'n, e, s, w, ne, se, sw, nw',
            AutoHide: false,
            MinWidth: 100,
            MinHeight: 100
        },
        ResizeIt: {
            Containment: false,
            Handles: 'n, e, s, w, ne, se, sw, nw',
            MinWidth: 100,
            MinHeight: 100,
            Start: false,
            Resize: function (panel, b) {
                var _id = "#" + panel[0].getAttribute("id");
                $(_id + " .bootstrap-table").bootstrapTable("refresh");
            },
            Stop: false,
            Disableui: true,
            MaxWidth: function () { return $(window).width() - 20; },
            MaxHeight: function () { return $(window).height() - 20; }
        },
        Rtl: false,
        SetStatus: false,
        Show: false,
        Template: false,
        Theme: 'default'
    };
});