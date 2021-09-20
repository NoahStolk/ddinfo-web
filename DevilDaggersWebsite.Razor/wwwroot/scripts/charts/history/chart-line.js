const preRelease = new Date(2016, 0, 1); // January 1st
const v1Release = new Date(2016, 1, 18); // February 18th
const v2Release = new Date(2016, 6, 5); // July 5th
const v3Release = new Date(2016, 8, 19); // September 19th

let monthShortNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

let getUrlParameter = function getUrlParameter(sParam) {
	let sPageURL = window.location.search.substring(1),
		sURLVariables = sPageURL.split('&'),
		sParameterName,
		i;

	for (i = 0; i < sURLVariables.length; i++) {
		sParameterName = sURLVariables[i].split('=');

		if (sParameterName[0] === sParam)
			return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
	}
};

let deathTypes = [];
$.ajax({
	url: "/api/deaths?gameVersion=V1",
	async: false,
	dataType: 'json',
	success: function (json) {
		deathTypes[0] = json;
	}
});
$.ajax({
	url: "/api/deaths?gameVersion=V2",
	async: false,
	dataType: 'json',
	success: function (json) {
		deathTypes[1] = json;
	}
});
$.ajax({
	url: "/api/deaths?gameVersion=V3",
	async: false,
	dataType: 'json',
	success: function (json) {
		deathTypes[2] = json;
	}
});

function getDeathType(date, entry) {
	let gameVersionIndex = 0;
	if (date > v2Release && date < v3Release)
		gameVersionIndex = 1;
	else if (date >= v3Release)
		gameVersionIndex = 2;

	for (let i = 0; i < deathTypes[gameVersionIndex].length; i++)
		if (deathTypes[gameVersionIndex][i].deathType === entry.deathType)
			return deathTypes[gameVersionIndex][i];

	return {
		colorCode: '444',
		name: 'Unknown'
	};
}

function createChart(chartName, data, minDate, maxDate, minTime, maxTime, yNumberTicks, showMarker) {
	return $.jqplot(chartName, [data], {
		axes: {
			xaxis: {
				renderer: $.jqplot.DateAxisRenderer,
				min: minDate,
				max: maxDate,
				tickOptions: {
					formatString: "%b %#d '%y",
					labelPosition: 'left',
					angle: -45
				},
				numberTicks: 15,
				labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
				tickRenderer: $.jqplot.CanvasAxisTickRenderer
			},
			yaxis: {
				min: minTime,
				max: maxTime,
				numberTicks: yNumberTicks,
				labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
				tickRenderer: $.jqplot.CanvasAxisTickRenderer,
				tickOptions: {
					formatString: "%#4d"
				}
			}
		},
		seriesDefaults: {
			step: true,
			rendererOptions: {
				smooth: true,
				animation: {
					show: true,
					speed: 1250
				}
			},
			markerOptions: {
				show: showMarker,
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
						xmin: preRelease,
						xmax: v1Release,
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
						xmin: v1Release,
						xmax: v2Release,
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
						xmin: v2Release,
						xmax: v3Release,
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
						xmin: v3Release,
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

function setHighlighterStyle(time, colorCode) {
	if (time < 500) {
		$('#h-time').addClass('golden');
		$('#h-time').removeClass('devil');
	}
	else {
		$('#h-time').removeClass('golden');
		$('#h-time').addClass('devil');
	}
	$('#h-death-type').css({ color: "#" + colorCode });
}

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
