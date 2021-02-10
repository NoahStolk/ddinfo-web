function getDataBasedOnMouseXPositionBar(chart, xy, plot, minXValue, maxXValue) {
	const data = plot.series[0].data;

	for (i = 1; i < data.length; i++) {
		const iDataPrevious = data[i - 1];
		const iData = data[i];

		let xPosStart = (iDataPrevious[0] - minXValue) / (maxXValue - minXValue) * chart.grid._width;
		let xPosEnd = (iData[0] - minXValue) / (maxXValue - minXValue) * chart.grid._width;

		if (xy.x > xPosStart && xy.x < xPosEnd) {
			return iData;
		}
	}

	return data[0];
}