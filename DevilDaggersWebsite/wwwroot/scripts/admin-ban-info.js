$(document).ready(function () {
	var directions = {
		"description": 1,
		"banned-account": 1,
		"responsible-account": 1
	};

	$(document).on("click", ".sorter", function () {
		var sorter = $(this);
		var sortValue = sorter.attr('sort');

		var sort = $('.sort').sort(function (a, b) {
			if (sortValue === 'description' || sortValue === 'banned-account' || sortValue === 'responsible-account') {
				contentA = $(a).attr(sortValue).toLowerCase();
				contentB = $(b).attr(sortValue).toLowerCase();
			}

			return (contentA < contentB ? -1 : contentA > contentB ? 1 : 0) * directions[sortValue];
		});

		directions[sortValue] *= -1;
		$(".ban-info-body").html(sort);

		$('[data-toggle="tooltip"]').tooltip();
	});
});