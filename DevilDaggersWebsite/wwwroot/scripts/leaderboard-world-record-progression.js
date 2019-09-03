var deathTypes = [];
$.getJSON("/API/GetDeaths?gameVersion=V1", function (data) {
	deathTypes[0] = data;
});
$.getJSON("/API/GetDeaths?gameVersion=V2", function (data) {
	deathTypes[1] = data;
});
$.getJSON("/API/GetDeaths?gameVersion=V3", function (data) {
	deathTypes[2] = data;
});

var gameVersions = [];
$.getJSON("/API/GetGameVersions", function (data) {
	gameVersions = data;
});

var monthShortNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

var minDate = new Date("2016-02-18T00:00:00Z");
var maxDate = Date.now();
var minTime = 450;
var maxTime = 1150;

$.getJSON("/API/GetWorldRecords", function (data) {
	var wrs = [];
	var wrIndex = 0;
	$.each(data, function (key, worldRecord) {
		var date = new Date(worldRecord.DateTime);

		var version = 0;
		for (var i = Object.keys(gameVersions).length; i > 0; i--) {
			if (date >= new Date(gameVersions["V" + i].ReleaseDate)) {
				version = i - 1;
				break;
			}
		}
		var death;
		for (var j = 0; j < deathTypes[version].length; j++) {
			if (deathTypes[version][j].DeathType === worldRecord.Entry.DeathType) {
				death = deathTypes[version][j];
				break;
			}
		}

		var accuracy = worldRecord.Entry.ShotsHit / worldRecord.Entry.ShotsFired * 100;
		if (isNaN(accuracy))
			accuracy = 0;

		wrs.push([date, worldRecord.Entry.Time / 10000, worldRecord.Entry.Username, worldRecord.Entry.Gems === 0 ? "?" : worldRecord.Entry.Gems.toFixed(0), worldRecord.Entry.Kills === 0 ? "?" : worldRecord.Entry.Kills.toFixed(0), accuracy === 0 ? "?" : accuracy.toFixed(2) + "%", death.ColorCode, death.Name]);

		if (++wrIndex === data.length) {
			wrs.push([maxDate + 10000000000 /*Ugly way to make sure no dot is visible*/, worldRecord.Entry.Time / 10000, worldRecord.Entry.Username, worldRecord.Entry.Gems === 0 ? "?" : worldRecord.Entry.Gems.toFixed(0), worldRecord.Entry.Kills === 0 ? "?" : worldRecord.Entry.Kills.toFixed(0), accuracy === 0 ? "?" : accuracy.toFixed(2) + "%", death.ColorCode, death.Name]);
		}
	});

	var chart = $.jqplot("world-record-progression-chart", [wrs], {
		axes: {
			xaxis: {
				renderer: $.jqplot.DateAxisRenderer,
				min: minDate,
				max: maxDate,
				tickOptions: {
					formatString: "%b %#d '%y"
				}
			},
			yaxis: {
				mark: 'outside',
				min: minTime,
				max: maxTime,
				numberTicks: (maxTime - minTime) / 50 + 1
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
			color: '#FF0000'
		},
		grid: {
			backgroundColor: '#000000'
		},
		canvasOverlay: {
			show: true,
			objects: [
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

	$('#world-record-progression-chart').bind('jqplotMouseMove', function (event, xy, axesData, neighbor, plot) {
		var xPerc = xy.x / chart.grid._width;
		var yPerc = 1 - xy.y / chart.grid._height;

		var minDistance = 1000;
		var closest = -1;

		// Check which point is closest to the mouse.
		for (i = 0; i < plot.series[0].data.length; i++) {
			var iData = plot.series[0].data[i];

			var xPos = (iData[0] - minDate) / (maxDate - minDate) * chart.grid._width;
			var yPos = (1 - (iData[1] - minTime) / (maxTime - minTime)) * chart.grid._height;

			var distance = Math.sqrt(Math.pow(xy.x - xPos, 2) + Math.pow(xy.y - yPos, 2));
			if (distance < 32 && distance < minDistance) {
				minDistance = distance;
				closest = i;
			}
		}

		// If any point is closer than 32 pixels from the mouse, highlight it.
		if (closest !== -1) {
			setHighlighter(plot.series[0].data[closest], xy);
		}
		// If not, check if mouse is hovering over a line.
		// Mouse needs to be within a range of 5% of the total grid height.
		else {
			var show = false;

			for (var i = 0; i < plot.series[0].data.length; i++) {
				iData = plot.series[0].data[i];
				yPos = (iData[1] - minTime) / (maxTime - minTime);

				if (i === plot.series[0].data.length || xPerc < (plot.series[0].data[i + 1][0] - minDate) / (maxDate - minDate)) {
					if (yPerc < yPos - 0.05 || yPerc > yPos + 0.05)
						break;

					setHighlighter(iData, xy);
					show = true;
					break;
				}
			}

			if (!show)
				$("#highlighter").hide();
		}
	});
	$('#world-record-progression-chart').bind('jqplotMouseLeave', function () {
		$("#highlighter").hide();
	});

	$(window).resize(function () {
		chart.replot();

		$('#world-record-progression-chart').append('<table id="highlighter">');
		$('#highlighter').append('<tr><td>Date</td><td id="h-date"></td></tr>');
		$('#highlighter').append('<tr><td>Time</td><td id="h-time"></td></tr>');
		$('#highlighter').append('<tr><td>Username</td><td id="h-username"></td></tr>');
		$('#highlighter').append('<tr><td>Gems</td><td id="h-gems"></td></tr>');
		$('#highlighter').append('<tr><td>Kills</td><td id="h-kills"></td></tr>');
		$('#highlighter').append('<tr><td>Accuracy</td><td id="h-accuracy"></td></tr>');
		$('#highlighter').append('<tr><td>Death type</td><td id="h-death-type"></td></tr>');
		$('#world-record-progression-chart').append('</table>');
	});

	function setHighlighter(data, xy) {
		var yAxisWidth = chart.grid._width - chart._width;
		var timePerc = (data[1] - minTime) / (maxTime - minTime);
		var date = new Date(data[0]);
		var dateString = monthShortNames[date.getMonth()] + " " + ("0" + date.getDate()).slice(-2) + " '" + date.getFullYear().toString().substring(2, 4);

		// Position
		$('#highlighter').css({
			position: "absolute",
			left: xy.x - yAxisWidth - ($('#highlighter').width() / 2 + 6) + "px",
			bottom: timePerc * chart.grid._height + (timePerc < 0.5 ? 112 : -256) + "px"
		});

		// Values
		$('#h-date').html(dateString);
		$('#h-time').html(data[1].toFixed(4));
		$('#h-username').html(data[2]);
		$('#h-gems').html(data[3]);
		$('#h-kills').html(data[4]);
		$('#h-accuracy').html(data[5]);
		$('#h-death-type').html(data[7]);

		// Styles
		if (data[1] < 500) {
			$('#h-time').addClass('golden');
			$('#h-time').removeClass('devil');
		}
		else {
			$('#h-time').removeClass('golden');
			$('#h-time').addClass('devil');
		}
		$('#h-death-type').css({ color: "#" + data[6] });

		// Show
		$("#highlighter").show();
	}
});