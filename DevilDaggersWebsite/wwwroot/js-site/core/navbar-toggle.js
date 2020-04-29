$("#navbar-toggle").click(function () {
	if (!$('#navbar-collapse').hasClass("collapsing")) {
		if ($('#navbar-toggle').hasClass("collapsed")) {
			$('#navbar-toggle').removeClass('collapsed');
		}
		else {
			$('#navbar-toggle').addClass('collapsed');
		}
	}
});