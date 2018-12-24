$(document).on("click", "#save", function () {
	var password = $("#pass").text();
	var bans = $("#textarea-bans").text();
	var donators = $("#textarea-donators").text();
	var flags = $("#textarea-flags").text();

	$.ajax({
		type: 'POST',
		url: "/Admin/Save",
		data: {
			password: password,
			bans: bans,
			donators: donators,
			flags: flags
		},
		success: function (data, textStatus, jqXHR) {
			//data - response from server
		},
		error: function (jqXHR, textStatus, errorThrown) {

		}
	});
});