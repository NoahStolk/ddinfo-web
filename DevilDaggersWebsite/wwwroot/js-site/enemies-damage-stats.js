$(document).on("click", ".enemy-expander", function () {
	var id = $(this).attr('id');
	$("#" + id + "-expand").toggleClass('expand');
	if ($("#" + id + "-expand").hasClass('expand')) {
		$(this).text("-");
	}
	else {
		$(this).text("+");
	}

	$(".enemy-expand").not("#" + id + "-expand").removeClass('expand');
});