const deathNames = ["FALLEN", "SWARMED", "IMPALED", "GORED", "INFESTED", "OPENED", "PURGED", "DESECRATED", "SACRIFICED", "EVISCERATED", "ANNIHILATED", "INTOXICATED", "ENVENOMATED", "INCARNATED", "DISCARNATED", "BARBED"];
const deathColors = ["#DDDDDD", "#352710", "#433114", "#6E5021", "#DCCB00", "#976E2E", "#4E3000", "#804E00", "#AF6B00", "#837E75", "#478B41", "#99A100", "#657A00", "#FF0000", "#FF3131", "#771D00"];

$.getJSON("/api/leaderboard-statistics/death-types", function (data) {
	const deathCounts = [];
	$.each(data, function (_, deathCount) {
		deathCounts.push(deathCount);
	});

	let sum = deathCounts.reduce(function (a, b) {
		return a + b;
	});

	const chartName = "death-types-chart";
	const chartId = "#" + chartName;
	const highlighterName = "death-types-highlighter";
	const highlighterId = "#" + highlighterName;

	const chart = $.jqplot(chartName, [deathCounts], {
		animate: !$.jqplot.use_excanvas,
		seriesColors: deathColors,
		axes: {
			xaxis: {
				renderer: $.jqplot.CategoryAxisRenderer,
				ticks: deathNames
			},
			yaxis: {
				mark: 'outside',
				min: 0,
				max: 100000,
				numberTicks: 11
			}
		},
		seriesDefaults: {
			renderer: $.jqplot.BarRenderer,
			rendererOptions: {
				highlightMouseDown: true,
				varyBarColor: true
			},
			pointLabels: { show: true }
		},
		grid: {
			backgroundColor: '#000',
			gridLineColor: '#666'
		}
	});

	$(chartId).bind('jqplotMouseMove', function (_event, xy, _axesData, _neighbor, plot) {
		const closestData = getDataBasedOnMouseXPositionBar(chart, xy, plot, 0, 16, deathCounts);

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
		$(highlighterId).append('<tr><td>Death</td><td id="h-death"></td></tr>');
		$(highlighterId).append('<tr><td>Amount</td><td id="h-count"></td></tr>');
		$(highlighterId).append('<tr><td>Percentage</td><td id="h-percentage"></td></tr>');
		$(highlighterId).append('</tbody>');
		$(chartId).append('</table>');
	});

	function setHighlighter(data, xy) {
		setHighlighterPosition(chart, highlighterId, data, xy, 0, 100000, true);

		const index = data[0] - 1;
		$('#h-death').html(deathNames[index]);
		$('#h-count').html(data[1]);
		$('#h-percentage').html((data[1] / sum * 100).toFixed(2) + '%');

		$(highlighterId).show();
	}
});