const dayLengthInMilliseconds = 1000 * 60 * 60 * 24;

$.getJSON("/api/leaderboard-history/user-activity?UserId=" + getUrlParameter("UserId"), function (data) {
	const activity = [];
	let activityIndex = 0;
	let deathsPrevious = 0;
	let datePrevious;
	let maxDeaths = 0;
	$.each(data, function (key, deaths) {
		const dateCurrent = new Date(key);
		if (!datePrevious)
			datePrevious = dateCurrent;

		if (activityIndex === 0)
			deathsPrevious = deaths;

		const periodLengthInMilliseconds = Math.abs(dateCurrent - datePrevious);
		const periodLengthInDays = dayLengthInMilliseconds / periodLengthInMilliseconds;

		const newDeaths = deaths - deathsPrevious;
		if (newDeaths < 300 * periodLengthInDays) {
			// Only apply when the numbers are realistic (less than 300 deaths per day); sometimes people switch accounts which messes up statistics.

			const approximateDailyDeaths = newDeaths * periodLengthInDays;
			activity.push([dateCurrent, approximateDailyDeaths, newDeaths, datePrevious]);

			if (newDeaths > maxDeaths) {
				maxDeaths = newDeaths;
			}
		}

		deathsPrevious = deaths;
		datePrevious = dateCurrent;

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

		const dateCurrent = new Date(data[0]);
		const yearCurrent = dateCurrent.getFullYear();
		const monthCurrent = dateCurrent.getMonth();
		const dayCurrent = dateCurrent.getDate();

		const datePrevious = new Date(data[3]);
		const yearPrevious = datePrevious.getFullYear();
		const monthPrevious = datePrevious.getMonth();
		const dayPrevious = datePrevious.getDate();

		let dateString;

		// Only show current date if the difference was less than 25 hours.
		if (Math.abs(dateCurrent - datePrevious) < dayLengthInMilliseconds + 1000 * 60 * 60)
			dateString = getDateString(dayCurrent, monthCurrent, yearCurrent);
		else
			dateString = getDateString(dayPrevious, monthPrevious, yearPrevious) + ' - ' + getDateString(dayCurrent, monthCurrent, yearCurrent);

		$('#h-activity-date').html(dateString);
		$('#h-activity-deaths-per-day').html(data[1].toFixed(2));
		$('#h-activity-deaths').html(data[2].toFixed(0));

		$(highlighterId).show();
	}

	function getDateString(day, month, year) {
		return monthShortNames[month] + " " + ("0" + day).slice(-2) + " '" + year.toString().substring(2, 4);
	}
}); 