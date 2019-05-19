$(document).ready(function () {
	$('[data-toggle="tooltip"]').tooltip();

	var directions = {
		"rank": -1,
		"flag": -1,
		"username": 1,
		"time": -1,
		"kills": -1,
		"gems": -1,
		"accuracy": -1,
		"death-type": 1,
		"enemies-alive": -1,
		"homing": -1,
		"level-2": 1,
		"level-3": 1,
		"level-4": 1,
		"submit-date": -1
	};
	
	$(document).on("click", ".leaderboard-row", function () {
		var id = $(this).attr('id').split('-')[0];
		$("#" + id + "-expand").toggleClass('expand');

		$(".leaderboard-expand").not("#" + id + "-expand").removeClass('expand');
	});
	
	$(document).on("click", ".sorter", function () {
		var sorter = $(this);
		var sortValue = sorter.attr('sort');

		var sort = $('.sort').sort(function (a, b) {
			if (sortValue === 'username' || sortValue === 'death-type' || sortValue === 'flag') {
				contentA = $(a).attr(sortValue).toLowerCase();
				contentB = $(b).attr(sortValue).toLowerCase();
			}
			else {
				contentA = parseInt($(a).attr(sortValue));
				contentB = parseInt($(b).attr(sortValue));
			}

			return (contentA < contentB ? -1 : contentA > contentB ? 1 : 0) * directions[sortValue];
		});

		directions[sortValue] *= -1;
		$(".leaderboard-body").html(sort);

		$('[data-toggle="tooltip"]').tooltip();
	});
});