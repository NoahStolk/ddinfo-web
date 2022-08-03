window.c2d = {
	getContext: function (id) {
		var c = document.getElementById(id);
		dc = c.getContext("2d");
		return dc;
	},

	clearRect: function (id, x, y, w, h) {
		var dc = c2d.getContext(id);
		dc.clearRect(x, y, w, h);
	},

	setFillStyle: function (id, fs) {
		var dc = c2d.getContext(id);
		dc.fillStyle = fs;
	},
	setStrokeStyle: function (id, ss) {
		var dc = c2d.getContext(id);
		dc.strokeStyle = ss;
	},
	setLineWidth: function (id, lw) {
		var dc = c2d.getContext(id);
		dc.lineWidth = lw;
	},
	beginPath: function (id) {
		var dc = c2d.getContext(id);
		dc.beginPath();
	},
	closePath: function (id) {
		var dc = c2d.getContext(id);
		dc.closePath();
	},
	moveTo: function (id, x, y) {
		var dc = c2d.getContext(id);
		dc.moveTo(x, y);
	},
	lineTo: function (id, x, y) {
		var dc = c2d.getContext(id);
		dc.lineTo(x, y);
	},
	arc: function (id, x, y, radius, startAngle, endAngle, anticlockwise) {
		var dc = c2d.getContext(id);
		dc.arc(x, y, radius, startAngle, endAngle, anticlockwise !== 0);
	},
	fill: function (di) {
		var dc = c2d.getContext(di);
		dc.fill();
	},
	stroke: function (di) {
		var dc = c2d.getContext(di);
		dc.stroke();
	},
};
