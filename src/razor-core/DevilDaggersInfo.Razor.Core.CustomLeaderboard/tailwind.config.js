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
			gridTemplateColumns: {
				'custom-leaderboard': '36px minmax(72px, 100%) minmax(72px, 100%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(120px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(64px, 5%) minmax(64px, 5%) minmax(64px, 5%)',
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
