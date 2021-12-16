function onResize() {
	for (let i = 0; i < window.charts.length; i++) {
		const chart = window.charts[i];
		if (!chart || !chart.canvas) {
			console.log("Chart at index " + i + " is null or doesn't have a canvas");
			return;
		}

		chart.canvas.width = window.innerWidth;
		chart.canvas.height = window.innerHeight;
		chart.canvas.style.width = window.innerWidth;
		chart.canvas.style.height = window.innerHeight;

		chart.chartWrapperComponent.invokeMethodAsync('OnResize', chart.canvas.width, chart.canvas.height);
	}
}

window.windowResize = (_) => {
	onResize();
};

window.init = () => {
	console.log("Making new charts array");
	window.charts = [];

	window.addEventListener("resize", onResize);
	getBoundingClientRect = (element, _) => { return element.getBoundingClientRect(); };
}

window.registerChart = (chartWrapperComponent, chartName) => {
	const canvasContainer = document.getElementById(chartName);
	if (!canvasContainer) {
		console.log("Cannot find element with ID " + chartName);
		return;
	}

	const canvasElements = canvasContainer.getElementsByTagName('canvas');
	if (!canvasElements || canvasElements.length === 0) {
		console.log("Cannot find canvas in element with ID " + chartName);
		return;
	}

	if (canvasElements.length > 1) {
		console.log("Multiple canvas elements found in element with ID " + chartName);
		return;
	}

	const chart = {
		chartWrapperComponent: chartWrapperComponent,
		canvas: canvasElements[0],
	};

	chart.canvas.onmousemove = (e) => {
		chart.chartWrapperComponent.invokeMethodAsync('OnMouseMove', e.clientX, e.clientY);
	};

	if (!window.charts) {
		console.log("Window charts is undefined");
		return;
	}

	console.log("Adding " + chartName + " to charts array");
	window.charts.push(chart);
};
