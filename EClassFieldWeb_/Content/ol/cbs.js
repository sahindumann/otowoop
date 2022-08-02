

(function ($) {


    var source = new ol.source.Vector({ wrapX: false });

    var vectorLayer = new ol.layer.Vector({
        source: source,
        title: 'vectorLayer'
    });



    var map = new ol.Map({
        target: 'map',
        layers: [
          new ol.layer.Tile({
        
           
                  source: new ol.source.BingMaps({
                      key: 'Ag7dnC971UMURHWf2CkE-TZ2h-ByPh7RYrqQxyPwHfGhWaipEw0SAtwExsMrichc',
                      imagerySet: 'RoadOnDemand'
                  })
              })
          
        ],
        view: new ol.View({
            center: ol.proj.fromLonLat([37.41, 8.82]),
            zoom: 4
        })
    });
    map.addLayer(vectorLayer);
    setTimeout(function () {
        $("#map").css("height", window.innerHeight - 80 + "px");
        $("#map").css("position", "absolute");
        map.updateSize();
    }, 1500);


    w3_open = function (id) {
        var thiss = id;
        if ($(thiss).hasClass("close")) {
            document.getElementById("main").style.marginLeft = "25%";
            document.getElementById("mySidebar").style.width = "25%";
            document.getElementById("map").style.width = "75%";
            document.getElementById("mySidebar").style.display = "block";

            map.updateSize();
            $(thiss).addClass("open");
            $(thiss).removeClass("close");
        }
        else {
            document.getElementById("mySidebar").style.display = "none";
            document.getElementById("main").style.marginLeft = "0%";
            document.getElementById("map").style.width = "100%";

            map.updateSize();



            $(thiss).addClass("close");
            $(thiss).removeClass("open");
        }
    }


    $("a.tools").click(function () {

        var tooltype = $(this).data("tool-type");

        switch (tooltype) {

            case "line":
              addInteraction('Point','Buffer')
                break;

        }

    });


    addInteraction = function (type,extendtype) {


        var vectorLayerInteraction = new ol.interaction.Draw({
            source: source,
            type: type,
            title: 'VectorInteraction'

        });
        map.addInteraction(vectorLayerInteraction);

        vectorLayerInteraction.on("drawstart", function (e) {



        });

        vectorLayerInteraction.on("drawend", function (e) {
            map.removeInteraction(vectorLayerInteraction);

            if (extendtype == 'Buffer')
            {
                createBuffer(1000, vectorLayer, e.feature);

            }

            //jsPanel.create({
            //    contentSize: '800 400',
            //    contentAjax: {
            //        method: 'POST',
            //        url: '/Partial/CallPartial/?id=Mahalle',
            //        data: { id: 'Mahalle' },
            //        done: function (panel) {
            //            panel.content.innerHTML = this.responseText;

            //            setTimeout(function () {
            //                $(panel).css("z-index", "99999px");
            //            }, 2000);

            //            $("#mahalle").DataTable({

            //                ajax: {


            //                    url: '/Cbs/GetCities',
            //                    data: { wkt: featureToWkt(e.feature) },



            //                },
            //                columns: [
            //         {
            //             "data": "Name",
            //             "field": "Name",
            //             "title": "Adı"
            //         },
            //             {
            //                 "data": "IlceAdi",
            //                 "field": "IlceAdi",
            //                 "title": "İlçe Adı"
            //             },
            //              {
            //                  "data": "ilAdi",
            //                  "field": "ilAdi",
            //                  "title": "İl Adı"
            //              }



            //                ]


            //            });

            //        },
            //        beforeSend: function () {
            //            this.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            //        }
            //    }
            //});




        });

    }

    featureToWkt = function (feature) {
        var geom = feature.getGeometry().transform('EPSG:3857', 'EPSG:4326');

        var format = new ol.format.WKT();
        var wktRepresenation = format.writeGeometry(geom);

        return wktRepresenation;

    }

    createBuffer = function (radius, layer, pointFeature)
    {

        var  radius= 1000;
     
            var poitnExtent = pointFeature.getGeometry().getExtent();
            var bufferedExtent = new ol.extent.buffer(poitnExtent,radius);
            console.log(bufferedExtent);
            var bufferPolygon = new ol.geom.Polygon(
            [
                [[bufferedExtent[0],bufferedExtent[1]],
                [bufferedExtent[0],bufferedExtent[3]],
                [bufferedExtent[2],bufferedExtent[3]],
                [bufferedExtent[2],bufferedExtent[1]],
                [bufferedExtent[0],bufferedExtent[1]]]
            ]
            );
 
            var bufferedFeature = new ol.Feature(bufferPolygon);
            layer.getSource().addFeature(bufferedFeature);
            var extent = layer.getSource().getExtent();
            map.getView().fit(extent, map.getSize());
    }


})(jQuery);