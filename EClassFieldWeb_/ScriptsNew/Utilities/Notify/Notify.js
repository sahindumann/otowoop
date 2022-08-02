define(["Utilities/Notify/NotifyDefaults"], function (NotifyDefaults) {

    /*
        Bağımlılık Listesi
        ------------------------------
        1) https://github.com/mouse0270/bootstrap-notify
    */

    return {

        Show: function (options) {

            var opts = $.extend({}, NotifyDefaults, options);

            switch (opts.Type) {
                case 'danger':
                    opts.Icon = 'fa fa-times-circle'; break;
                case 'info':
                    opts.Icon = 'fa fa-info-circle'; break;
                case 'warning':
                    opts.Icon = 'fa fa-exclamation-triangle'; break;
                default:
                    opts.Icon = 'fa fa-check-circle'; break;
            }

            $.notify({
                icon: opts.Icon,
                message: ((opts.Title == '') ? 'İşlem yapılamadı!' : opts.Message),
                title: opts.Title + ' ',
                placement: {
                    from: opts.Placement.From,
                    align: opts.Placement.Align
                },
                animate: {
                    enter: opts.Animate.Enter,
                    exit: opts.Animate.Exit
                }
            }, {
                type: opts.Type
            });
        }
    };
});