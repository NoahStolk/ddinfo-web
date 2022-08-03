window.drawTile = (id, x, y, r, g, b, tileSize) => {
	const dc = c2d.getContext(id);
	const border = 1;

	dc.strokeStyle = "rgba(" + r + ", " + g + ", " + b + ", 0.75)";
	dc.fillStyle = "rgb(" + r + ", " + g + ", " + b + ")";

	dc.strokeRect(x * tileSize + 0.5, y * tileSize + 0.5, tileSize - 1, tileSize - 1);
	dc.fillRect(x * tileSize + border, y * tileSize + border, tileSize - border * 2, tileSize - border * 2);
};
