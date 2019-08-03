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

$.getJSON("/API/GetWorldRecords", function (data) {
	var wrs = [];
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
	});

	var chart = $.jqplot("world-record-progression-chart", [wrs], {
		axes: {
			xaxis: {
				renderer: $.jqplot.DateAxisRenderer,
				min: new Date("2016-02-01T00:00:00Z"),
				max: Date.now() + 86400000 * 18, // Amount of milliseconds in a day * amount of days
				tickOptions: {
					formatString: "%b %#d '%y"
				}
			},
			yaxis: {
				min: 400,
				max: 1150,
				numberTicks: 16,
				tickOptions: {
					formatString: '%.4f'
				}
			}
		},
		series: [
			{
				color: '#FF0000'
			}
		],
		seriesDefaults: {
			step: true,
			markerOptions: {
				show: false
			}
		},
		highlighter: {
			show: true,
			tooltipAxes: 'xy',
			yvalues: 7,
			formatString: '<table class="jqplot-highlighter"> \
			<tr><td>Date:</td><td>%s</td></tr> \
			<tr><td>Time:</td><td>%s</td></tr> \
			<tr><td>Username:</td><td>%s</td></tr> \
			<tr><td>Gems:</td><td>%s</td></tr> \
			<tr><td>Kills:</td><td>%s</td></tr> \
			<tr><td>Accuracy:</td><td>%s</td></tr> \
			<tr><td>Death type:</td><td style="color: #%s">%s</td></tr></table>'
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
						tooltipLocation: 'sw'
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
						tooltipLocation: 'sw'
					}
				},
				{
					rectangle: {
						xmin: new Date("2016-09-19"),
						xmax: Date.now(),
						xminOffset: "0px",
						xmaxOffset: "0px",
						yminOffset: "0px",
						ymaxOffset: "0px",
						color: "rgba(200, 0, 200, 0.1)",
						showTooltip: true,
						tooltipFormatString: "V3",
						tooltipLocation: 'sw'
					}
				}
			]
		}
	});

	$(window).resize(function () {
		chart.replot();
	});
});