$(document).ready(function () {
	var directions = {
		"name": 1,
		"players": -1,
		"submits": -1,
		"last-played": 1,
		"created": -1,
		"bronze": -1,
		"silver": -1,
		"golden": -1,
		"devil": -1,
		"leviathan": -1,
		"world-record": -1
	};

	$(document).on("click", ".sorter", function () {
		var sorter = $(this);
		var sortValue = sorter.attr('sort');

		var sort = $('.sort').sort(function (a, b) {
			if (sortValue === 'name') {
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
