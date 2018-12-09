var deathTypes;
$.getJSON("/API/GetDeaths", function (data) {
	deathTypes = data;
});

$.getJSON("/API/GetWorldRecords", function (data) {
	var wrs = [];
	$.each(data, function (key, val) {
		var accuracy = val.ShotsHit / val.ShotsFired * 100;
		if (isNaN(accuracy))
			accuracy = 0;
		var death = deathTypes[val.DeathType + 1];

		wrs.push([new Date(key), val.Time / 10000, val.Username, val.Gems === 0 ? "?" : val.Gems.toFixed(0), val.Kills === 0 ? "?" : val.Kills.toFixed(0), accuracy === 0 ? "?" : accuracy.toFixed(2) + "%", death.ColorCode, death.Name]);
	});

	$.jqplot("world-record-progression-chart", [wrs], {
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
});