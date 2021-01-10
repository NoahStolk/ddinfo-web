function getDataBasedOnMouseXPosition(chart, xy, plot, minXValue, maxXValue) {
	const data = plot.series[0].data;

	let iData;
	for (i = 0; i < data.length - 1; i++) {
		iData = data[i];
		const iDataNext = data[i + 1];

		let xPosStart = (iData[0] - minXValue) / (maxXValue - minXValue) * chart.grid._width;
		let xPosEnd = (iDataNext[0] - minXValue) / (maxXValue - minXValue) * chart.grid._width;

		if (xy.x > xPosStart && xy.x < xPosEnd) {
			return iData;
		}
	}

	return data[data.length - 1];
}

function setHighlighterPosition(chart, highlighterId, data, xy, minYValue, maxYValue, useMousePosition) {
	const yAxisWidth = chart.grid._width - chart._width;
	const yPerc = (data[1] - minYValue) / (maxYValue - minYValue);

	const width = $(highlighterId).css('width').replace('px', '');
	$(highlighterId).css({
		position: "absolute",
		left: clamp(xy.x - yAxisWidth - ($(highlighterId).width() / 2 + 6), 0, chart._width - width) + "px",
		bottom: (useMousePosition ? (chart.grid._height - xy.y + 64) : (yPerc * chart.grid._height + (yPerc < 0.5 ? 112 : -256))) + "px",
	});
}

function clamp(val, min, max) {
	return val < min ? min : val > max ? max : val;
}