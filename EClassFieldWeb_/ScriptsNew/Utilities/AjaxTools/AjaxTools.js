define(["Utilities/MessageBox/MessageBox", "Utilities/Util"], function (MessageBox, Util) {
    return {
        Call: function (controller, action, method, isAsync, data, oncomplete) {




  
            if (NProgress) { NProgress.start(); }


            $.ajax({
                type: method,
                url: "/"+ controller + '/' + action,
                async: isAsync,
                data: data != null ? JSON.stringify(data) : null,
                contentType: "application/json; charset=utf-8",

                error: function (xhr, status, error) {

                  

                    MessageBox.Show({ Type: 'OK', Title: 'Hata!', MessageText: 'Üzgünüm, İşlem Aşağıdaki Sebeplerden Dolayı Devam Edemiyor!<br/><br/>' + error, MessageType: 'danger' });
                }
            }).done(function (data) {



                if (oncomplete) {
             
                    oncomplete(data);

                }
            }).always(function () {
                if (NProgress) { NProgress.done(); }

            });
        },

        CallStandartAjax: function (url, method, dataType, isAsync, data, oncomplete) {
            $.ajax({
                type: method,
                url: url,
                async: isAsync,
                dataType: dataType,
                data: data != null ? JSON.stringify(data) : null,
                contentType: "application/json; charset=utf-8",
                error: function (xhr, status, error) {
                    MessageBox.Show({ Type: 'OK', Title: 'Hata!', MessageText: 'Üzgünüm, İşlem Aşağıdaki Sebeplerden Dolayı Devam Edemiyor!<br/><br/>' + error, MessageType: 'danger' });
                }
            }).done(function (data) {
                if (oncomplete) {
                    oncomplete(data);
                }
            });
        },

        //GETDataSync: function (controller, action, oncomplete) {
        //    this.Call(controller, action, 'GET', false, null, oncomplete);
        //},
        GETDataSync: function (controller, action, oncomplete, onerror) {
            if (onerror == null || onerror == undefined) {
                this.Call(controller, action, 'GET', false, null, oncomplete);
            }
            else {
                this.Call(controller, action, 'GET', false, null, oncomplete, onerror);
            }
           
        },
        GETDataAsync: function (controller, action, oncomplete) {
            this.Call(controller, action, 'GET', true, null, oncomplete);
        },

        GETPartialSync: function (action, oncomplete) {
            this.Call('Partial', action, 'GET', false, null, oncomplete);
        },

        GETPartialAsync: function (action, oncomplete) {
            this.Call('Partial', action, 'GET', true, null, oncomplete);
        },

        GETDataFromURLSync: function (url, oncomplete) {
            this.CallStandartAjax(url, 'GET', false, null, oncomplete);
        },

        GETDataFromURLAsync: function (url, oncomplete) {
            this.CallStandartAjax(url, 'GET', true, null, oncomplete);
        },

        POSTDataSync: function (controller, action, data, oncomplete) {
            this.Call(controller, action, 'POST', false, data, oncomplete);
        },

        POSTDataAsync: function (controller, action, data, oncomplete) {
            this.Call(controller, action, 'POST', true, data, oncomplete);
        },

        POSTPartialSync: function (action, data, oncomplete) {
            this.Call('Partial', action, 'POST', false, data, oncomplete);
        },

        POSTPartialAsync: function (action, data, oncomplete) {
            this.Call('Partial', action, 'POST', true, data, oncomplete);
        },

        POSTDataFromURLSync: function (url, data, oncomplete) {
            this.CallStandartAjax(url, 'POST', false, data, oncomplete);
        },

        POSTDataFromURLAsync: function (url, data, oncomplete) {
            this.CallStandartAjax(url, 'POST', true, data, oncomplete);
        },

        SetHTML: function (elementId, data) {
            $('#' + elementId).empty();
            $('#' + elementId).html(data);
        },

        SetLoadPage: function (elementId, call) {
            $(elementId).load(call);
        },

        LoadPartialPage: function (partialName, targetIdentifier, isPost, isSync, onComplate) {
            var type;
            if (isPost) {
                type = isSync ? 'POSTPartialSync' : 'POSTPartialAsync';
            } else {
                type = isSync ? 'GETPartialSync' : 'GETPartialAsync';
            }

            this[type]('CallPartial', { partialName: partialName }, function (data) {
                $(targetIdentifier).html(data);

                if (onComplate != undefined) {
                    onComplate();
                }
            });
        },




    };
});