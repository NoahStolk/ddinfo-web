function onArenaResize() {
	const bounds = window.arena.canvasContainer.getBoundingClientRect();
	const arenaSize = Math.min(window.innerWidth, bounds.width);

	window.arena.canvas.width = arenaSize;
	window.arena.canvas.height = arenaSize;
	window.arena.canvas.style.width = arenaSize;
	window.arena.canvas.style.height = arenaSize;
	window.arena.wrapperComponent.invokeMethodAsync('OnResize', arenaSize);
}

window.windowResize = () => {
	onArenaResize();
};

window.initArena = () => {
	window.addEventListener("resize", onArenaResize);
	getBoundingClientRect = (element, _) => { return element.getBoundingClientRect(); };
}

window.registerArena = (wrapperComponent) => {
	const canvasContainer = document.getElementById('arena');
	if (!canvasContainer) {
		console.log("Cannot find element with ID 'arena'.");
		return;
	}

	const arena = {
		wrapperComponent: wrapperComponent,
		canvasContainer: canvasContainer,
		canvas: document.getElementById('arena-canvas'),
	};

	arena.canvas.onmousemove = (e) => {
		arena.wrapperComponent.invokeMethodAsync('OnMouseMove', e.clientX, e.clientY);
	};

	window.arena = arena;
};
