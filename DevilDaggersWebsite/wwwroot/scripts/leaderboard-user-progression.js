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

var getUrlParameter = function getUrlParameter(sParam) {
	var sPageURL = window.location.search.substring(1),
		sURLVariables = sPageURL.split('&'),
		sParameterName,
		i;

	for (i = 0; i < sURLVariables.length; i++) {
		sParameterName = sURLVariables[i].split('=');

		if (sParameterName[0] === sParam) {
			return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
		}
	}
};

$.getJSON("/API/GetUserProgression?userID=" + getUrlParameter("userID"), function (data) {
	var pbs = [];
	$.each(data, function (key, val) {
		var date = new Date(key);

		var version = 0;
		for (var i = Object.keys(gameVersions).length; i > 0; i--) {
			if (date >= new Date(gameVersions["V" + i].ReleaseDate)) {
				version = i - 1;
				break;
			}
		}
		var death;
		for (var j = 0; j < deathTypes[version].length; j++) {
			if (deathTypes[version][j].DeathType === val.DeathType) {
				death = deathTypes[version][j];
				break;
			}
		}

		var accuracy = val.ShotsHit / val.ShotsFired * 100;
		if (isNaN(accuracy))
			accuracy = 0;

		pbs.push([date, val.Time / 10000, val.Rank.toFixed(0), val.Username, val.Gems === 0 ? "?" : val.Gems.toFixed(0), val.Kills === 0 ? "?" : val.Kills.toFixed(0), accuracy === 0 ? "?" : accuracy.toFixed(2) + "%", death.ColorCode, death.Name]);
	});

	// TODO: This is just to prevent the graph from crashing. Something else should be displayed instead.
	if (pbs.length === 0)
		pbs.push([Date.now(), 0, 0, "", 0, 0, "0%", "FFFFFF", "FALLEN"]);

	var lowestTime = 10000;
	var highestTime = 0;
	for (var i = 0; i < pbs.length; i++) {
		if (pbs[i][1] > highestTime)
			highestTime = pbs[i][1];
		if (pbs[i][1] < lowestTime)
			lowestTime = pbs[i][1];
	}

	lowestTime = Math.floor(lowestTime / 50) * 50;
	highestTime = Math.ceil(highestTime / 50) * 50 + 50;

	var steps = (highestTime - lowestTime) / 50 + 1;

	$.jqplot("user-progression-chart", [pbs], {
		axes: {
			xaxis: {
				renderer: $.jqplot.DateAxisRenderer,
				min: pbs[0][0] - 86400000 * 7, // Amount of milliseconds in a day * amount of days
				max: Date.now() + 86400000 * 7,
				tickOptions: {
					formatString: "%b %#d '%y"
				}
			},
			yaxis: {
				min: lowestTime,
				max: highestTime,
				numberTicks: steps,
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
			yvalues: 8,
			formatString: '<table class="jqplot-highlighter"> \
			<tr><td>Date:</td><td>%s</td></tr> \
			<tr><td>Time:</td><td>%s</td></tr> \
			<tr><td>Rank:</td><td>%s</td></tr> \
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