function onResize() {
	for (let i = 0; i < window.charts.length; i++) {
		const chart = window.charts[i];
		if (!chart || !chart.canvas) {
			console.log("Chart at index " + i + " is null or doesn't have a canvas");
			return;
		}

		const bounds = chart.canvasContainer.getBoundingClientRect();
		const canvasWidth = bounds.width;
		const canvasHeight = Math.min(window.innerHeight * 0.8, canvasWidth * (9 / 16));

		chart.canvas.width = canvasWidth;
		chart.canvas.height = canvasHeight;
		chart.canvas.style.width = canvasWidth;
		chart.canvas.style.height = canvasHeight;
		chart.chartWrapperComponent.invokeMethodAsync('OnResize', canvasWidth, canvasHeight);
	}
}

window.windowResize = (_) => {
	onResize();
};

window.init = () => {
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
