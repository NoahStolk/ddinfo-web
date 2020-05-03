let monthShortNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

let gameVersions = [];
$.ajax({
	url: "/Api/GetGameVersions",
	async: false,
	dataType: 'json',
	success: function (json) {
		gameVersions = json;
	}
});

let deathTypes = [];
$.ajax({
	url: "/Api/GetDeaths?gameVersion=V1",
	async: false,
	dataType: 'json',
	success: function (json) {
		deathTypes[0] = json;
	}
});
$.ajax({
	url: "/Api/GetDeaths?gameVersion=V2",
	async: false,
	dataType: 'json',
	success: function (json) {
		deathTypes[1] = json;
	}
});
$.ajax({
	url: "/Api/GetDeaths?gameVersion=V3",
	async: false,
	dataType: 'json',
	success: function (json) {
		deathTypes[2] = json;
	}
});

function getGameVersion(date) {
	for (let i = Object.keys(gameVersions).length; i > 0; i--)
		if (date >= new Date(gameVersions["V" + i].ReleaseDate))
			return i - 1;
	return 0;
}

function getDeathType(date, entry) {
	const gameVersion = getGameVersion(date);
	for (let i = 0; i < deathTypes[gameVersion].length; i++)
		if (deathTypes[gameVersion][i].DeathType === entry.DeathType)
			return deathTypes[gameVersion][i];
}

function createChart(chartName, data, minDate, maxDate, minTime, maxTime, yNumberTicks) {
	return $.jqplot(chartName, [data], {
		axes: {
			xaxis: {
				renderer: $.jqplot.DateAxisRenderer,
				min: minDate,
				max: maxDate,
				tickOptions: {
					formatString: "%b %#d '%y"
				},
				numberTicks: 20
			},
			yaxis: {
				mark: 'outside',
				min: minTime,
				max: maxTime,
				numberTicks: yNumberTicks
			}
		},
		seriesDefaults: {
			step: true,
			rendererOptions: {
				smooth: true,
				animation: {
					show: true
				}
			},
			markerOptions: {
				show: true,
				size: 6.5
			},
			color: '#f00'
		},
		grid: {
			backgroundColor: '#000',
			gridLineColor: '#666'
		},
		canvasOverlay: {
			show: true,
			objects: [
				{
					rectangle: {
						xmin: new Date("2016-01-01"),
						xmax: new Date("2016-02-18"),
						xminOffset: "0px",
						xmaxOffset: "0px",
						yminOffset: "0px",
						ymaxOffset: "0px",
						color: "rgba(200, 0, 0, 0.1)",
						showTooltip: true,
						tooltipFormatString: "Pre-release",
						tooltipLocation: 'se'
					}
				},
				{
					rectangle: {
						xmin: new Date("2016-02-18"),
						xmax: new Date("2016-07-05"),
						xminOffset: "0px",
						xmaxOffset: "0px",
						yminOffset: "0px",
						ymaxOffset: "0px",
						color: "rgba(0, 200, 200, 0.1)",
						showTooltip: true,
						tooltipFormatString: "V1",
						tooltipLocation: 'se'
					}
				},
				{
					rectangle: {
						xmin: new Date("2016-07-05"),
						xmax: new Date("2016-09-19"),
						xminOffset: "0px",
						xmaxOffset: "0px",
						yminOffset: "0px",
						ymaxOffset: "0px",
						color: "rgba(200, 200, 0, 0.1)",
						showTooltip: true,
						tooltipFormatString: "V2",
						tooltipLocation: 'se'
					}
				},
				{
					rectangle: {
						xmin: new Date("2016-09-19"),
						xmax: maxDate,
						xminOffset: "0px",
						xmaxOffset: "0px",
						yminOffset: "0px",
						ymaxOffset: "0px",
						color: "rgba(200, 0, 200, 0.1)",
						showTooltip: true,
						tooltipFormatString: "V3",
						tooltipLocation: 'se'
					}
				}
			]
		}
	});
}

function getClosestDataToMouse(chart, xy, plot, minDate, maxDate, minTime, maxTime) {
	let xPerc = xy.x / chart.grid._width;
	let yPerc = 1 - xy.y / chart.grid._height;

	let minDistance = 1000;
	let closest = -1;

	// Check which point is closest to the mouse.
	let iData;
	for (i = 0; i < plot.series[0].data.length; i++) {
		iData = plot.series[0].data[i];

		let xPos = (iData[0] - minDate) / (maxDate - minDate) * chart.grid._width;
		let yPos = (1 - (iData[1] - minTime) / (maxTime - minTime)) * chart.grid._height;

		let distance = Math.sqrt(Math.pow(xy.x - xPos, 2) + Math.pow(xy.y - yPos, 2));
		if (distance < 32 && distance < minDistance) {
			minDistance = distance;
			closest = i;
		}
	}

	// If any point is closer than 32 pixels from the mouse, highlight it.
	if (closest !== -1)
		return plot.series[0].data[closest];

	// If not, check if mouse is hovering over a line.
	// Mouse needs to be within a range of 5% of the total grid height.
	for (let i = 0; i < plot.series[0].data.length; i++) {
		iData = plot.series[0].data[i];
		yPos = (iData[1] - minTime) / (maxTime - minTime);

		if (i === plot.series[0].data.length || xPerc < (plot.series[0].data[i + 1][0] - minDate) / (maxDate - minDate)) {
			if (yPerc < yPos - 0.05 || yPerc > yPos + 0.05)
				break;

			return iData;
		}
	}

	return null;
}

function setHighlighterPosition(chart, data, xy, minTime, maxTime) {
	const yAxisWidth = chart.grid._width - chart._width;
	const timePerc = (data[1] - minTime) / (maxTime - minTime);
	$('#highlighter').css({
		position: "absolute",
		left: xy.x - yAxisWidth - ($('#highlighter').width() / 2 + 6) + "px",
		bottom: timePerc * chart.grid._height + (timePerc < 0.5 ? 112 : -256) + "px"
	});
}

function setHighlighterStyle(time, colorCode) {
	// Styles
	if (time < 500) {
		$('#h-time').addClass('golden');
		$('#h-time').removeClass('devil');
	}
	else {
		$('#h-time').removeClass('golden');
		$('#h-time').addClass('devil');
	}
	$('#h-death-type').css({ color: "#" + colorCode });

	// Show
	$("#highlighter").show();
}