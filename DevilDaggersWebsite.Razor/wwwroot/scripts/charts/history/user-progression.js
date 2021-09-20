const maxDate = Date.now();

$.getJSON("/api/leaderboard-history/user-progression?userId=" + getUrlParameter("id"), function (data) {
	const pbs = [];
	let pbIndex = 0;
	$.each(data, function (key, pb) {
		const date = new Date(key);
		const deathType = getDeathType(date, pb);
		const accuracy = pb.daggersFired === 0 ? 0 : pb.daggersHit / pb.daggersFired * 100;

		pbs.push([date, pb.time / 10000, pb.rank.toFixed(0), pb.username, pb.gems === 0 ? "?" : pb.gems.toFixed(0), pb.kills === 0 ? "?" : pb.kills.toFixed(0), accuracy === 0 ? "?" : accuracy.toFixed(2) + "%", deathType.colorCode, deathType.name]);

		if (++pbIndex === Object.keys(data).length) {
			// Ugly way to make sure no dot is visible
			pbs.push([maxDate + 10000000000, pb.time / 10000, '', '', '', '', '', deathType.colorCode, deathType.name]);
		}
	});

	const minDate = pbs[0][0];

	let minTime = 10000;
	let maxTime = 0;
	for (let i = 0; i < pbs.length; i++) {
		if (pbs[i][1] > maxTime)
			maxTime = pbs[i][1];
		if (pbs[i][1] < minTime)
			minTime = pbs[i][1];
	}
	minTime = Math.floor(minTime / 50) * 50;
	maxTime = Math.ceil(maxTime / 50) * 50;

	const chartName = "user-progression-chart";
	const chartId = "#" + chartName;
	const highlighterName = "user-progression-highlighter";
	const highlighterId = "#" + highlighterName;
	const chart = createChart(chartName, pbs, minDate, maxDate, minTime, maxTime, (maxTime - minTime) / 50 + 1, true);

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
		$(highlighterId).append('<tr><td>Rank</td><td id="h-rank"></td></tr>');
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

		// Values
		const date = new Date(data[0]);
		const dateString = monthShortNames[date.getMonth()] + " " + ("0" + date.getDate()).slice(-2) + " '" + date.getFullYear().toString().substring(2, 4);
		$('#h-date').html(dateString);
		$('#h-rank').html(data[2]);
		$('#h-time').html(data[1].toFixed(4));
		$('#h-username').html(data[3].substring(0, 16));
		$('#h-gems').html(data[4]);
		$('#h-kills').html(data[5]);
		$('#h-accuracy').html(data[6]);
		$('#h-death-type').html(data[8]);

		setHighlighterStyle(data[1], data[7]);

		// Show
		$(highlighterId).show();
	}
});
