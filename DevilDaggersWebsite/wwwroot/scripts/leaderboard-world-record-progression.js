const minDate = new Date("2016-02-18T00:00:00Z");
const maxDate = Date.now();
const minTime = 450;
const maxTime = 1150; // Determine with current WR (ceil(wr / 50) * 50)

$.getJSON("/Api/GetWorldRecords", function (data) {
	const wrs = [];
	let wrIndex = 0;
	$.each(data, function (_key, wr) {
		const date = new Date(wr.DateTime);
		const deathType = getDeathType(date, wr.Entry);
		const accuracy = wr.Entry.ShotsFired === 0 ? 0 : wr.Entry.ShotsHit / wr.Entry.ShotsFired * 100;

		wrs.push([date, wr.Entry.Time / 10000, wr.Entry.Username, wr.Entry.Gems === 0 ? "?" : wr.Entry.Gems.toFixed(0), wr.Entry.Kills === 0 ? "?" : wr.Entry.Kills.toFixed(0), accuracy === 0 ? "?" : accuracy.toFixed(2) + "%", deathType.ColorCode, deathType.Name]);

		if (++wrIndex === data.length) {
			wrs.push([maxDate + 10000000000 /*Ugly way to make sure no dot is visible*/, wr.Entry.Time / 10000, wr.Entry.Username, wr.Entry.Gems === 0 ? "?" : wr.Entry.Gems.toFixed(0), wr.Entry.Kills === 0 ? "?" : wr.Entry.Kills.toFixed(0), accuracy === 0 ? "?" : accuracy.toFixed(2) + "%", deathType.ColorCode, deathType.Name]);
		}
	});

	const chartName = "world-record-progression-chart";
	const chartId = "#" + chartName;
	const chart = createChart(chartName, wrs, minDate, maxDate, minTime, maxTime, (maxTime - minTime) / 50 + 1);

	$(chartId).bind('jqplotMouseMove', function (_event, xy, _axesData, _neighbor, plot) {
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
		$('#h-time').html(data[1].toFixed(4));
		$('#h-username').html(data[2]);
		$('#h-gems').html(data[3]);
		$('#h-kills').html(data[4]);
		$('#h-accuracy').html(data[5]);
		$('#h-death-type').html(data[7]);

		setHighlighterStyle(data[1], data[6]);
	}
});