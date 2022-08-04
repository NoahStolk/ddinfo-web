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
