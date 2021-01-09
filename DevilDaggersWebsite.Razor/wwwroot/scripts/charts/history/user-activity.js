$.getJSON("/api/leaderboard-history/user-activity?UserId=" + getUrlParameter("UserId"), function (data) {
	const activity = [];
	let activityIndex = 0;
	let deathsPrevious = 0;
	let maxDeaths = 0;
	$.each(data, function (key, deaths) {
		const date = new Date(key);

		if (activityIndex === 0)
			deathsPrevious = deaths;

		const newDeaths = deaths - deathsPrevious;
		if (newDeaths < 300) {
			// Only apply when the numbers are realistic; sometimes people switch accounts which messes up statistics.
			activity.push([date, deaths - deathsPrevious]);

			if (newDeaths > maxDeaths) {
				maxDeaths = newDeaths;
			}
		}

		deathsPrevious = deaths;

		if (++activityIndex === Object.keys(data).length) {
			// Ugly way to make sure no dot is visible
			activity.push([maxDate + 10000000000, deaths]);
		}
	});

	const minDate = activity[0][0];

	const chartName = "user-activity-chart";
	const chartId = "#" + chartName;
	const highlighterName = "user-activity-highlighter";
	const highlighterId = "#" + highlighterName;
	const chart = createChart(chartName, activity, minDate, maxDate, 0, Math.ceil(maxDeaths / 100) * 100, 20, false);

	$(chartId).bind('jqplotMouseMove', function (_event, xy, _axesData, _neighbor, plot) {
		const closestData = getClosestDataToMouse(chart, xy, plot, minDate, maxDate, 0, maxDeaths);

		if (!closestData)
			$().hide();
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
		$(highlighterId).append('<tr><td>Date</td><td id="h-activity-date"></td></tr>');
		$(highlighterId).append('<tr><td>Deaths this period</td><td id="h-activity-deaths"></td></tr>');
		$(highlighterId).append('</tbody>');
		$(chartId).append('</table>');
	});

	function setHighlighter(data, xy) {
		setHighlighterPosition(chart, highlighterId, data, xy, 0, maxDeaths);

		const date = new Date(data[0]);
		const dateString = monthShortNames[date.getMonth()] + " " + ("0" + date.getDate()).slice(-2) + " '" + date.getFullYear().toString().substring(2, 4);
		$('#h-activity-date').html(dateString);
		$('#h-activity-deaths').html(data[1]);

		$(highlighterId).show();
	}
}); 