$(document).ready(function () {
	$(document).on("click", ".leaderboard-row", function () {
		var id = $(this).attr('id').split('-')[0];

		if ($("#" + id + "-expand").css('visibility') != 'visible')
			$("#" + id + "-expand").css('visibility', 'visible');
		else
			$("#" + id + "-expand").css('visibility', 'collapse');
	});
});