(function($) {
    "use strict";
		var theme_path 	=	$('#theme_path').val();
		  $("#defualt" ).on('click',function(){
			  $("#defualt-color-css" ).attr("href", theme_path + "css/colors/defualt.css");
			  return false;
		  });
		   $("#green" ).on('click',function(){
			  $("#defualt-color-css" ).attr("href", theme_path + "css/colors/green.css");
			  return false;
		  });
		  
		  $("#purple" ).on('click',function(){
			  $("#defualt-color-css" ).attr("href", theme_path + "css/colors/purple.css");
			  return false;
		  });
		  
		  $("#blue" ).on('click',function(){
			  $("#defualt-color-css" ).attr("href", theme_path + "css/colors/blue.css");
			  return false;
		  });
		  $("#gold" ).on('click',function(){
			  $("#defualt-color-css" ).attr("href", theme_path + "css/colors/gold.css");
			  return false;
		  });
		  $(".picker_close").click(function(){
			  	$("#choose_color").toggleClass("position");
			  
		   });
		  
})(jQuery);
