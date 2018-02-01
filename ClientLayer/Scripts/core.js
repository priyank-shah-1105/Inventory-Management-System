$(document).ready(function () {

	$.ajaxSetup({
		cache: false
	});
 
    // After page load, basic jq functions
    $('.options-toggle').on('click', function (e) {
    	e.preventDefault();
    	$('.options-holder').toggleClass('closed');
        //$('.options-holder .col-sm-4').slideToggle('hidden');
      });

    // Sidebar Toggle
    $('.toggle-left-sidebar').on('click', function (e) {
    	e.preventDefault();
    	$('.left-sidebar').toggleClass('left-sidebar-open');
    	
    })

    $('.minibar-small').click(function(){
      $('.site-holder').toggleClass('mini-sidebar')
    });

   });
    

function randNum() {
	return ((Math.floor(Math.random() * (1 + 40 - 20))) + 20) * 1200;
}






