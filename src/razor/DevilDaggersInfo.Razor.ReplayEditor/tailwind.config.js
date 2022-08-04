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
			'green': '#0a0',
			'dark-red': '#600',
			'dark-orange': '#640',
			'dark-green': '#060',
			'darker-red': '#300',
			'darker-orange': '#320',
			'darker-green': '#030',
			'color-text': '#ddd',
			'gray-1': '#0c0c0c',
			'gray-2': '#181818',
			'gray-3': '#242424',
			'gray-4': '#303030',
			'gray-5': '#3c3c3c',
			'gray-6': '#484848',
			'gray-7': '#545454',
			'gray-8': '#606060',
			'gray-9': '#6c6c6c',
		},
		extend: {
			fontFamily: {
				'calibri': ['calibri', 'sans-serif'],
			},
			maxWidth: {
				'arena': '512px', // 10 * 51 + 2
			},
		},
	},
};
