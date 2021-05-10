const dayLengthInMilliseconds = 1000 * 60 * 60 * 24;

$.getJSON("/api/leaderboard-history/user-activity?UserId=" + getUrlParameter("id"), function (data) {
	const activity = [];
	let activityIndex = 0;
	let deathsNext = 0;
	let maxDeaths = 0; // Used for determining the highest value of the graph.
	let i = 0;
	$.each(data, function (_, deaths) {
		const length = Object.keys(data).length;

		const dateStart = new Date(Object.keys(data)[i]);
		const dateEnd = i >= length - 1 ? Date.now() : new Date(Object.keys(data)[i + 1]);
		if (activityIndex === length - 1)
			deathsNext = deaths;
		else
			deathsNext = Object.values(data)[i + 1];

		const periodLengthInMilliseconds = Math.abs(dateEnd - dateStart);
		const periodLengthInDays = Math.max(1, periodLengthInMilliseconds / dayLengthInMilliseconds);

		let deathsInThisPeriod = deathsNext - deaths;
		let approximateDailyDeaths = periodLengthInDays === 0 ? 0 : deathsInThisPeriod / periodLengthInDays;

		// Workaround for pocket and Ravenholmzombies who switched accounts.
		if (approximateDailyDeaths > 234) {
			deathsInThisPeriod = 0;
			approximateDailyDeaths = 0;
		}

		activity.push([dateStart, approximateDailyDeaths, deathsInThisPeriod, dateEnd]);

		if (approximateDailyDeaths > maxDeaths) {
			maxDeaths = approximateDailyDeaths;
		}

		if (++activityIndex === length) {
			// Ugly way to make sure no dot is visible
			activity.push([maxDate + 10000000000, deaths]);
		}

		i++;
	});

	if (activity.length === 0)
		activity.push([new Date(2016, 0, 1), 0]);

	const minDate = activity[0][0];

	const chartName = "user-activity-chart";
	const chartId = "#" + chartName;
	const highlighterName = "user-activity-highlighter";
	const highlighterId = "#" + highlighterName;
	const chart = createChart(chartName, activity, minDate, maxDate, 0, Math.ceil(maxDeaths / 50) * 50, 11, false);

	$(chartId).bind('jqplotMouseMove', function (_event, xy, _axesData, _neighbor, plot) {
		const closestData = getDataBasedOnMouseXPosition(chart, xy, plot, minDate, maxDate);

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
		$(highlighterId).append('<tr><td>Approximate deaths per day</td><td id="h-activity-deaths-per-day"></td></tr>');
		$(highlighterId).append('<tr><td>Total deaths</td><td id="h-activity-deaths"></td></tr>');
		$(highlighterId).append('</tbody>');
		$(chartId).append('</table>');
	});

	function setHighlighter(data, xy) {
		setHighlighterPosition(chart, highlighterId, data, xy, 0, maxDeaths, true);

		const dateStart = new Date(data[0]);
		const yearStart = dateStart.getFullYear();
		const monthStart = dateStart.getMonth();
		const dayStart = dateStart.getDate();

		const dateEnd = new Date(data[3]);
		const yearEnd = dateEnd.getFullYear();
		const monthEnd = dateEnd.getMonth();
		const dayEnd = dateEnd.getDate();

		let dateString = getDateString(dayStart, monthStart, yearStart) + ' - ' + getDateString(dayEnd, monthEnd, yearEnd);

		$('#h-activity-date').html(dateString);
		$('#h-activity-deaths-per-day').html(data[1].toFixed(2));
		$('#h-activity-deaths').html(data[2].toFixed(0));

		$(highlighterId).show();
	}

	function getDateString(day, month, year) {
		return monthShortNames[month] + " " + ("0" + day).slice(-2) + " '" + year.toString().substring(2, 4);
	}
}); 
