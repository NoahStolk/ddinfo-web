const deathNames = ["FALLEN", "SWARMED", "IMPALED", "GORED", "INFESTED", "OPENED", "PURGED", "DESECRATED", "SACRIFICED", "EVISCERATED", "ANNIHILATED", "INTOXICATED", "ENVENOMATED", "INCARNATED", "DISCARNATED", "BARBED"];
const deathColors = ["#DDDDDD", "#352710", "#433114", "#6E5021", "#DCCB00", "#976E2E", "#4E3000", "#804E00", "#AF6B00", "#837E75", "#478B41", "#99A100", "#657A00", "#FF0000", "#FF3131", "#771D00"];
const daggerNames = ["Devil", "Golden", "Silver", "Bronze", "Default"];
const daggerColors = ["#FF0000", "#FFDF00", "#DDDDDD", "#CD7F32", "#444444"];

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