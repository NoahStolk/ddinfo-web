$(document).ready(function () {
	$(document).on("click", ".leaderboard-row", function () {
		var id = $(this).attr('id').split('-')[0];
		$("#" + id + "-expand").toggleClass('expand');

		$(".leaderboard-expand").not("#" + id + "-expand").removeClass('expand');
	});
});