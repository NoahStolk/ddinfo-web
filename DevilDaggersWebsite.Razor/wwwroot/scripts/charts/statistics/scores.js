$.getJSON("/api/leaderboard-statistics/times", function (data) {
	const pre500ScoreCounts = [];
	const post500ScoreCounts = [];
	$.each(data, function (key, scoreCount) {
		if (key < 500)
			pre500ScoreCounts.push(scoreCount);
		else
			post500ScoreCounts.push(scoreCount);
	});

	let pre500TotalScores = pre500ScoreCounts.reduce(function (a, b) {
		return a + b;
	});
	let post500TotalScores = post500ScoreCounts.reduce(function (a, b) {
		return a + b;
	});

	let chartName = "scores-pre500-chart";
	let chartId = "#" + chartName;
	let highlighterName = "scores-pre500-highlighter";
	let highlighterId = "#" + highlighterName;
	setChart(chartName, chartId, highlighterName, highlighterId, 'h-pre500', pre500ScoreCounts, pre500TotalScores, 0, 500, 10, 10000);

	chartName = "scores-post500-chart";
	chartId = "#" + chartName;
	highlighterName = "scores-post500-highlighter";
	highlighterId = "#" + highlighterName;
	setChart(chartName, chartId, highlighterName, highlighterId, 'h-post500', post500ScoreCounts, post500TotalScores, 500, 1200, 10, 50);
});

function setChart(chartName, chartId, highlighterName, highlighterId, tablePrefix, scoreCounts, totalScores, minScore, maxScore, stepScore, maxCountRounder) {
	const barCount = (maxScore - minScore) / stepScore;
	let maxCount = 0;
	for (let i = 0; i < scoreCounts.length; i++) {
		const scores = scoreCounts[i];
		if (maxCount < scores)
			maxCount = scores;
	}
	maxCount = Math.ceil(maxCount / maxCountRounder) * maxCountRounder;

	const chart = $.jqplot(chartName, [scoreCounts], {
		animate: !$.jqplot.use_excanvas,
		axes: {
			xaxis: {
				renderer: $.jqplot.CategoryAxisRenderer,
				ticks: range(minScore, maxScore, stepScore).map(i => i.toString()), // Must be strings or the plugin turns 0 into 1.
				labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
				tickRenderer: $.jqplot.CanvasAxisTickRenderer,
				tickOptions: {
					labelPosition: 'left',
					angle: -45
				}
			},
			yaxis: {
				mark: 'outside',
				min: 0,
				max: maxCount,
				numberTicks: 11,
				labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
				tickRenderer: $.jqplot.CanvasAxisTickRenderer,
				tickOptions: {
					formatString: "%#5d"
				}
			}
		},
		seriesDefaults: {
			renderer: $.jqplot.BarRenderer,
			rendererOptions: {
				highlightMouseDown: true,
				barMargin: 6
			},
			pointLabels: { show: true }
		},
		grid: {
			backgroundColor: '#000',
			gridLineColor: '#222'
		}
	});

	$(chartId).bind('jqplotMouseMove', function (_event, xy, _axesData, _neighbor, plot) {
		const closestData = getDataBasedOnMouseXPositionBar(chart, xy, plot, 0, barCount);

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
		$(highlighterId).append('<tr><td>Score</td><td id="' + tablePrefix + '-score"></td></tr>');
		$(highlighterId).append('<tr><td>Amount</td><td id="' + tablePrefix + '-score-count"></td></tr>');
		$(highlighterId).append('<tr><td>Percentage</td><td id="' + tablePrefix + '-score-percentage"></td></tr>');
		$(highlighterId).append('</tbody>');
		$(chartId).append('</table>');
	});

	function setHighlighter(data, xy) {
		setHighlighterPosition(chart, highlighterId, data, xy, 0, 100000, true);

		const index = data[0] - 1;
		$('#' + tablePrefix + '-score').html(minScore + index * 10);
		$('#' + tablePrefix + '-score-count').html(data[1]);
		$('#' + tablePrefix + '-score-percentage').html((data[1] / totalScores * 100).toFixed(2) + '%');

		$(highlighterId).show();
	}
}

function range(start, stop, step = 1) {
	return Array(Math.ceil((stop - start) / step)).fill(start).map((x, y) => x + y * step);
}
