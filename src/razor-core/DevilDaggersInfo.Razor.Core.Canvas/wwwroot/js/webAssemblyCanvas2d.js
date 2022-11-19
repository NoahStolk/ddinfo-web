function getContext(id) {
	const c = document.getElementById(id);
	if (c == null)
		throw new Error(`Element with ID '${id}' does not exist.`);
	return c.getContext("2d");
}

export function clearRect(id, x, y, w, h) {
	getContext(id).clearRect(x, y, w, h);
}

export function fillRect(id, x, y, w, h) {
	getContext(id).fillRect(x, y, w, h);
}

export function setFillStyle(id, fs) {
	getContext(id).fillStyle = fs;
}

export function setStrokeStyle(id, ss) {
	getContext(id).strokeStyle = ss;
}

export function setLineWidth(id, lw) {
	getContext(id).lineWidth = lw;
}

export function setFont(id, font) {
	getContext(id).font = font;
}

export function setTextAlign(id, textAlign) {
	getContext(id).textAlign = textAlign;
}

export function beginPath(id) {
	getContext(id).beginPath();
}

export function closePath(id) {
	getContext(id).closePath();
}

export function moveTo(id, x, y) {
	getContext(id).moveTo(x, y);
}

export function lineTo(id, x, y) {
	getContext(id).lineTo(x, y);
}

export function arc(id, x, y, radius, startAngle, endAngle, anticlockwise) {
	getContext(id).arc(x, y, radius, startAngle, endAngle, anticlockwise !== 0);
}

export function fill(id) {
	getContext(id).fill();
}

export function stroke(id) {
	getContext(id).stroke();
}

export function strokeText(id, t, x, y) {
	getContext(id).strokeText(t, x, y);
}

export function save(id) {
	getContext(id).save();
}

export function restore(id) {
	getContext(id).restore();
}

export function translate(id, x, y) {
	getContext(id).translate(x, y);
}

export function rotate(id, angle) {
	getContext(id).rotate(angle);
}
