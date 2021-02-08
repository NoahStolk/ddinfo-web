const minDate = new Date("2016-01-01T00:00:00Z");
const maxDate = Date.now();

$.getJSON("/api/leaderboard-history/world-records", function (data) {
	const wrs = [];
	let wrIndex = 0;
	$.each(data, function (_key, wr) {
		const date = new Date(wr.dateTime);
		const deathType = getDeathType(date, wr.entry);
		const accuracy = wr.entry.daggersFired === 0 ? 0 : wr.entry.daggersHit / wr.entry.daggersFired * 100;

		wrs.push([date, wr.entry.time / 10000, wr.entry.username, wr.entry.gems === 0 ? "?" : wr.entry.gems.toFixed(0), wr.entry.kills === 0 ? "?" : wr.entry.kills.toFixed(0), accuracy === 0 ? "?" : accuracy.toFixed(2) + "%", deathType.colorCode, deathType.name]);

		if (++wrIndex === data.length) {
			// Ugly way to make sure no dot is visible
			wrs.push([maxDate + 10000000000, wr.entry.time / 10000, '', '', '', '', deathType.colorCode, deathType.name]);
		}
	});

	let minTime = 10000;
	let maxTime = 0;
	for (let i = 0; i < wrs.length; i++) {
		if (wrs[i][1] > maxTime)
			maxTime = wrs[i][1];
		if (wrs[i][1] < minTime)
			minTime = wrs[i][1];
	}
	minTime = Math.floor(minTime / 50) * 50;
	maxTime = Math.ceil(maxTime / 50) * 50;

	const chartName = "world-record-progression-chart";
	const chartId = "#" + chartName;
	const highlighterName = "world-record-progression-highlighter";
	const highlighterId = "#" + highlighterName;
	const chart = createChart(chartName, wrs, minDate, maxDate, minTime, maxTime, (maxTime - minTime) / 50 + 1, true);

	$(chartId).bind('jqplotMouseMove', function (_event, xy, _axesData, _neighbor, plot) {
		const closestData = getClosestDataToMouse(chart, xy, plot, minDate, maxDate, minTime, maxTime);

		if (!closestData)
			$(highlighterId).hide();
		else
			setHighlighter(closestData, xy);
	});
	$(chartId).bind('jqplotMouseLeave', function () {
		$(highlighterId).hide();
	});

	$(window).resize(function () {
		chart.replot();

		$(chartId).append('<table class="highlighter" id="' + highlighterName + '">');
		$(highlighterId).append('<tbody>');
		$(highlighterId).append('<tr><td>Date</td><td id="h-date"></td></tr>');
		$(highlighterId).append('<tr><td>Time</td><td id="h-time"></td></tr>');
		$(highlighterId).append('<tr><td>Username</td><td id="h-username"></td></tr>');
		$(highlighterId).append('<tr><td>Gems</td><td id="h-gems"></td></tr>');
		$(highlighterId).append('<tr><td>Kills</td><td id="h-kills"></td></tr>');
		$(highlighterId).append('<tr><td>Accuracy</td><td id="h-accuracy"></td></tr>');
		$(highlighterId).append('<tr><td>Death type</td><td id="h-death-type"></td></tr>');
		$(highlighterId).append('</tbody>');
		$(chartId).append('</table>');
	});

	function setHighlighter(data, xy) {
		setHighlighterPosition(chart, highlighterId, data, xy, minTime, maxTime, false);

		const date = new Date(data[0]);
		const dateString = monthShortNames[date.getMonth()] + " " + ("0" + date.getDate()).slice(-2) + " '" + date.getFullYear().toString().substring(2, 4);
		$('#h-date').html(dateString);
		$('#h-time').html(data[1].toFixed(4));
		$('#h-username').html(data[2].substring(0, 16));
		$('#h-gems').html(data[3]);
		$('#h-kills').html(data[4]);
		$('#h-accuracy').html(data[5]);
		$('#h-death-type').html(data[7]);

		setHighlighterStyle(data[1], data[6]);

		$(highlighterId).show();
	}
});