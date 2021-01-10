$.getJSON("/api/leaderboard-statistics/times", function (data) {
	const scoreCounts = [];
	$.each(data, function (_, scoreCount) {
		scoreCounts.push(scoreCount);
	});

	let sum = scoreCounts.reduce(function (a, b) {
		return a + b;
	});

	const chartName = "scores-chart";
	const chartId = "#" + chartName;
	const highlighterName = "scores-highlighter";
	const highlighterId = "#" + highlighterName;

	const chart = $.jqplot(chartName, [scoreCounts], {
		animate: !$.jqplot.use_excanvas,
		axes: {
			xaxis: {
				renderer: $.jqplot.CategoryAxisRenderer,
				ticks: range(0, 1200, 10)
			},
			yaxis: {
				mark: 'outside',
				min: 0,
				max: 50000,
				numberTicks: 11
			}
		},
		seriesDefaults: {
			renderer: $.jqplot.BarRenderer,
			rendererOptions: {
				highlightMouseDown: true
			},
			pointLabels: { show: true }
		},
		grid: {
			backgroundColor: '#000',
			gridLineColor: '#222'
		}
	});

	$(chartId).bind('jqplotMouseMove', function (_event, xy, _axesData, _neighbor, plot) {
		const closestData = getDataBasedOnMouseXPositionBar(chart, xy, plot, 0, 120);

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
		$(highlighterId).append('<tr><td>Score</td><td id="h-score"></td></tr>');
		$(highlighterId).append('<tr><td>Amount</td><td id="h-score-count"></td></tr>');
		$(highlighterId).append('<tr><td>Percentage</td><td id="h-score-percentage"></td></tr>');
		$(highlighterId).append('</tbody>');
		$(chartId).append('</table>');
	});

	function setHighlighter(data, xy) {
		setHighlighterPosition(chart, highlighterId, data, xy, 0, 100000, true);

		const index = data[0] - 1;
		$('#h-score').html(index * 10);
		$('#h-score-count').html(data[1]);
		$('#h-score-percentage').html((data[1] / sum * 100).toFixed(2) + '%');

		$(highlighterId).show();
	}
});

function range(start, stop, step = 1) {
	return Array(Math.ceil((stop - start) / step)).fill(start).map((x, y) => x + y * step);
}