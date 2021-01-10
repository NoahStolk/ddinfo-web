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