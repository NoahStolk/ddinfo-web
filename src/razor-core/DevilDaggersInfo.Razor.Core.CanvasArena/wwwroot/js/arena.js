function onArenaResize() {
	const bounds = window.arena.canvasContainer.getBoundingClientRect();
	const arenaSize = Math.min(window.innerWidth, bounds.width);

	window.arena.canvas.width = arenaSize;
	window.arena.canvas.height = arenaSize;
	window.arena.canvas.style.width = arenaSize;
	window.arena.canvas.style.height = arenaSize;
	window.arena.wrapperComponent.invokeMethodAsync('OnResize', arenaSize);
}

window.arenaInitialResize = () => {
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

// TODO: Move to webAssemblyArena.js file.
window.drawTile = (d) => {
	const dc = c2d.getContext(d);
	const x = jsi.readInt32(d, 4);
	const y = jsi.readInt32(d, 8);
	const r = jsi.readInt32(d, 12);
	const g = jsi.readInt32(d, 16);
	const b = jsi.readInt32(d, 20);
	const tileSize = jsi.readFloat(d, 24);

	const border = 1;

	dc.strokeStyle = "rgba(" + r + ", " + g + ", " + b + ", 0.75)";
	dc.fillStyle = "rgb(" + r + ", " + g + ", " + b + ")";

	dc.strokeRect(x * tileSize + 0.5, y * tileSize + 0.5, tileSize - 1, tileSize - 1);
	dc.fillRect(x * tileSize + border, y * tileSize + border, tileSize - border * 2, tileSize - border * 2);
};
