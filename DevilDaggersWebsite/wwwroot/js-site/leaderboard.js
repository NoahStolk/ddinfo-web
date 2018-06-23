$(document).ready(function () {
	$(document).on("click", ".leaderboard-row", function () {
		var id = $(this).attr('id').split('-')[0];
		$("#" + id + "-expand").toggleClass('expand');

		$(".leaderboard-expand").not("#" + id + "-expand").removeClass('expand');
	});
	
	$(document).on("click", ".sorter", function () {
		var sorter = $(this);

		var sort = $('.sort').sort(function (a, b) {
			var contentA = parseInt($(a).attr(sorter.attr('sort')));
			var contentB = parseInt($(b).attr(sorter.attr('sort')));
			return (contentA < contentB) ? -1 : (contentA > contentB) ? 1 : 0;
		});

		$(".leaderboard-body").html(sort);
	});
});