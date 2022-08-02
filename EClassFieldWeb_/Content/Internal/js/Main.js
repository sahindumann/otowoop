

(function ($) {


    function formatRepo(repo) {
        if (repo.loading) {
            return "Aranıyor";
            $('#sb_loading').show();
        }
        var markup = "";
        markup += "<div data-fullcat="+repo.id+">" + repo.text + "</div>";



        return markup;
    }

    function formatRepoSelection(repo) {
        return repo.full_name || repo.text;
    }

    $(document).ready(function () {

        $("#cat_selectilan,.cat_select").select2({

      

 
            placeholder:"Birşeyler arayın",
         
      
            ajax: {
                url: "/Home/GetCategories",
                dataType: 'json',
                delay: 500,
                data: function (params) {
                    return {
                        text: params.term, // search term
                        page: params.page,
                        isnumber:$(this).data("idnumber")?true:false
                    };
                },
                processResults: function (data, page) {

                    return {
                        results: data
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 1,
            templateResult: formatRepo, // omitted for brevity, see the source of this page
            templateSelection: formatRepoSelection // omitted for brevity, see the source of this page  

        });




        $("#cat_selectilan").on('select2:select', function (evt) {
            try {
                $('#sb_loading').show();
                $('#attributes').html('');
                $('#attributes').load("/IlanVer/GetAttributePartial/?categoryID=" + $(this).val(), function (res) {


                    $('#sb_loading').hide();
                });

            }
            catch (e) {


            }

            $('#sb_loading').hide();
        });
    });




    $("#autocomplete-dynamic").autocomplete({
        serviceUrl: '/Home/GetCategoriesSearch',

        paramName:'text',
        transformResult: function (response) {
            return {
                suggestions: $.map(JSON.parse(response), function (dataItem) {
                    return { value: dataItem.value, data: dataItem.data };
                })
            };
        }
    });




    //setTimeout(function () {
    //    alert("sf");
    //    $(function () {
    //        $(".img-resizer").aeImageResize({ height: 250, width: 250 });
    //    });

    //}, 3000);



})(jQuery);


