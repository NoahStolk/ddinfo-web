window.initArena = () => {
	getBoundingClientRect = (element, _) => { return element.getBoundingClientRect(); };
}

window.registerArena = (wrapperComponent) => {
	const arena = {
		wrapperComponent: wrapperComponent,
		canvas: document.getElementById('arena-canvas'),
	};

	arena.canvas.onmousemove = (e) => {
		arena.wrapperComponent.invokeMethodAsync('OnMouseMove', e.clientX, e.clientY);
	};

	window.arena = arena;
};
