function getContext(id) {
	const c = document.getElementById(id);
	if (c == null)
		throw new Error(`Element with ID '${id}' does not exist.`);
	return c.getContext("2d");
}

export function clearRect(id, x, y, w, h) {
	const dc = getContext(id);
	dc.clearRect(x, y, w, h);
}

export function setFillStyle(id, fs) {
	const dc = getContext(id);
	dc.fillStyle = fs;
}

export function setStrokeStyle(id, ss) {
	const dc = getContext(id);
	dc.strokeStyle = ss;
}

export function setLineWidth(id, lw) {
	const dc = getContext(id);
	dc.lineWidth = lw;
}

export function beginPath(id) {
	const dc = getContext(id);
	dc.beginPath();
}

export function closePath(id) {
	const dc = getContext(id);
	dc.closePath();
}

export function moveTo(id, x, y) {
	const dc = getContext(id);
	dc.moveTo(x, y);
}

export function lineTo(id, x, y) {
	const dc = getContext(id);
	dc.lineTo(x, y);
}

export function arc(id, x, y, radius, startAngle, endAngle, anticlockwise) {
	const dc = getContext(id);
	dc.arc(x, y, radius, startAngle, endAngle, anticlockwise !== 0);
}

export function fill(di) {
	const dc = getContext(di);
	dc.fill();
}

export function stroke(di) {
	const dc = getContext(di);
	dc.stroke();
}
