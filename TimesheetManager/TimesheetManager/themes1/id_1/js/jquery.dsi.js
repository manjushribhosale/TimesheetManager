
$(document).ready(function(){
	
	// ****************************************************************************************************
	// BACK TO TOP
	// HIDE BACK TO TOP ON LOAD
	$("#back-top").hide();
	
	// FADE IN BACK TO TOP
	$(function () {
		$(window).scroll(function () {
			if ($(this).scrollTop() > 100) {
				$('#back-top').fadeIn();
			} else {
				$('#back-top').fadeOut();
			}
		});

		// SCROLL BACK TO TOP ON CLICK
		$('#back-top a').click(function () {
			$('body,html').animate({
				scrollTop: 0
			}, 800);
			return false;
		});
	});
	// ****************************************************************************************************
	
	
	
	// ****************************************************************************************************
	// NAVIGATION ANIMATION
	jQuery.easing.def = 'easeOutBounce';
    // Get the ID of the body
    var parentID = $("body").attr("id");
    // Loop through the nav list items
    $("#nav li").each(function() {
        // compare IDs of the body and list-items
        var myID = $(this).attr("id");
        // only perform the change on hover if the IDs don't match (so the active link doesn't change on hover)
        if (myID != "n-" + parentID) {
            // for mouse actions
            $(this).children("a").hover(function() {
                // add a class to the list item so that additional styling can be applied to the <em> and the text
                $(this).addClass('over');
                // add in the span that will be faded in and out
                $(this).append("<span></span>");
                $(this).find("span").fadeIn(150);
				
			}, function() {
                $(this).removeClass('over');
                // fade out the span then remove it completely to prevent the animations from continuing to run if you move over different items quickly
                $(this).find("span").fadeOut(600, function() {
                    $(this).remove();
                });
            });
			
        }
    });
	// ****************************************************************************************************
	
});