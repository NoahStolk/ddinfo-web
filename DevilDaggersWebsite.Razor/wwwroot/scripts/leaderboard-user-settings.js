$(document).ready(function () {
	var directions = {
		"rank": -1,
		"flag": -1,
		"username": 1,
		"time": -1,
		"e-dpi": -1,
		"dpi": -1,
		"in-game-sens": -1,
		"fov": -1,
		"hand": -1,
		"flash": -1,
		"gamma": -1,
		"legacy-audio": -1
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
			if (sortValue === 'username' || sortValue === 'flag') {
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
