/*
Theme : Carspot | Car Dealership - Vehicle Marketplace And Car Services
Author: ScriptsBundle
Version: 1.0
Designed and Development by: ScriptsBundle
*/
/*
====================================
[ CSS TABLE CONTENT ]
------------------------------------
    1.0 -  Page Preloader
	2.0 -  Counter FunFacts
    3.0 -  List Grid Style Switcher
	4.0 -  Sticky Ads
	5.0 -  Accordion Panels
    6.0 -  Accordion Style 2
	7.0 -  Jquery CheckBoxes
	8.0 -  Jquery Select Dropdowns
    9.0 -  Profile Image Upload
    10.0 - Masonry Grid System
	11.0 - Featured Carousel 1
    12.0 - Featured Carousel 2
	12.0 - Featured Carousel 3
	13.0 - Category Carousel
	14.0 - Background Image Rotator Carousel
	15.0 - Single Ad Slider Carousel
	16.0 - Single Page SLider With Thumb
	17.0 - Price Range Slider
	18.0 - Template MegaMenu
	19.0 - Back To Top
	20.0 - Tooltip
	21.0 - Quick Overview Modal
-------------------------------------
[ END JQUERY TABLE CONTENT ]
=====================================
*/

var files=[];

(function ($) {

    "use strict";


    $(document).ready(function(){
        var isilanduzenle=false;
        $(".loadimages").each(function(){
          
            isilanduzenle=true;
            files.push({guid:this.alt,orgnalname:this.alt});
        });

        if(isilanduzenle)
        {
        
            $(".dz-remove").click(function(){
            
              

                var index=getArrayIndex($(this).data("imageurl"),files,"guid");

               


                //$.post("/Ilanver/DeleteImage/",{filename:$(this).data("imageurl")},function(data){
                files.splice(index,1);
                $("#_id"+$(this).data("imageurl").replace(".","")).remove();
                //});
            });
        }


    });

    /*Video Popup*/
    var get_active_val = $('#is_video_active').val();
    if (get_active_val != "" && get_active_val == 1) {
        $("a.play-video").YouTubePopUp();
    }
    //Car Comparison
    $('#comparison_button').on('click', function () {
        var id1 = $('#keyword1').val();
        var id2 = $('#keyword2').val();
        if (id1 && id2) {
            $('#sb_loading').show();
            $.post(carspot_ajax_url, { action: 'comparison_data_fetch', keyword1: id1, keyword2: id2, }).done(function (response) {
                $('#sb_loading').hide();
                $('#populate_data').html(response);
                $('#first_accor').first().addClass('open');
                $('#first_accor .accordion-content').first().css('display', 'block').slideDown(400);
            });
        }
        else {
            var msg = 'Select Cars You Want To Compare';
            toastr.error(msg, '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            return false;
        }
    });



    $('#submit_loader').on('click', function () {
        $('#sb_loading').show();
        $(this).closest("form").submit();
    });
    $('#submit_loader2').on('click', function () {
        $('#sb_loading').show();
        $(this).closest("form").submit();
    });

    var ajax_url = $("input#carspot_ajax_url").val();
    var searchType = $('select#autocomplete-select').find('option:selected').val();
    var extraParams = { "action": "carspot_data_fetch_live", "searchType": searchType };

    console.log(ajax_url);
    //   $('#autocomplete-dynamic').autocomplete({

    //       serviceUrl: "/Base/GetCategoriesSearch",
    //	id: 'test',
    //       type: 'POST',
    //       paramName:'text'
    //	params : { "action": "data_fetch", searh_type: function(){return $('select#autocomplete-select').find('option:selected').val();  }},
    //	autoSelectFirst: true,
    //	clear: true,
    //	onSearchStart: function(){ $('#autocomplete-dynamic').addClass('banner-icon-loader'); },
    //	onSearchComplete:function(){ $('#autocomplete-dynamic').removeClass('banner-icon-loader'); },		
    //	onSelect: function (suggestion)
    //	{
    //	}
    //});	




    var carspot_ajax_url = $('#carspot_ajax_url').val();

    var ua = navigator.userAgent.toLowerCase();
    if (ua.indexOf("safari/") !== -1 &&  // It says it's Safari
        ua.indexOf("windows") !== -1 &&  // It says it's on Windows
        ua.indexOf("chrom") === -1     // It DOESN'T say it's Chrome/Chromium
    ) {
        $('.sb-top-bar_notification').show();
    }

    /* ======= Preloader ======= */
    $(window).load(function () {
        $('#cssload-wrapper').fadeOut('slow', function () { $(this).remove(); });
    });

    $('.fancybox').fancybox();


    if ($('.wow').length) {
        var wow = new WOW(
            {
                boxClass: 'wow',
                animateClass: 'animated',
                offset: 0,
                mobile: false,
                live: true
            }
        );
        wow.init();
    }

    /* ======= Counter FunFacts ======= */
    var timer = $('.timer');
    if (timer.length) {
        timer.appear(function () {
            timer.countTo();
        });
    }

    /* ======= Progress bars ======= */
    $('.progress-bar > span').each(function () {
        var $this = $(this);
        var width = $(this).data('percent');
        $this.css({
            'transition': 'width 3s'
        });
        setTimeout(function () {
            $this.appear(function () {
                $this.css('width', width + '%');
            });
        }, 500);
    });

    /* ======= Accordion Panels ======= */
    $('.accordion li').first().addClass('open');
    $('.accordion li .accordion-content').first().css('display', 'block').slideDown(400);
    $(document).on('click', '.accordion-title a', function (event) {
        event.preventDefault();
        if ($(this).parents('li').hasClass('open')) {
            $(this).parents('li').removeClass('open').find('.accordion-content').slideUp(400);
        } else {
            $(this).parents('.accordion').find('.accordion-content').not($(this).parents('li').find('.accordion-content')).slideUp(400);
            $(this).parents('.accordion').find('> li').not($(this).parents('li')).removeClass('open');
            $(this).parents('li').addClass('open').find('.accordion-content').slideDown(400);
        }
    });

    /* ======= Accordion Style 2 ======= */
    $('#accordion').on('shown.bs.collapse', function () {
        var offset = $('.panel.panel-default > .panel-collapse.in').offset();
        if (offset) {
            $('html,body').animate({
                //scrollTop: $('.panel-title a').offset().top - 20
            }, 500);
        }
    });

    /* ======= Jquery CheckBoxes ======= */
    $('.skin-minimal .list li input').iCheck({
        checkboxClass: 'icheckbox_minimal',
        radioClass: 'iradio_minimal',
        increaseArea: '20%' // optional
    });

    var get_sticky = $('#is_sticky_header').val();
    var is_sticky = false;
    if (get_sticky != "" && get_sticky == "1") {
        var is_sticky = true;
    }
    if ($('#is_rtl').val() != "" && $('#is_rtl').val() == "1") {
        /* ======= Jquery Select Dropdowns ======= */
        $(".select").select2({
            dir: "rtl",
            placeholder: $('#select_place_holder').val(),
            allowClear: true,
            width: '100%',
            height:'30px'
        });

        $(".sb_variation").select2({
            dir: "rtl",
            placeholder: $('#select_place_holder').val(),
            allowClear: false,
            theme: "classic",
            width: '100%',
        });

        $(".search-select").select2({
            dir: "rtl",
            placeholder: $('#select_place_holder').val(),
            allowClear: false,
            theme: "classic",
            width: '100%',
        });

        $(".product-thumb").owlCarousel({
            rtl: true,
            dots: ($(".product-thumb .item").length > 1) ? false : false,
            loop: ($(".product-thumb .item").length > 1) ? true : false,
            autoWidth: true,
            nav: true,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 4,
                }
            }
        });

        $('.featured-slider-shop').owlCarousel({
            rtl: true,
            items: 4,
            dots: ($(".featured-slider-shop .item").length > 1) ? false : false,
            loop: ($(".featured-slider-shop .item").length > 1) ? true : false,
            nav: true,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 4,
                }
            }
        });

        $(".owl-testimonial-2").owlCarousel({
            rtl: true,
            autoplay: true,
            autoplayTimeout: 3000,
            autoplayHoverPause: true,
            responsiveClass: true,
            dots: ($(".owl-testimonial-2 .item").length > 1) ? false : false,
            loop: ($(".owl-testimonial-2 .item").length > 1) ? true : false,
            items: 3,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 3,
                }
            },
            stopOnHover: true
        });

        $(".owl-testimonial-1").owlCarousel({
            rtl: true,
            autoplay: true,
            autoplayTimeout: 3000,
            autoplayHoverPause: true,
            dots: ($(".owl-testimonial-2 .item").length > 1) ? false : false,
            loop: ($(".owl-testimonial-2 .item").length > 1) ? true : false,
            responsiveClass: true,
            items: 2,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 2,
                }
            },
            stopOnHover: true
        });

        $('.featured-slider').owlCarousel({
            items: 3,
            rtl: true,
            dots: ($(".featured-slider .item").length > 1) ? false : false,
            loop: ($(".featured-slider .item").length > 1) ? true : false,
            nav: true,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 3,
                }
            }
        });


        $(".clients-list").owlCarousel({
            rtl: true,
            dots: ($(".clients-list .item").length > 1) ? false : false,
            loop: ($(".clients-list .item").length > 1) ? true : false,
            nav: false,
            items: 5,
            autoplay: true,
            autoplayTimeout: 2000,
            autoplayHoverPause: true,
            responsive: {
                0: {
                    items: 2,
                },
                600: {
                    items: 4,
                },
                1000: {
                    items: 5,
                }
            }


        });

        /* ======= Featured Carousel 2 ======= */
        $('.featured-slider-1').owlCarousel({
            rtl: true,
            dots: ($(".featured-slider-1 .item").length > 1) ? false : false,
            loop: ($(".featured-slider-1 .item").length > 1) ? true : false,
            margin: -10,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: true
                },
                600: {
                    items: 2,
                    nav: true
                },
                1000: {
                    items: 3,
                    nav: true,
                    loop: false
                }
            }
        });

        /* ======= Featured  Carousel 3 ======= */
        $('.featured-slider-3').owlCarousel({
            rtl: true,
            dots: ($(".featured-slider-3 .item").length > 1) ? false : false,
            loop: ($(".featured-slider-3 .item").length > 1) ? true : false,
            margin: 0,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: true
                },
                600: {
                    items: 1,
                    nav: true
                },
                1000: {
                    items: 1,
                    nav: true,
                    loop: false
                }
            }
        });

        /* ======= Category Carousel ======= */
        $('.category-slider').owlCarousel({
            loop: true,
            rtl: true,
            dots: false,
            margin: 0,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: true
                },
                600: {
                    items: 2,
                    nav: true
                },
                1000: {
                    items: 4,
                    nav: true,
                    loop: false
                }
            }
        });

        /* ======= Background Image Rotator Carousel ======= */
        $('.background-rotator-slider').owlCarousel({
            loop: false,
            rtl: true,
            dots: false,
            margin: 0,
            autoplay: true,
            mouseDrag: true,
            touchDrag: true,
            autoplayTimeout: 5000,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            nav: false,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 1,
                },
                1000: {
                    items: 1,
                }
            }
        });

        /* ======= إعلان واحد Slider Carousel  ======= */
        $('.single-details').owlCarousel({
            dots: ($(".single-details .item").length > 1) ? false : false,
            loop: ($(".single-details .item").length > 1) ? true : false,
            rtl: true,
            margin: 0,
            autoplay: false,
            mouseDrag: true,
            touchDrag: true,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            nav: true,
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 1,
                },
                1000: {
                    items: 1,
                }
            }
        });




        /*==========  Single Page SLider With Thumb ==========*/

        $('#carousel').flexslider({
            animation: "slide",
            controlNav: false,
            animationLoop: false,
            slideshow: false,
            itemWidth: 100,
            itemMargin: 5,
            asNavFor: '#single-slider',
            rtl: true
       
        });

        $('#single-slider').flexslider({
            rtl: true,
            animation: "slide",
            controlNav: false,
            animationLoop: false,
            slideshow: false,
            sync: "#carousel",
            controlsContainer: ".flex-container",
            start: function(slider) {
                $('.slides li img').click(function(event){
                    event.preventDefault();
                    slider.flexAnimate(slider.getTarget("next"));
                });
            }

        });

        $('.posts-masonry').imagesLoaded(function () {
            $('.posts-masonry').isotope({
                layoutMode: 'masonry',
                transitionDuration: '0.3s',
                isOriginLeft: false,
            });
        });


        $('#menu-1').megaMenu({
            // DESKTOP MODE SETTINGS
            logo_align: 'left', // align the logo left or right. options (left) or (right)
            links_align: 'left', // align the links left or right. options (left) or (right)
            socialBar_align: 'right', // align the socialBar left or right. options (left) or (right)
            searchBar_align: 'left', // align the search bar left or right. options (left) or (right)
            trigger: 'hover', // show drop down using click or hover. options (hover) or (click)
            effect: 'expand-top', // drop down effects. options (fade), (scale), (expand-top), (expand-bottom), (expand-left), (expand-right)
            effect_speed: 400, // drop down show speed in milliseconds
            sibling: true, // hide the others showing drop downs if this option true. this option works on if the trigger option is "click". options (true) or (false)
            outside_click_close: true, // hide the showing drop downs when user click outside the menu. this option works if the trigger option is "click". options (true) or (false)
            top_fixed: false, // fixed the menu top of the screen. options (true) or (false)
            sticky_header: is_sticky, // menu fixed on top when scroll down down. options (true) or (false)
            sticky_header_height: 200, // sticky header height top of the screen. activate sticky header when meet the height. option change the height in px value.
            menu_position: 'horizontal', // change the menu position. options (horizontal), (vertical-left) or (vertical-right)
            full_width: false, // make menu full width. options (true) or (false)
            // MOBILE MODE SETTINGS
            mobile_settings: {
                collapse: true, // collapse the menu on click. options (true) or (false)
                sibling: true, // hide the others showing drop downs when click on current drop down. options (true) or (false)
                scrollBar: true, // enable the scroll bar. options (true) or (false)
                scrollBar_height: 400, // scroll bar height in px value. this option works if the scrollBar option true.
                top_fixed: false, // fixed menu top of the screen. options (true) or (false)
                sticky_header: false, // menu fixed on top when scroll down down. options (true) or (false)
                sticky_header_height: 200 // sticky header height top of the screen. activate sticky header when meet the height. option change the height in px value.
            }
        });


    }
    else {
        /* ======= Template MegaMenu  ======= */
        $('#menu-1').megaMenu({
            // DESKTOP MODE SETTINGS
            logo_align: 'left', // align the logo left or right. options (left) or (right)
            links_align: 'left', // align the links left or right. options (left) or (right)
            socialBar_align: 'left', // align the socialBar left or right. options (left) or (right)
            searchBar_align: 'right', // align the search bar left or right. options (left) or (right)
            trigger: 'hover', // show drop down using click or hover. options (hover) or (click)
            effect: 'expand-top', // drop down effects. options (fade), (scale), (expand-top), (expand-bottom), (expand-left), (expand-right)
            effect_speed: 400, // drop down show speed in milliseconds
            sibling: true, // hide the others showing drop downs if this option true. this option works on if the trigger option is "click". options (true) or (false)
            outside_click_close: true, // hide the showing drop downs when user click outside the menu. this option works if the trigger option is "click". options (true) or (false)
            top_fixed: false, // fixed the menu top of the screen. options (true) or (false)
            sticky_header: is_sticky, // menu fixed on top when scroll down down. options (true) or (false)
            sticky_header_height: 200, // sticky header height top of the screen. activate sticky header when meet the height. option change the height in px value.
            menu_position: 'horizontal', // change the menu position. options (horizontal), (vertical-left) or (vertical-right)
            full_width: false, // make menu full width. options (true) or (false)
            // MOBILE MODE SETTINGS
            mobile_settings: {
                collapse: true, // collapse the menu on click. options (true) or (false)
                sibling: true, // hide the others showing drop downs when click on current drop down. options (true) or (false)
                scrollBar: true, // enable the scroll bar. options (true) or (false)
                scrollBar_height: 400, // scroll bar height in px value. this option works if the scrollBar option true.
                top_fixed: false, // fixed menu top of the screen. options (true) or (false)
                sticky_header: false, // menu fixed on top when scroll down down. options (true) or (false)
                sticky_header_height: 200 // sticky header height top of the screen. activate sticky header when meet the height. option change the height in px value.
            }
        });

        /* ======= Jquery Select Dropdowns ======= */
        $("select").select2({
            placeholder: $('#select_place_holder').val(),
            allowClear: true,
            width: '100%',
            maximumSelectionLength:Infinity
      
        });

        $('select').on('select2:select', function (e) {

            var ids={Text:this.name,Value:''};

         

            for(var i=0;i<e.target.length;i++)
            {
                if(e.target[i].selected)
                {
                    ids.Value+=  e.target[i].value+",";
                }
             
            }

            $.ajax({
                type: "POST",
                url:"/Base/GetLocationType",
                data:{model:ids},
                success:function(data)
                {
                    
                    if(data.length>=1)
                    {
                        var tt="";
                        if(ids.Text=="City")
                        {
                            tt="Town";
                        }
                        else if(ids.Text=="Town")
                        {
                            tt="Neighboard";
                        }

                        if(ids.Text=="City")
                        {
                            $("select[name=Town]").html('');
                            $("select[name=Neighboard]").html('');
                        }



                        $("select[name="+tt+"]").html('');

                        for (var i = 0; i < data.length; i++) {
                            $("select[name="+tt+"]").append("<option value="+data[i].Value+">"+data[i].Text+"</option>");

                        }
                        
                    }

                }
            });
            


        });
  
        //$('select').selectpicker({
        //    style: 'btn-default',
        //    size: 10,
      
        //    width:'100%',
        //    language: 'tr',
        //    liveSearch:true,
        //    showTick:true,
          

        //});


        $(".sb_variation").select2({
            placeholder: $('#select_place_holder').val(),
            allowClear: false,
            theme: "classic",
            width: '100%',
        });

        $(".search-select").select2({
            placeholder: $('#select_place_holder').val(),
            allowClear: false,
            theme: "classic",
            width: '100%',
        });

        /* ======= Featured Carousel 1 ======= */

        $('.featured-slider').owlCarousel({
            items: 3,
            dots: ($(".featured-slider .item").length > 1) ? false : false,
            loop: ($(".featured-slider .item").length > 1) ? true : false,
            nav: true,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 3,
                }
            }
        });

        $(".product-thumb").owlCarousel({
            dots: ($(".product-thumb .item").length > 1) ? false : false,
            loop: ($(".product-thumb .item").length > 1) ? true : false,
            autoWidth: true,
            nav: true,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 4,
                }
            }
        });

        $('.featured-slider-shop').owlCarousel({
            items: 4,
            dots: ($(".featured-slider-shop .item").length > 1) ? false : false,
            loop: ($(".featured-slider-shop .item").length > 1) ? true : false,
            nav: true,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 4,
                }
            }
        });

        /* ======= Featured Carousel 2 ======= */
        $('.featured-slider-1').owlCarousel({
            dots: ($(".featured-slider-1 .item").length > 1) ? false : false,
            loop: ($(".featured-slider-1 .item").length > 1) ? true : false,
            margin: -10,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: true
                },
                600: {
                    items: 2,
                    nav: true
                },
                1000: {
                    items: 3,
                    nav: true,
                    loop: false
                }
            }
        });

        /* ======= Featured  Carousel 3 ======= */
        $('.featured-slider-3').owlCarousel({
            dots: ($(".featured-slider-3 .item").length > 1) ? false : false,
            loop: ($(".featured-slider-3 .item").length > 1) ? true : false,
            margin: 0,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: true
                },
                600: {
                    items: 1,
                    nav: true
                },
                1000: {
                    items: 1,
                    nav: true,
                    loop: false
                }
            }
        });

        /* ======= Category Carousel ======= */
        $('.category-slider').owlCarousel({
            loop: true,
            dots: false,
            margin: 0,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: true
                },
                600: {
                    items: 2,
                    nav: true
                },
                1000: {
                    items: 4,
                    nav: true,
                    loop: false
                }
            }
        });

        /* ======= Background Image Rotator Carousel ======= */
        $('.background-rotator-slider').owlCarousel({
            loop: false,
            dots: false,
            margin: 0,
            autoplay: true,
            mouseDrag: true,
            touchDrag: true,
            autoplayTimeout: 5000,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            nav: false,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 1,
                },
                1000: {
                    items: 1,
                }
            }
        });

        $(".owl-testimonial-2").owlCarousel({
            autoplay: true,
            autoplayTimeout: 3000,
            autoplayHoverPause: true,
            responsiveClass: true,
            dots: ($(".owl-testimonial-2 .item").length > 1) ? false : false,
            loop: ($(".owl-testimonial-2 .item").length > 1) ? true : false,
            items: 3,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 3,
                }
            },
            stopOnHover: true
        });


        $(".owl-testimonial-1").owlCarousel({
            autoplay: true,
            autoplayTimeout: 3000,
            autoplayHoverPause: true,
            dots: false,
            responsiveClass: true,
            loop: true,
            items: 2,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 2,
                }
            },
            stopOnHover: true
        });

        $(".clients-list").owlCarousel({
            dots: ($(".clients-list .item").length > 1) ? false : false,
            loop: ($(".clients-list .item").length > 1) ? true : false,
            nav: false,
            items: 5,
            autoplay: true,
            autoplayTimeout: 2000,
            autoplayHoverPause: true,
            responsive: {
                0: {
                    items: 2,
                },
                600: {
                    items: 4,
                },
                1000: {
                    items: 5,
                }
            }


        });



        /* ======= Single Ad Slider Carousel  ======= */
        $('.single-details').owlCarousel({
            dots: ($(".single-details .item").length > 1) ? false : false,
            loop: ($(".single-details .item").length > 1) ? true : false,
            margin: 0,
            autoplay: false,
            mouseDrag: true,
            touchDrag: true,
            responsiveClass: true, // Optional helper class. Add 'owl-reponsive-' + 'breakpoint' class to main element.
            nav: true,
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 1,
                },
                1000: {
                    items: 1,
                }
            }
        });

        /*==========  Single Page SLider With Thumb ==========*/
        $('#carousel').flexslider({
            animation: "slide",
            controlNav: false,
            animationLoop: false,
            slideshow: false,
            itemWidth: 160,
            itemMargin: 5,
            asNavFor: '#single-slider'
        });

        $('#single-slider').flexslider({
            animation: "slide",
            controlNav: false,
            animationLoop: false,
            slideshow: false,
            sync: "#carousel",

        });

    }

    /* ======= Profile Image Upload ======= */
    $(document).on('change', '.btn-file :file', function () {
        var input = $(this),
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [label]);
    });
    $(document).on('fileselect', '.btn-file :file', function (event, label) {
        var input = $(this).parents('.input-group').find(':text'),
            log = label;
        if (input.length) {
            input.val(log);
        }
    });


    /* ======= Masonry Grid System ======= */
    $('.posts-masonry').imagesLoaded(function () {
        $('.posts-masonry').isotope({
            layoutMode: 'masonry',
            transitionDuration: '0.3s'
        });
    });

    $(".small-box").click(function () {
        $('html, body').animate({
            scrollTop: $("#tab1default").offset().top
        }, 2000);
    });



    /*==========  Back To Top  ==========*/
    var offset = 300,
        offset_opacity = 1200,
        //duration of the top scrolling animation (in ms)
        scroll_top_duration = 700,
        //grab the "back to top" link
        $back_to_top = $('.cd-top');
    //hide or show the "back to top" link
    $(window).scroll(function () {
        ($(this).scrollTop() > offset) ? $back_to_top.addClass('cd-is-visible') : $back_to_top.removeClass('cd-is-visible cd-fade-out');
        if ($(this).scrollTop() > offset_opacity) {
            $back_to_top.addClass('cd-fade-out');
        }
    });
    //smooth scroll to top
    $back_to_top.on('click', function (event) {

        event.preventDefault();
        $('body,html').animate({
            scrollTop: 0,
        }, scroll_top_duration);
    });

    /*==========  Tooltip  ==========*/
    $('body').on('hover', '[data-toggle="tooltip"]', function () {

        $('[data-toggle="tooltip"]').tooltip();
        $(this).trigger('hover');


    });

    /*==========  Quick Overview Modal  ==========*/


    //Signup Kullanıcı Kaydet
    // Validating Registration process
    if ($('#sb-sign-form').length > 0) {
        $('#sb_register_msg').hide();
        $('#sb_register_redirect').hide();
        $('#sb-sign-form').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
        })
            .on('form:submit', function () {
                $('#sb_loading').show();
                // Ajax for Registration
                $('#sb_register_submit').hide();
                $('#sb_register_msg').show();
          
                $.post("/SignUp/Index", {
                    action: 'sb_register_user',
                    name: $("#name").val(),

                    tel: $("#tel").val()
                    , email: $("#email").val(),
                    sifre: $("#sifre").val(),
                    __RequestVerificationToken: $("input[name=__RequestVerificationToken]").val()


                }).done(function (response) {
                    var result=response;

         
                    $('#sb_loading').hide();
               
                    $('#sb_register_msg').hide();

                    if (result.Result) {
                        $('#sb_register_redirect').show();
                        window.location = "/SignUp/CeptenOnay/?userID="+response.ID;
                    }
                    else {
                        $('#sb_register_submit').show();
                        toastr.error(response, '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });

                    }
                });

                return false;
            });
    }

    //Cepten Onay
    $("#sb_cepten_onay").click(function(){
    
    
    
    });


    if ($('#sb-login-form').length > 0) {
        // Login Process
        $('#sb_login_msg').hide();
        $('#sb_login_redirect').hide();

        $('#sb-login-form').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
        })
            .on('form:submit', function () {
                $('#sb_loading').show();
                // Ajax for Registration
                $('#sb_login_submit').hide();
                $('#sb_login_msg').show();
                $.post("/Login/Index", { action: 'sb_login_user',email:$("#email").val(),sifre:$("#sifre").val() }).done(function (response) {
                    $('#sb_loading').hide();
                    $('#sb_login_msg').hide();
                    
                    if ($.trim(response) == '1') {
                        $('#sb_login_redirect').show();
                        window.location = "/";
                    }
                    else {
                        $('#sb_login_submit').show();
                        toastr.error(response, '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });

                    }
                });

                return false;
            });
    }

    /*// Forgot Password*/
    if ($('#sb-forgot-form').length > 0) {
        $('#sb_forgot_msg').hide();

        $('#sb-forgot-form').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
        })
            .on('form:submit', function () {
                // Ajax for Registration
                $('#sb_forgot_submit').hide();
                $('#sb_forgot_msg').show();
                $('#sb_loading').show();
                $.post(carspot_ajax_url, { action: 'sb_forgot_password', sb_data: $("form#sb-forgot-form").serialize(), }).done(function (response) {
                    $('#sb_loading').hide();
                    $('#sb_forgot_msg').hide();

                    if ($.trim(response) == '1') {
                        $('#sb_forgot_submit').show();
                        $('#sb_forgot_email').val('');
                        toastr.success($('#carspot_forgot_msg').val(), '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                        $('#myModal').modal('hide');
                    }
                    else {
                        $('#sb_forgot_submit').show();
                        toastr.error(response, '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });

                    }
                });

                return false;
            });
    }

    /*// Reset Password*/
    if ($('#sb-reset-password-form').length > 0) {
        $('#sb_reset_password_modal').modal('show');
        $('#sb_reset_password_msg').hide();

        $('#sb-reset-password-form').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
        })
            .on('form:submit', function () {
                if ($('#sb_new_password').val() != $('#sb_confirm_new_password').val()) {
                    toastr.error($('#adforest_password_mismatch_msg').val(), '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                    return false;
                }
                // Ajax for Registration
                $('#sb_reset_password_submit').hide();
                $('#sb_reset_password_msg').show();
                $('#sb_loading').show();
                $.post(carspot_ajax_url, { action: 'sb_reset_password', sb_data: $("form#sb-reset-password-form").serialize(), }).done(function (response) {
                    $('#sb_loading').hide();
                    $('#sb_reset_password_msg').hide();

                    var get_r = response.split('|');
                    if ($.trim(get_r[0]) == '1') {
                        toastr.success(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                        $('#sb_reset_password_modal').modal('hide');
                        $('#sb_reset_password_submit').show();
                        window.location = $('#login_page').val();
                    }
                    else {
                        $('#sb_reset_password_submit').show();
                        toastr.error(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });

                    }

                });

                return false;
            });
    }

    /*// Change Password*/
    $(document).on('click', '#change_pwd', function () {
        $('#sb_loading').show();
        $.post(carspot_ajax_url, { action: 'sb_change_password', sb_data: $("form#sb-change-password").serialize(), }).done(function (response) {
            $('#sb_loading').hide();
            var get_r = response.split('|');
            if ($.trim(get_r[0]) == '1') {
                toastr.success(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                $('#myModal').modal('hide');
                window.location = $('#login_page').val();
            }
            else {
                toastr.error(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            }
        });

    });

    var is_load_required = 0;
    /*// Add Post*/
    if ($('#ad_post_form').length > 0) {
        $('#ad_cat_sub_div').hide();
        $('#ad_cat_sub_sub_div').hide();
        $('#ad_cat_sub_sub_sub_div').hide();

        //ad_cat_sub_sub_sub_div
        $('#ad_country_sub_div').hide();
        $('#ad_country_sub_sub_div').hide();
        $('#ad_country_sub_sub_sub_div').hide();

        if ($('#is_update').val() != "") {
            var level = $('#is_level').val();
            if (level >= 2) {
                $('#ad_cat_sub_div').show();
            }
            if (level >= 3) {
                $('#ad_cat_sub_sub_div').show();
            }
            if (level >= 4) {
                $('#ad_cat_sub_sub_sub_div').show();
            }

            var country_level = $('#country_level').val();
            if (country_level >= 2) {
                $('#ad_country_sub_div').show();
            }
            if (country_level >= 3) {
                $('#ad_country_sub_sub_div').show();
            }
            if (country_level >= 4) {
                $('#ad_country_sub_sub_sub_div').show();
            }

        }









        //İLAN VER


        $('#ad_post_form').parsley().on('field:validated', function () {


        })
            .on('form:submit', function () {
                // Ad Post
                $('#sb_loading').show();

                var filestr="";
                for (var i = 0; i < files.length; i++) {
    
                    filestr+=files[i].guid+";";
                }
                var models=[];
                $(".postdetails .attr").each(function(){
                
                    if($(this).data("attribute-id")!=null)
                    {
                        var attributeid=$(this).data("attribute-id");
                        var text=$(this).val();
                        if(this.type=="select-one")
                        {

                            models.push({AttributeId:attributeid,SubAttributeId:$(this).val()});
                        }
                        else if($(this).hasClass("subattr"))
                        {
                            models.push({AttributeId:attributeid,Text:$(this).val(),SubAttributeId:$(this).val()});
                        }
                        else {
                            models.push({AttributeId:attributeid,Text:$(this).val()});
                        }
                    }
                
                });
        
                
                $.post("/Ilanver/", {ilanID:$("#ilanID").val(),
                    catID:document.getElementById("cat_selectilan").value,
                    attributes:models,
                    action: 'sb_ad_posting',
                    sb_data: $("form#ad_post_form").serialize(), 
                    is_update: $('#is_update').val(),
                    Title: $("#ad_title").val(), 
                    Files: filestr,
                    Desc: $("#Desc").val(),
                    Latidue:$("#ad_map_lat").val(),
                    Longtude:$("#ad_map_long").val()
                    ,FaceLink:$("#FaceLink").val(),
                    CityID:$("#city_id").val(),
                    TownID:$("#town_id").val(),
                    AreaID:$("#area_id").val(),
                    NeigborhoodID:$("#neighboard_id").val(),
                    price:$("#ad_price").val(),
                    UserName:$("#UserName").val(),
                    Tel1:$("#Tel1").val(),
                    Tel2:$("#Tel2").val(),
                    Link:$("#Link").val()
                
                })
                    .done(function (response) {

                    $('#sb_loading').hide();
                    if ($.trim(response) == "0") {
                        toastr.error($('#not_logged_in').val(), '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                    }
                    else if ($.trim(response) == "img_req") {
                        toastr.error($('#required_images').val(), '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                    }
                    else {
                        toastr.success($('#ad_updated').val(), '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                        setTimeout(function(){
                            window.location = "/"
                        },500);
                    }
                });

                return false;
            });

        /* Level 1 */
        $('#ad_cat').on('change', function () {
            $('#sb_loading').show();
            $.post(carspot_ajax_url, { action: 'sb_get_sub_cat', cat_id: $("#ad_cat").val(), }).done(function (response) {
                $('#sb_loading').hide();
                $("#ad_cat_sub").val('');
                $("#ad_cat_sub_sub").val('');
                $("#ad_cat_sub_sub_sub").val('');
                if ($.trim(response) != "") {
                    $('#ad_cat_id').val($("#ad_cat").val());
                    $('#ad_cat_sub_div').show();
                    $('#ad_cat_sub').html(response);
                    $('#ad_cat_sub_sub_div').hide();
                    $('#ad_cat_sub_sub_sub_div').hide();
                }
                else {
                    $('#ad_cat_sub_div').hide();
                    $('#ad_cat_sub_sub_div').hide();
                    $('#ad_cat_sub_sub_sub_div').hide();

                }
                /*For Category Templates*/
                getCustomTemplate(carspot_ajax_url, $("#ad_cat").val(), $("#is_update").val(), true);
                /*For Category Templates*/

            });
        });

        /* Level 2 */
        $('#ad_cat_sub').on('change', function () {
            $('#sb_loading').show();
            $.post(carspot_ajax_url, { action: 'sb_get_sub_cat', cat_id: $("#ad_cat_sub").val(), }).done(function (response) {
                $('#sb_loading').hide();
                $("#ad_cat_sub_sub").val('');
                $("#ad_cat_sub_sub_sub").val('');
                if ($.trim(response) != "") {
                    $('#ad_cat_id').val($("#ad_cat_sub").val());
                    $('#ad_cat_sub_sub_div').show();
                    $('#ad_cat_sub_sub').html(response);
                    $('#ad_cat_sub_sub_sub_div').hide();
                }
                else {
                    $('#ad_cat_sub_sub_div').hide();
                    $('#ad_cat_sub_sub_sub_div').hide();
                }
                /*For Category Templates*/
                getCustomTemplate(carspot_ajax_url, $("#ad_cat_sub").val(), $("#is_update").val());
                /*For Category Templates*/
            });
        });

        /* Level 3 */
        $('#ad_cat_sub_sub').on('change', function () {
            $('#sb_loading').show();
            $.post(carspot_ajax_url, { action: 'sb_get_sub_cat', cat_id: $("#ad_cat_sub_sub").val(), }).done(function (response) {
                $('#sb_loading').hide();
                $("#ad_cat_sub_sub_sub").val('');
                if ($.trim(response) != "") {
                    $('#ad_cat_id').val($("#ad_cat_sub_sub").val());
                    $('#ad_cat_sub_sub_sub_div').show();
                    $('#ad_cat_sub_sub_sub').html(response);
                }
                else {
                    $('#ad_cat_sub_sub_sub_div').hide();
                }
                /*For Category Templates*/
                getCustomTemplate(carspot_ajax_url, $("#ad_cat_sub_sub").val(), $("#is_update").val());
                /*For Category Templates*/

            });
        });

        /* Level 4 */
        $('#ad_cat_sub_sub_sub').on('change', function () {
            $('#ad_cat_id').val($("#ad_cat_sub_sub_sub").val());
            /*For Category Templates*/
            getCustomTemplate(carspot_ajax_url, $("#ad_cat_sub_sub_sub").val(), $("#is_update").val());
            /*For Category Templates*/

        });







        //Countries
        /* Level 1 */
        $('#ad_country').on('change', function () {
            $('#sb_loading').show();
            $.post(carspot_ajax_url, { action: 'sb_get_sub_states', cat_id: $("#ad_country").val(), }).done(function (response) {
                $('#sb_loading').hide();
                $("#ad_country_states").val('');
                $("#ad_cat_sub_sub").val('');
                $("#ad_cat_sub_sub_sub").val('');
                if ($.trim(response) != "") {
                    $('#ad_country_id').val($("#ad_cat").val());
                    $('#ad_country_sub_div').show();
                    $('#ad_country_states').html(response);
                    $('#ad_cat_sub_sub_div').hide();
                    $('#ad_country_sub_sub_sub_div').hide();
                }
                else {
                    $('#ad_country_sub_div').hide();
                    $('#ad_cat_sub_sub_div').hide();
                    $('#ad_country_sub_sub_sub_div').hide();

                }

            });
        });


        /* Level 2 */
        $('#ad_country_states').on('change', function () {
            $('#sb_loading').show();
            $.post(carspot_ajax_url, { action: 'sb_get_sub_states', cat_id: $("#ad_country_states").val(), }).done(function (response) {
                $('#sb_loading').hide();
                $("#ad_country_cities").val('');
                $("#ad_country_towns").val('');
                if ($.trim(response) != "") {
                    $('#ad_country_id').val($("#ad_country_states").val());
                    $('#ad_country_sub_sub_div').show();
                    $('#ad_country_cities').html(response);
                    $('#ad_country_sub_sub_sub_div').hide();
                }
                else {
                    $('#ad_country_sub_sub_div').hide();
                    $('#ad_country_sub_sub_sub_div').hide();
                }
            });
        });

        /* Level 3 */
        $('#ad_country_cities').on('change', function () {
            $('#sb_loading').show();
            $.post(carspot_ajax_url, { action: 'sb_get_sub_states', cat_id: $("#ad_country_cities").val(), }).done(function (response) {
                $('#sb_loading').hide();
                $("#ad_country_towns").val('');
                if ($.trim(response) != "") {
                    $('#ad_country_id').val($("#ad_country_cities").val());
                    $('#ad_country_sub_sub_sub_div').show();
                    $('#ad_country_towns').html(response);
                }
                else {
                    $('#ad_country_sub_sub_sub_div').hide();
                }
            });
        });


    }




    // select profile tabs
    $(document).on('click', '.messages_actions', function () {
        var sb_action = $(this).attr('sb_action');
        if (sb_action != "") {
            //$('.dashboard-menu-container ul li').removeClass('active');
            //$(this).closest("li").addClass('active');
            $('#sb_loading').show();
            $.post(carspot_ajax_url, { action: sb_action }).done(function (response) {
                $('#sb_loading').hide();
                $('#carspot_res').html(response);
                $('[data-toggle="tooltip"]').tooltip();
                $('[data-toggle=confirmation]').confirmation({
                    rootSelector: '[data-toggle=confirmation]',
                    // other options
                });


            });
        }
    });
    $('.menu-name, .profile_tabs').on('click', function () {
        var sb_action = $(this).attr('sb_action');
        if (sb_action != "") {
            $('.dashboard-menu-container ul li').removeClass('active');
            $(this).closest("li").addClass('active');
            $('#sb_loading').show();
            $.post(carspot_ajax_url, { action: sb_action }).done(function (response) {
                $('#sb_loading').hide();
                $('#carspot_res').html(response);
                if (sb_action != "my_msgs") {
                    $('.posts-masonry').imagesLoaded(function () {
                        $('.posts-masonry').isotope({
                            layoutMode: 'masonry',
                            transitionDuration: '0.3s',
                        });
                    });

                }
                $('[data-toggle="tooltip"]').tooltip();
                $('[data-toggle=confirmation]').confirmation({
                    rootSelector: '[data-toggle=confirmation]',
                    // other options
                });


            });
        }
    });

    // Update Profile
    $(document).on('click', '#sb_user_profile_update', function () {

        // Ajax for Update profile
        $('#sb_loading').show();
        $.post(carspot_ajax_url, { action: 'sb_update_profile', sb_data: $("form#sb_update_profile").serialize(), }).done(function (response) {
            $('#sb_loading').hide();

            if ($.trim(response) == '1') {
                $('.sb_put_user_name').html($('input[name=sb_user_name]').val());
                $('.sb_put_user_address').html($('input[name=sb_user_address]').val());
                $('.sb_user_type').html($('select[name=sb_user_type]').val());
                toastr.success($('#carspot_profile_msg').val(), '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                $('body,html').animate({
                    scrollTop: 0,
                }, scroll_top_duration);
            }
            else {
                $('#sb_forgot_submit').show();
                toastr.error(response, '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            }
        });

    });

    // Upload user profile picture 
    $('body').on('change', '.sb_files-data', function (e) {

        var fd = new FormData();
        var files_data = $('.form-group .sb_files-data');

        $.each($(files_data), function (i, obj) {
            $.each(obj.files, function (j, file) {
                fd.append('my_file_upload[' + j + ']', file);
            });
        });

        fd.append('action', 'upload_user_pic');
        $('#sb_loading').show();
        $.ajax({
            type: 'POST',
            url: carspot_ajax_url,
            data: fd,
            contentType: false,
            processData: false,
            success: function (res) {
                $('#sb_loading').hide();
                var res_arr = res.split("|");
                if ($.trim(res_arr[0]) == "1") {
                    $('#user_dp').attr('src', res_arr[1]);
                    $('#img-upload').attr('src', res_arr[1]);
                }
                else {
                    toastr.error(res_arr[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                }

            }
        });


    });


    if ($('#is_sub_active').val() == "1") {
        /*images uplaod*/
        sbDropzone_image();

    }
    /*Make Post on blur of title field*/
    $('#ad_title').on('blur', function () {
        if ($('#is_update').val() == "") {
            $.post(carspot_ajax_url, { action: 'post_ad', title: $('#ad_title').val(), is_update: $('#is_update').val(), }).done(function (response) {

            });
        }

    });


    if ($('#facebook_key').val() != "" && $('#google_key').val() != "") {
        // Hello JS
        hello.init({
            facebook: $('#facebook_key').val(),
            google: $('#google_key').val(),
        }, { redirect_uri: $('#redirect_uri').val() });
    }
    else if ($('#facebook_key').val() != "" && $('#google_key').val() == "") {
        // Hello JS
        hello.init({
            facebook: $('#facebook_key').val(),
        }, { redirect_uri: $('#redirect_uri').val() });
    }
    else if ($('#google_key').val() != "" && $('#facebook_key').val() == "") {
        // Hello JS
        hello.init({
            google: $('#google_key').val(),
        }, { redirect_uri: $('#redirect_uri').val() });
    }


    // Hello JS Hander
    $('.form-grid a.btn-social').on('click', function () {
        hello.on('auth.login', function (auth) {
            console.log(auth);
            $('#sb_loading').show();
            // Call user information, for the given network
            hello(auth.network).api('me').then(function (r) {
                if ($('#get_action').val() == 'login') {
                    $.post(carspot_ajax_url, { action: 'sb_social_login', email: r.email, key_code: $('#nonce').val() }).done(function (response) {

                        var get_r = response.split('|');
                        if ($.trim(get_r[0]) == '1') {
                            $('#nonce').val(get_r[1]);
                            if ($.trim(get_r[2]) == '1') {
                                toastr.success(get_r[3], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                                window.location = $('#profile_page').val();
                            }
                            else {
                                toastr.error(get_r[3], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                            }

                        }

                    });

                }
                else {
                    $('#sb_reg_name').val(r.name);
                    $('#sb_reg_email').val(r.email);
                }
                $('#sb_loading').hide();
            });
        });
    });

    if ($('#is_sub_active').val() == "1") {
        /* Tags*/
        carspot_inputTags();
    }


    // Single Ad JS
    /* ======= Show Number ======= */
    $('.number').click(function () {
        $(this).find('span').text($(this).data('last'));
    });


    /* ======= Ad Location ======= */
    if ($('#lat').length > 0) {
        var lat = $('#lat').val();
        var lon = $('#lon').val();
        var map = "";
        var latlng = new google.maps.LatLng(lat, lon);
        var myOptions = {
            zoom: 13,
            center: latlng,
            scrollwheel: false,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            size: new google.maps.Size(480, 240)
        }
        map = new google.maps.Map(document.getElementById("itemMap"), myOptions);
        var marker = new google.maps.Marker({
            map: map,
            position: latlng
        });
    }

    // Report Ad
    $('#sb_mark_it').on('click', function () {
        $('#sb_loading').show();
        $.post(carspot_ajax_url, { action: 'sb_report_ad', option: $('#report_option').val(), comments: $('#report_comments').val(), ad_id: $('#ad_id').val(), }).done(function (response) {
            $('#sb_loading').hide();
            var get_r = response.split('|');
            if ($.trim(get_r[0]) == '1') {
                toastr.success(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                $('.report-quote').modal('hide');
            }
            else {
                toastr.error(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            }

        });
    });

    // Add to favourites
    $('#ad_to_fav,.save-ad').on('click', function () {
        $('#sb_loading').show();
        $.post(carspot_ajax_url, { action: 'sb_fav_ad', ad_id: $(this).attr('data-adid'), }).done(function (response) {
            $('#sb_loading').hide();
            var get_p = response.split('|');
            if ($.trim(get_p[0]) == '1') {
                toastr.success(get_p[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            }
            else {
                toastr.error(get_p[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            }

        });
    });


    // Delete  Ad
    $('body').on('hover', '.remove_fav_ad', function (e) {
        $(this).confirmation({
            rootSelector: '[data-toggle=confirmation]',
            // other options
        });
    });
    // Remove to favourites
    $('body').on('click', '.remove_fav_ad', function (e) {
        var id = $(this).attr('data-adid');
        $.post(carspot_ajax_url, { action: 'sb_fav_remove_ad', ad_id: $(this).attr('data-adid'), }).done(function (response) {
            var get_r = response.split('|');
            if ($.trim(get_r[0]) == '1') {
                $('body').find('#holder-' + id).remove();
                toastr.success(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            }
            else {
                toastr.error(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            }

        });
    });

    // Send message to ad owner
    if ($('#send_message_pop').length > 0) {

        $('#send_message_pop').parsley().on('field:validated', function () {
        })
            .on('form:submit', function () {
                $('#sb_loading').show();
                $.post(carspot_ajax_url, { action: 'sb_send_message', sb_data: $("form#send_message_pop").serialize(), }).done(function (response) {
                    $('#sb_loading').hide();
                    var get_r = response.split('|');
                    if ($.trim(get_r[0]) == '1') {
                        toastr.success(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                        $('#sb_forest_message').val('');
                        $(".close").trigger("click");
                    }
                    else {
                        toastr.error(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                    }
                });
                return false;
            });

    }

    $('body').on('click', '.user_list', function () {
        $('#sb_loading').show();
        $('.message-history-active').removeClass('message-history-active');
        $(this).addClass('message-history-active');
        var second_user = $(this).attr('second_user');
        var inbox = $(this).attr('inbox');
        var prnt = 'no';
        if (inbox == 'yes') {
            prnt = 'yes';
        }
        var cid = $(this).attr('cid');
        $('#' + second_user + '_' + cid).html('');
        $.post(carspot_ajax_url, { action: 'sb_get_messages', ad_id: cid, user_id: second_user, receiver: second_user, inbox: prnt }).done(function (response) {
            $('#usr_id').val(second_user);
            $('#rece_id').val(second_user);
            $('#msg_receiver_id').val(second_user);
            $('#ad_post_id').val(cid)
            $('#sb_loading').hide();
            $('#messages').html(response);
        });
    });

    $('body').on('click', '#send_msg', function () {
        $('#send_message').parsley().on('field:validated', function () {
        })
            .on('form:submit', function () {
                var inbox = $('#send_msg').attr('inbox');
                var prnt = 'no';
                if (inbox == 'yes') {
                    prnt = 'yes';
                }

                $('#sb_loading').show();
                $.post(carspot_ajax_url, { action: 'sb_send_message', sb_data: $("form#send_message").serialize(), }).done(function (response) {
                    var get_r = response.split('|');
                    if ($.trim(get_r[0]) == '1') {
                        toastr.success(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                        $('#sb_forest_message').val('');
                        $.post(carspot_ajax_url, { action: 'sb_get_messages', ad_id: $("#ad_post_id").val(), user_id: $('#usr_id').val(), inbox: prnt }).done(function (response) {
                            $('#sb_loading').hide();
                            $('#messages').html(response);
                            $('.message-details .list-wraps').scrollTop(20000).perfectScrollbar('update');
                        });
                    }
                    else {
                        toastr.error(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                    }
                });
                return false;
            });
    });

    // Delete  Ad
    $('body').on('hover', '.remove_ad', function (e) {
        $(this).confirmation({
            rootSelector: '[data-toggle=confirmation]',
            // other options
        });
    });
    // Delete  Ad
    $('body').on('click', '.remove_ad', function (e) {

        $(this).confirmation({
            rootSelector: '[data-toggle=confirmation]',
            // other options
        });
        $('#sb_loading').show();
        var id = $(this).attr('data-adid');
        $.post(carspot_ajax_url, { action: 'sb_remove_ad', ad_id: $(this).attr('data-adid'), }).done(function (response) {
            $('#sb_loading').hide();
            var get_r = response.split('|');
            if ($.trim(get_r[0]) == '1') {
                $('body').find('#holder-' + id).remove();
                toastr.success(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            }
            else {
                toastr.error(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            }

        });
    });

    // My ads pagination
    $('body').on('click', '.sb_page', function () {
        $('#sb_loading').show();
        var this_action = 'my_ads';
        if ($(this).attr('ad_type') == 'yes') {
            this_action = 'my_fav_ads';
        }
        $.post(carspot_ajax_url, { action: this_action, paged: $(this).attr('page_no'), }).done(function (response) {
            $('#sb_loading').hide();
            $('#carspot_res').html(response);
            $('body,html').animate({
                scrollTop: 200,
            }, scroll_top_duration);

            $('.posts-masonry').imagesLoaded(function () {
                $('.posts-masonry').isotope({
                    layoutMode: 'masonry',
                    transitionDuration: '0.3s',
                });
            });



        });

    });
    // Load Messages
    $('body').on('click', '.get_msgs', function () {
        $('#sb_loading').show();
        $.post(carspot_ajax_url, { action: 'sb_load_messages', ad_id: $(this).attr('ad_msg'), }).done(function (response) {
            $('#sb_loading').hide();
            $('#carspot_res').html(response);
        });

    });

    var previous;

    // My ads pagination
    $('body').on('focus', '.ad_status', function () {
        previous = this.value;
    }).on('change', '.ad_status', function () {
        if ($(this).val() != "") {
            if (confirm($('#confirm_update').val())) {
                $('#sb_loading').show();
                $.post(carspot_ajax_url, { action: 'sb_update_ad_status', ad_id: $(this).attr('adid'), status: $(this).val(), }).done(function (response) {
                    $('#sb_loading').hide();
                    var get_r = response.split('|');
                    if ($.trim(get_r[0]) == '1') {
                        toastr.success(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                        previous = this.value;
                        $('.menu-name[sb_action="my_ads"]').get(0).click();
                    }
                    else {
                        toastr.error(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                    }
                });
            }
            else {
                $(this).val(previous)
            }
        }

    });

    // Add to Cart
    $('body').on('click', '.sb_add_cart', function () {
        $('#sb_loading').show();
        $.post(carspot_ajax_url, { action: 'sb_add_cart', product_id: $(this).attr('data-product-id'), qty: $(this).attr('data-product-qty'), }).done(function (response) {
            $('#sb_loading').hide();
            var get_r = response.split('|');
            if ($.trim(get_r[0]) == '1') {
                toastr.success(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                window.location = get_r[2];
            }
            else {
                toastr.error(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                window.location = get_r[2];
            }
        });

    });
    if ($('#is_sub_active').val() == "1") {

        $('[data-toggle=confirmation]').confirmation({
            rootSelector: '[data-toggle=confirmation]',

        });

    }

    if ($('#ad_description').length > 0) {


        $('#ad_description').jqte({
            link: false,
            unlink: false,
            formats: false,
            format: false,
            funit: false,
            fsize: false,
            fsizes: false,
            color: false,
            strike: false,
            source: false,
            sub: false,
            sup: false,
            indent: false,
            outdent: false,
            right: false,
            left: false,
            center: false,
            remove: false,
            rule: false,
            title: false,

        });

    }

    $('#sb_feature_ad').on('click', function () {
        $('#sb_loading').show();
        $.post(carspot_ajax_url, { action: 'sb_make_featured', ad_id: $(this).attr('aaa_id'), }).done(function (response) {
            $('#sb_loading').hide();
            var get_r = response.split('|');
            if ($.trim(get_r[0]) == '1') {
                toastr.success(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                location.reload();
            }
            else {
                toastr.error(get_r[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
            }
        });


    });

    $(document).on('click', '.ad_title_show', function () {
        var cur_ad_id = $(this).attr('cid');
        $('.sb_ad_title').hide();
        $('#title_for_' + cur_ad_id).show();

    });

    if ($('#msg_notification_on').val() != "" && $('#msg_notification_on').val() != 0 && $('#msg_notification_time').val() != "") {
        setInterval(function () {
            $.post(carspot_ajax_url, { action: 'sb_check_messages', new_msgs: $('#is_unread_msgs').val(), }).done(function (response) {
                var get_r = response.split('|');
                if ($.trim(get_r[0]) == '1') {
                    toastr.success(get_r[1], '', { timeOut: 5000, "closeButton": true, "positionClass": "toast-bottom-left" });
                    $('#is_unread_msgs').val(get_r[2]);
                    $('.msgs_count').html(get_r[2]);
                    $('.notify').html('<span class="heartbit"></span><span class="point"></span>');

                    $.post(carspot_ajax_url, { action: 'sb_get_notifications' }).done(function (notifications) {
                        $('.message-center').html(notifications);
                    });
                }
            });



        }, $('#msg_notification_time').val());
    }
    function carspot_inputTags() {
        $('#tags').tagsInput({
            'width': '100%',
            'height': '5px;',
            'defaultText': '',
        });
    }

    //Mileage
    if ($('#get_mileage').length > 0) {
        $('#get_mileage').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
        })
            .on('form:submit', function () {
                $('#sb_loading').show();
                return true;
            });
    }




    /*// Rate User */
    if ($('#user_ratting_form').length > 0) {

        $('#user_ratting_form').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
        })
            .on('form:submit', function () {
                // Ajax for Registration
                $('#sb_loading').show();

                $.post(carspot_ajax_url, {


                    action: 'sb_post_user_ratting', sb_data: $("form#user_ratting_form").serializeArray(), type: "POST", dataType: "json"
                }).done(function (response) {
                    $('#sb_loading').hide();

                    var res_arr = response.split("|");
                    if ($.trim(res_arr[0]) != "0") {
                        toastr.success(res_arr[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                        location.reload();
                    }
                    else {
                        toastr.error(res_arr[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });

                    }
                });

                return false;
            });
    }

    /*// Replay to Rator */
    if ($('#sb-reply-rating-form').length > 0) {

        $('#sb-reply-rating-form').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
        })
            .on('form:submit', function () {
                // Ajax for Registration
                $('#sb_loading').show();
                $.post(carspot_ajax_url, { action: 'sb_reply_user_rating', sb_data: $("form#sb-reply-rating-form").serialize(), }).done(function (response) {
                    $('#sb_loading').hide();

                    var res_arr = response.split("|");
                    if ($.trim(res_arr[0]) != "0") {
                        toastr.success(res_arr[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                        location.reload();
                    }
                    else {
                        toastr.error(res_arr[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });

                    }
                });

                return false;
            });
    }

    $('.clikc_reply').on('click', function () {
        $('#rator_name').html($(this).attr('data-rator-name'));
        $('#rator_reply').val($(this).attr('data-rator-id'));
    });

    /* Bidding System  */
    /*// Replay to Rator */
    if ($('#sb_bid_ad').length > 0) {

        $('#sb_bid_ad').parsley().on('field:validated', function () {
            var ok = $('.parsley-error').length === 0;
        })
            .on('form:submit', function () {
                // Ajax for Registration
                $('#sb_loading').show();
                $.post(carspot_ajax_url, { action: 'sb_submit_bid', sb_data: $("form#sb_bid_ad").serialize(), }).done(function (response) {
                    $('#sb_loading').hide();

                    var res_arr = response.split("|");
                    if ($.trim(res_arr[0]) != "0") {
                        toastr.success(res_arr[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                        location.reload();
                    }
                    else {
                        toastr.error(res_arr[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });

                    }
                });

                return false;
            });
    }

    var $scrollbar = $('.bidding');
    $scrollbar.perfectScrollbar({
        maxScrollbarLength: 150,
    });
    $scrollbar.perfectScrollbar('update');

    $('form.custom-search-form select').on("select2:select", function (e) {
        $('#sb_loading').show();
        $(this).closest("form").submit();
        $('#sb_loading').hide();

    });


    /* For Cart Buttons */
    $('div.select-package a').on("click", function (e) {
        $('#sb_loading').show();
        var adonId = $(this).attr('data-adon-id');
        var ajax_url = $("input#carspot_ajax_url").val();
        $.post(carspot_ajax_url, { action: 'carspot_add_ad_adons', adon_id: adonId, }).done(function (response) {
            $('#sb_loading').hide();
            var res_arr = response.split("|");
            if ($.trim(res_arr[0]) == 1) {
                toastr.success(res_arr[2], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                $('#sb-quick-cart-price').html(res_arr[4]);

            }
            else {
                toastr.error(res_arr[2], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                $('#sb-quick-cart-price').html(res_arr[4]);
            }

            $("div.select-package a[data-adon-id=" + res_arr[1] + "]").html(res_arr[3]);


        });

    });


    function getArrayIndex(title,array,propname)
    {
    
        for (var i = 0; i < array.length; i++) {
    
            if(array[i][propname]==title)
                return i;
        }
    }

    //SÜRÜKLE BIRAK
    function sbDropzone_image() {

        Dropzone.autoDiscover = false;
        var acceptedFileTypes = "image/*"; //dropzone requires this param be a comma separated list
        var fileList = new Array;
        var i = 0;
        $("#dropzone").dropzone({
            addRemoveLinks: true,
            paramName: "my_file_upload",
            maxFiles:"30", //$('#sb_upload_limit').val(), //change limit as per your requirements
            acceptedFiles: '.jpeg,.jpg,.png',
            dictMaxFilesExceeded: $('#adforest_max_upload_reach').val(),
            /*acceptedFiles: acceptedFileTypes,*/
            url: "/IlanVer/ResimUpload" + "?action=upload_ad_images&is_update=" + $('#is_update').val(),
            parallelUploads: 1,
            dictDefaultMessage: $('#dictDefaultMessage').val(),
            dictFallbackMessage: $('#dictFallbackMessage').val(),
            dictFallbackText: $('#dictFallbackText').val(),
            dictFileTooBig: $('#dictFileTooBig').val(),
            dictInvalidFileType: $('#dictInvalidFileType').val(),
            dictResponseError: $('#dictResponseError').val(),
            dictCancelUpload: $('#dictCancelUpload').val(),
            dictCancelUploadConfirmation: $('#dictCancelUploadConfirmation').val(),
            dictRemoveFile: $('#dictRemoveFile').val(),
            dictRemoveFileConfirmation: null,
            dictMaxFilesExceeded: $('#dictMaxFilesExceeded').val(),
            init: function () {
                var thisDropzone = this;
                $.post("/IlanVer/ResimUpload", { action: 'get_uploaded_ad_images', is_update: $('#is_update').val() }).done(function (data) {
                    $.each(data, function (key, value) {
                        var mockFile = { name: value.name, size: value.size };
                        thisDropzone.options.addedfile.call(thisDropzone, mockFile);
                        thisDropzone.options.thumbnail.call(thisDropzone, mockFile, value.name);
                        $('a.dz-remove:eq(' + i + ')').attr("data-dz-remove", value.id);
                        i++;
                    });
                    if (i > 0)
                        $('.dz-message').hide();
                    else
                        $('.dz-message').show();
                });
                this.on("addedfile", function (file) { $('.dz-message').hide(); });
                this.on("success", function (file, responseText) {

                  
                    files.push({guid:responseText,orginalname:file});

                   
                    document.getElementById("files").value += responseText + ";";
                    var res_arr = responseText.split("|");
                    if ($.trim(res_arr[0]) != "0") {
                        $('a.dz-remove:eq(' + i + ')').attr("data-dz-remove", responseText);
                        i++;
                        $('.dz-message').hide();
                    }
                    else {
                        if (i == 0)
                            $('.dz-message').show();
                        this.removeFile(file);
                        toastr.error(res_arr[1], '', { timeOut: 2500, "closeButton": true, "positionClass": "toast-top-right" });
                    }
                });
                this.on("removedfile", function (file) {

                    var index=getArrayIndex(file,files,"orginalname");
                    var nameguid=  files[index].guid;
                 
                    var img_id = file._removeLink.attributes[2].value;
                    if (img_id != "") {
                        i--;
                        if (i == 0)
                            $('.dz-message').show();
                        $.post("/Ilanver/DeleteImage", {filename:nameguid }).done(function (response) {

                            if ($.trim(response) == "1") {
                                /*this.removeFile(file);*/
                                
                            }
                        });
                    }
                });

            },

        });
    }
    function getCustomTemplate(ajax_url, catId, updateId, is_top = false) {
        /*For Category Templates*/
        $.post(ajax_url, { action: 'sb_get_sub_template', 'cat_id': catId, 'is_update': updateId, }).done(function (response) {
            if ($.trim(response) != "") {
                $("#dynamic-fields").html(response);
                $('.skin-minimal .list li input').iCheck({
                    checkboxClass: 'icheckbox_minimal',
                    radioClass: 'iradio_minimal',
                    increaseArea: '20%'
                });
                $('#dynamic-fields select').select2();
                if ($('#input_ad_post_form_type').val() == 1) {
                    sbDropzone_image();
                }

                carspot_inputTags();
            }
            $('#sb_loading').hide();
            if (is_top) {
                $.post(carspot_ajax_url, { action: 'sb_get_car_total', }).done(function (cartTotal) {
                    $('#sb-quick-cart-price').html(cartTotal);
                });
            }

        });
        /*For Category Templates*/
    }


        $(document).on('change', '#ad_price_type', function () {
            if (this.value == "on_call" || this.value == "free" || this.value == "no_price") {
                $('#ad_price').attr("data-parsley-required", "false")
                $('#ad_price').val('');
                $('#ad_price').parent('div').hide();
            }
            else {
                $('#ad_price').attr("data-parsley-required", "true")
                $('#ad_price').parent('div').show();
            }
        });


    })(jQuery);
    jQuery(document).ready(function ($) {
        $("#comparison_button").trigger("click");
        $("#ad_price_type").trigger("change");
    });


    function carspot_validateEmail(sEmail) {
        var filter = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/;
        if (filter.test(sEmail)) {
            return true;
        }
        else {
            return false;
        }
    }
    function carspot_select_msg(cid, second_user, prnt) {
        jQuery('.message-history-active').removeClass('message-history-active');
        jQuery(document).find('#' + second_user + '_' + cid).html('');
        jQuery(document).find('#sb_' + second_user + '_' + cid).addClass('message-history-active');
        jQuery('#sb_loading').show();

        jQuery.post(jQuery('#carspot_ajax_url').val(), { action: 'sb_get_messages', ad_id: cid, user_id: second_user, receiver: second_user, inbox: prnt }).done(function (response) {
            jQuery('#usr_id').val(second_user);
            jQuery('#rece_id').val(second_user);
            jQuery('#msg_receiver_id').val(second_user);
            jQuery('#ad_post_id').val(cid)
            jQuery('#sb_loading').hide();
            jQuery('#messages').html(response);
        });
    }


