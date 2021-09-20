$('.dropdown').on('show.bs.dropdown', function () {
	$(this).find('.dropdown-menu').first().stop(true, true).slideDown(200);
});

$('.dropdown').on('hide.bs.dropdown', function () {
	$(this).find('.dropdown-menu').first().stop(true, true).slideUp(200);
});
