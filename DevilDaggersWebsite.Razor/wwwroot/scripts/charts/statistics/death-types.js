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
				ticks: deathNames,
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
				max: 100000,
				numberTicks: 11,
				labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
				tickRenderer: $.jqplot.CanvasAxisTickRenderer,
				tickOptions: {
					formatString: "%#6d"
				}
			}
		},
		seriesDefaults: {
			renderer: $.jqplot.BarRenderer,
			rendererOptions: {
				highlightMouseDown: true,
				varyBarColor: true,
				barMargin: 6
			},
			pointLabels: { show: true }
		},
		grid: {
			backgroundColor: '#000',
			gridLineColor: '#666'
		}
	});

	$(chartId).bind('jqplotMouseMove', function (_event, xy, _axesData, _neighbor, plot) {
		const closestData = getDataBasedOnMouseXPositionBar(chart, xy, plot, 0, deathCounts.length, deathCounts);

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
		$(highlighterId).append('<tr><td>Amount</td><td id="h-death-count"></td></tr>');
		$(highlighterId).append('<tr><td>Percentage</td><td id="h-death-percentage"></td></tr>');
		$(highlighterId).append('</tbody>');
		$(chartId).append('</table>');
	});

	function setHighlighter(data, xy) {
		setHighlighterPosition(chart, highlighterId, data, xy, 0, 100000, true);

		const index = data[0] - 1;
		$('#h-death').html(deathNames[index]);
		$('#h-death-count').html(data[1]);
		$('#h-death-percentage').html((data[1] / sum * 100).toFixed(2) + '%');

		$(highlighterId).show();
	}
});
