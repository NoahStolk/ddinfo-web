const deathNames = ["FALLEN", "SWARMED", "IMPALED", "GORED", "INFESTED", "OPENED", "PURGED", "DESECRATED", "SACRIFICED", "EVISCERATED", "ANNIHILATED", "INTOXICATED", "ENVENOMATED", "INCARNATED", "DISCARNATED", "ENTANGLED", "HAUNTED"];
const deathColors = ["#DDDDDD", "#352710", "#433114", "#6E5021", "#DCCB00", "#976E2E", "#4E3000", "#804E00", "#AF6B00", "#837E75", "#478B41", "#99A100", "#657A00", "#FF0000", "#FF3131", "#771D00", "#C8A2C8"];
const daggerNames = ["Default", "Bronze", "Silver", "Golden", "Devil", "Leviathan"];
const daggerColors = ["#444444", "#CD7F32", "#DDDDDD", "#FFDF00", "#FF0000", "#A00000"];

function setHighlighterPosition(chart, highlighterId, data, xy, minYValue, maxYValue, useMousePosition) {
	const yAxisWidth = chart.grid._width - chart._width;
	const yPerc = (data[1] - minYValue) / (maxYValue - minYValue);

	const width = $(highlighterId).css('width').replace('px', '');
	$(highlighterId).css({
		position: "absolute",
		left: clamp(xy.x - yAxisWidth - ($(highlighterId).width() / 2 + 6), 0, chart._width - width) + "px",
		bottom: (useMousePosition ? (chart.grid._height - xy.y + 128) : (yPerc * chart.grid._height + (yPerc < 0.5 ? 112 : -256))) + "px",
	});
}

function clamp(val, min, max) {
	return val < min ? min : val > max ? max : val;
}
