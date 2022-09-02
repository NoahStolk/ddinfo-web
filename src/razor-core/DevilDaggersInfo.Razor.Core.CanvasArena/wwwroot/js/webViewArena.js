function drawTile(dc, r, g, b, x, y, tileSize) {
	dc.strokeStyle = "rgba(" + r + ", " + g + ", " + b + ", 0.75)";
	dc.fillStyle = "rgb(" + r + ", " + g + ", " + b + ")";

	const border = 1;
	dc.strokeRect(x * tileSize + 0.5, y * tileSize + 0.5, tileSize - 1, tileSize - 1);
	dc.fillRect(x * tileSize + border, y * tileSize + border, tileSize - border * 2, tileSize - border * 2);
}

window.drawTiles = (id, colors, tileSize) => {
	const dc = c2d.getContext(id);

	const arenaDimension = 51;
	for (let i = 0; i < arenaDimension; i++) {
		for (let j = 0; j < arenaDimension; j++) {
			const color = colors[i * arenaDimension + j];
			const r = color >> 24 & 0xFF;
			const g = color >> 16 & 0xFF;
			const b = color >> 8 & 0xFF;
			if (!(r === 0 && g === 0 && b === 0))
				drawTile(dc, r, g, b, i, j, tileSize);
		}
	}
};
