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
				min: new Date("2016-01-01T00:00:00Z"),
				max: Date.now() + 604800000, // Amount of milliseconds in a week
				tickOptions: {
					formatString: "%b %#d '%y"
				}
			},
			yaxis: {
				min: 400,
				max: 1150,
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
			markerOptions: { show: false }
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
		cursor: {
			show: false
		},
		grid: {
			backgroundColor: '#000000'
		}
	});
});