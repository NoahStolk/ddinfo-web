$.getJSON("/api/leaderboard-statistics/daggers", function (data) {
	const daggerCounts = [];
	$.each(data, function (_, daggerCount) {
		daggerCounts.push(daggerCount);
	});

	let sum = daggerCounts.reduce(function (a, b) {
		return a + b;
	});

	const chartName = "daggers-chart";
	const chartId = "#" + chartName;
	const highlighterName = "daggers-highlighter";
	const highlighterId = "#" + highlighterName;

	const chart = $.jqplot(chartName, [daggerCounts], {
		animate: !$.jqplot.use_excanvas,
		seriesColors: daggerColors,
		axes: {
			xaxis: {
				renderer: $.jqplot.CategoryAxisRenderer,
				ticks: daggerNames,
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
					formatString: "%#6d  " // Spaces are a hack to add more margin.
				}
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
		const closestData = getDataBasedOnMouseXPositionBar(chart, xy, plot, 0, daggerCounts.length, daggerCounts);

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
		$(highlighterId).append('<tr><td>Dagger</td><td id="h-dagger"></td></tr>');
		$(highlighterId).append('<tr><td>Amount</td><td id="h-dagger-count"></td></tr>');
		$(highlighterId).append('<tr><td>Percentage</td><td id="h-dagger-percentage"></td></tr>');
		$(highlighterId).append('</tbody>');
		$(chartId).append('</table>');
	});

	function setHighlighter(data, xy) {
		setHighlighterPosition(chart, highlighterId, data, xy, 0, 100000, true);

		const index = data[0] - 1;
		$('#h-dagger').html(daggerNames[index]);
		$('#h-dagger-count').html(data[1]);
		$('#h-dagger-percentage').html((data[1] / sum * 100).toFixed(2) + '%');

		$(highlighterId).show();
	}
});
