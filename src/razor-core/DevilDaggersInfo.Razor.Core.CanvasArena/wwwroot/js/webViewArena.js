function renderTile(dc, r, g, b, x, y, tileSize) {
	dc.strokeStyle = "rgba(" + r + ", " + g + ", " + b + ", 0.75)";
	dc.fillStyle = "rgb(" + r + ", " + g + ", " + b + ")";

	const border = 1;
	dc.strokeRect(x * tileSize + 0.5, y * tileSize + 0.5, tileSize - 1, tileSize - 1);
	dc.fillRect(x * tileSize + border, y * tileSize + border, tileSize - border * 2, tileSize - border * 2);
}

function renderShrink(dc, canvasCenter, shrinkRadius, tileUnit, tileSize) {
	dc.strokeStyle = "#f08";
	dc.lineWidth = 1;
	dc.beginPath();
	dc.arc(canvasCenter, canvasCenter, shrinkRadius / tileUnit * tileSize, 0, 3.1415 * 2);
	dc.stroke();
}

function renderRaceDagger(dc, canvasCenter, daggerX, daggerY, tileUnit, tileSize) {
	const daggerCenterX = canvasCenter + daggerX / tileUnit * tileSize;
	const daggerCenterY = canvasCenter + daggerY / tileUnit * tileSize;

	dc.beginPath();
	dc.moveTo(daggerCenterX, daggerCenterY + 6);
	dc.lineTo(daggerCenterX + 4, daggerCenterY - 6);
	dc.lineTo(daggerCenterX + 1, daggerCenterY - 6);
	dc.lineTo(daggerCenterX + 1, daggerCenterY - 10);
	dc.lineTo(daggerCenterX - 1, daggerCenterY - 10);
	dc.lineTo(daggerCenterX - 1, daggerCenterY - 6);
	dc.lineTo(daggerCenterX - 4, daggerCenterY - 6);
	dc.closePath();

	dc.fillStyle = "#444";
	dc.fill();

	dc.strokeStyle = "#fff";
	dc.stroke();
}

function renderPlayer(dc, canvasCenter) {
	dc.beginPath();
	dc.moveTo(canvasCenter, canvasCenter + 3);
	dc.lineTo(canvasCenter + 3, canvasCenter);
	dc.lineTo(canvasCenter, canvasCenter - 3);
	dc.lineTo(canvasCenter - 3, canvasCenter);
	dc.closePath();

	dc.fillStyle = "#f00";
	dc.fill();

	dc.strokeStyle = "#fff";
	dc.stroke();
}

window.drawArena = (id, colors, canvasSize, shrinkRadius, race, daggerX, daggerY) => {
	const dc = c2d.getContext(id);
	const canvasCenter = canvasSize / 2;
	const tileUnit = 4; // Tiles are 4 units in width/length in the game.
	const arenaDimension = 51;
	const tileSize = canvasSize / arenaDimension;
	
	dc.clearRect(0, 0, canvasSize, canvasSize);

	for (let i = 0; i < arenaDimension; i++) {
		for (let j = 0; j < arenaDimension; j++) {
			const color = colors[i * arenaDimension + j];
			const r = color >> 24 & 0xFF;
			const g = color >> 16 & 0xFF;
			const b = color >> 8 & 0xFF;
			if (!(r === 0 && g === 0 && b === 0))
				renderTile(dc, r, g, b, i, j, tileSize);
		}
	}

	if (shrinkRadius > 0 && shrinkRadius <= 100)
		renderShrink(dc, canvasCenter, shrinkRadius, tileUnit, tileSize);
	
	renderPlayer(dc, canvasCenter);
	
	if (race)
		renderRaceDagger(dc, canvasCenter, daggerX, daggerY, tileUnit, tileSize);
};
