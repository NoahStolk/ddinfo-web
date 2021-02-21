$(function () {
	if (document.location.href.indexOf('#') > -1) {
		$(window).scrollTop($(window).scrollTop() - 70);
	}
});

// Select all links with hashes
$('a[href*="#"]')
	// Remove links that don't actually link to anything
	.not('[href="#"]')
	.not('[href="#0"]')
	.click(function (event) {
		// On-page links
		//if (
		//	location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '')
		//	&&
		//	location.hostname == this.hostname
		//) {
		// Figure out element to scroll to
		var target = $(this.hash);
		target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
		// Does a scroll target exist?
		if (target.length) {
			// Only prevent default if animation is actually gonna happen
			event.preventDefault();
			$('html, body').animate({
				scrollTop: target.offset().top - 70 // header height
			}, 1000, function () {
				// Callback after animation
				// Must change focus!
				var $target = $(target);
				$target.focus();
				if ($target.is(":focus")) { // Checking if the target was focused
					return false;
				} else {
					$target.attr('tabindex', '-1'); // Adding tabindex for elements not focusable
					$target.focus(); // Set focus again
				}
			});
		}
		//}
	});

$(window).scroll(function () {
	if ($(this).scrollTop() > 256) {
		$('.back-to-top').removeClass('transparent-on-start');
		$('.back-to-top').fadeIn();
	}
	else {
		$('.back-to-top').fadeOut();
	}
});

$(document).ready(function () {
	$('[data-toggle="tooltip"]').tooltip();
});

function copyToClipboard(text) {
	var $temp = $("<input>");
	$("body").append($temp);
	$temp.val(text).select();
	document.execCommand("copy");
	$temp.remove();
}
