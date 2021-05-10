$(document).ready(function () {
	const worldRecordHolderSortDirections = {
		"username": 1,
		"total-time-held": 1,
		"longest-time-held": 1,
		"world-record-count": -1,
		"first-held": -1,
		"last-held": -1
	};

	$(document).on("click", ".world-record-holder-sorter", function () {
		var sorter = $(this);
		var sortValue = sorter.attr('sort');

		var sort = $('.world-record-holder-sort').sort(function (a, b) {
			if (sortValue === 'username') {
				contentA = $(a).attr(sortValue).toLowerCase();
				contentB = $(b).attr(sortValue).toLowerCase();
			}
			else {
				contentA = parseInt($(a).attr(sortValue));
				contentB = parseInt($(b).attr(sortValue));
			}

			return (contentA < contentB ? -1 : contentA > contentB ? 1 : 0) * worldRecordHolderSortDirections[sortValue];
		});

		worldRecordHolderSortDirections[sortValue] *= -1;
		$(".world-record-holder-sort-body").html(sort);
	});

	const worldRecordSortDirections = {
		"username": 1,
		"time": -1,
		"duration": 1,
		"improvement": -1,
		"game-version": 1
	};

	$(document).on("click", ".world-record-sorter", function () {
		var sorter = $(this);
		var sortValue = sorter.attr('sort');

		var sort = $('.world-record-sort').sort(function (a, b) {
			if (sortValue === 'username' || sortValue === 'game-version') {
				contentA = $(a).attr(sortValue).toLowerCase();
				contentB = $(b).attr(sortValue).toLowerCase();
			}
			else {
				contentA = parseInt($(a).attr(sortValue));
				contentB = parseInt($(b).attr(sortValue));
			}

			return (contentA < contentB ? -1 : contentA > contentB ? 1 : 0) * worldRecordSortDirections[sortValue];
		});

		worldRecordSortDirections[sortValue] *= -1;
		$(".world-record-sort-body").html(sort);
	});
});
