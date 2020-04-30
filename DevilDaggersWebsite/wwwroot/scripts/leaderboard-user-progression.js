const maxDate = Date.now() + 86400000 * 7;

let getUrlParameter = function getUrlParameter(sParam) {
	let sPageURL = window.location.search.substring(1),
		sURLVariables = sPageURL.split('&'),
		sParameterName,
		i;

	for (i = 0; i < sURLVariables.length; i++) {
		sParameterName = sURLVariables[i].split('=');

		if (sParameterName[0] === sParam)
			return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
	}
};

$.getJSON("/Api/GetUserProgressionById?UserId=" + getUrlParameter("UserId"), function (data) {
	const pbs = [];
	let pbIndex = 0;
	$.each(data, function (key, pb) {
		const date = new Date(key);
		const deathType = getDeathType(date, pb);
		const accuracy = pb.ShotsFired === 0 ? 0 : pb.ShotsHit / pb.ShotsFired * 100;

		pbs.push([date, pb.Time / 10000, pb.Rank.toFixed(0), pb.Username, pb.Gems === 0 ? "?" : pb.Gems.toFixed(0), pb.Kills === 0 ? "?" : pb.Kills.toFixed(0), accuracy === 0 ? "?" : accuracy.toFixed(2) + "%", deathType.ColorCode, deathType.Name]);

		if (++pbIndex === Object.keys(data).length) {
			pbs.push([maxDate + 10000000000 /*Ugly way to make sure no dot is visible*/, pb.Time / 10000, pb.Rank.toFixed(0), pb.Username, pb.Gems === 0 ? "?" : pb.Gems.toFixed(0), pb.Kills === 0 ? "?" : pb.Kills.toFixed(0), accuracy === 0 ? "?" : accuracy.toFixed(2) + "%", deathType.ColorCode, deathType.Name]);
		}
	});

	// TODO: This is just to prevent the graph from crashing. Something else should be displayed instead.
	if (pbs.length === 0)
		return;

	const minDate = pbs[0][0] - 86400000 * 7; // Amount of milliseconds in a day * amount of days

	let minTime = 10000;
	let maxTime = 0;
	for (let i = 0; i < pbs.length; i++) {
		if (pbs[i][1] > maxTime)
			maxTime = pbs[i][1];
		if (pbs[i][1] < minTime)
			minTime = pbs[i][1];
	}
	minTime = Math.floor(minTime / 50) * 50;
	maxTime = Math.ceil(maxTime / 50) * 50 + 50;

	const chartName = "user-progression-chart";
	const chartId = "#" + chartName;
	const chart = createChart(chartName, pbs, minDate, maxDate, minTime, maxTime, (maxTime - minTime) / 50 + 1);

	$(chartId).bind('jqplotMouseMove', function (event, xy, axesData, neighbor, plot) {
		const closestData = getClosestDataToMouse(chart, xy, plot, minDate, maxDate, minTime, maxTime);

		if (!closestData)
			$("#highlighter").hide();
		else
			setHighlighter(closestData, xy);
	});
	$(chartId).bind('jqplotMouseLeave', function () {
		$("#highlighter").hide();
	});

	$(window).resize(function () {
		chart.replot();

		$(chartId).append('<table id="highlighter">');
		$('#highlighter').append('<tr><td>Date</td><td id="h-date"></td></tr>');
		$('#highlighter').append('<tr><td>Rank</td><td id="h-rank"></td></tr>');
		$('#highlighter').append('<tr><td>Time</td><td id="h-time"></td></tr>');
		$('#highlighter').append('<tr><td>Username</td><td id="h-username"></td></tr>');
		$('#highlighter').append('<tr><td>Gems</td><td id="h-gems"></td></tr>');
		$('#highlighter').append('<tr><td>Kills</td><td id="h-kills"></td></tr>');
		$('#highlighter').append('<tr><td>Accuracy</td><td id="h-accuracy"></td></tr>');
		$('#highlighter').append('<tr><td>Death type</td><td id="h-death-type"></td></tr>');
		$(chartId).append('</table>');
	});

	function setHighlighter(data, xy) {
		setHighlighterPosition(chart, data, xy, minTime, maxTime);

		// Values
		const date = new Date(data[0]);
		const dateString = monthShortNames[date.getMonth()] + " " + ("0" + date.getDate()).slice(-2) + " '" + date.getFullYear().toString().substring(2, 4);
		$('#h-date').html(dateString);
		$('#h-rank').html(data[2]);
		$('#h-time').html(data[1].toFixed(4));
		$('#h-username').html(data[3]);
		$('#h-gems').html(data[4]);
		$('#h-kills').html(data[5]);
		$('#h-accuracy').html(data[6]);
		$('#h-death-type').html(data[8]);

		setHighlighterStyle(data[1], data[6]);
	}
});