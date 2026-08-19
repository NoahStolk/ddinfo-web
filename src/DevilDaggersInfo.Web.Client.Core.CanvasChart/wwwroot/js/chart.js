function onResize() {
	for (let i = 0; i < window.charts.length; i++) {
		const chart = window.charts[i];
		if (!chart || !chart.canvas) {
			console.log("Chart at index " + i + " is null or doesn't have a canvas.");
			continue;
		}

		const bounds = chart.canvasContainer.getBoundingClientRect();
		const canvasWidth = bounds.width;
		const canvasHeight = Math.min(window.innerHeight * 0.8, canvasWidth * (9 / 16));

		chart.canvas.width = canvasWidth;
		chart.canvas.height = canvasHeight;
		chart.canvas.style.width = canvasWidth + "px";
		chart.canvas.style.height = canvasHeight + "px";
		chart.chartWrapperComponent.invokeMethodAsync('OnResize', canvasWidth, canvasHeight);
	}
}

window.chartInitialResize = () => {
	onResize();
};

// Every chart component calls this on its first render, so it must not reset state that other charts on the same page
// have already registered.
window.initChart = () => {
	if (window.charts)
		return;

	window.charts = [];

	window.addEventListener("resize", onResize);
	getBoundingClientRect = (element, _) => { return element.getBoundingClientRect(); };
}

window.registerChart = (chartWrapperComponent, chartName) => {
	const canvasContainer = document.getElementById(chartName);
	if (!canvasContainer) {
		console.log("Cannot find element with ID '" + chartName + "'.");
		return;
	}

	const canvasElements = canvasContainer.getElementsByTagName('canvas');
	if (!canvasElements || canvasElements.length === 0) {
		console.log("Cannot find canvas in element with ID '" + chartName + "'.");
		return;
	}

	if (canvasElements.length > 1) {
		console.log("Multiple canvas elements found in element with ID '" + chartName + "'.");
		return;
	}

	const chart = {
		chartWrapperComponent: chartWrapperComponent,
		chartName: chartName,
		canvasContainer: canvasContainer,
		canvas: canvasElements[0],
	};

	chart.canvas.onmousemove = (e) => {
		chart.chartWrapperComponent.invokeMethodAsync('OnMouseMove', e.clientX, e.clientY);
	};

	if (!window.charts) {
		console.log("Window charts is undefined");
		return;
	}

	window.charts.push(chart);
};

// Charts are not reset between navigations, so a disposed component must remove itself. Otherwise its detached canvas
// stays in the list and reports a zero-sized bounding rect.
window.unregisterChart = (chartName) => {
	if (!window.charts)
		return;

	const index = window.charts.findIndex(c => c && c.chartName === chartName);
	if (index === -1)
		return;

	const chart = window.charts[index];
	if (chart.canvas)
		chart.canvas.onmousemove = null;

	window.charts.splice(index, 1);
};
