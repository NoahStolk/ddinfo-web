module.exports = {
	content: [
		'./**/*.{html,razor,tailwind}',
	],
	theme: {
		colors: {
			'color-link': '#f11',
			'color-link-hover': '#911',
			'white': '#fff',
			'black': '#000',
			'red': '#f00',
			'orange': '#d80',
			'yellow': '#bb0',
			'green': '#0a0',
			'color-text': '#ddd',
		},
		extend: {
			fontFamily: {
				'calibri': ['calibri', 'sans-serif'],
			},
		},
	},
	variants: {
		extend: {
			backgroundColor: ['even', 'odd'],
			opacity: ['disabled'],
		},
	},
};
