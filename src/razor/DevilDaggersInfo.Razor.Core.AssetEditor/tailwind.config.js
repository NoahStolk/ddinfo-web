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
			'color-ban': '#666',
			'color-path': '#faa',
			'no-data': '#666',
			'underline-leviathan': '#810017',
			'underline-devil': '#cb2012',
			'underline-golden': '#fcbe34',
			'underline-silver': '#9ca4aa',
			'underline-bronze': '#765a46',
			'underline-default': '#434343',
			'tooltip-background': '#4449',
		},
		extend: {
			fontFamily: {
				'calibri': ['calibri', 'sans-serif'],
				'goethe': ['goethe'],
			},
			gridTemplateColumns: {
			},
			maxHeight: {
				'screen-half': '50vh',
			},
			spacing: {
				'128': '32rem',
				'160': '40rem',
				'192': '48rem',
				'224': '56rem',
				'256': '64rem',
			},
			transitionProperty: {
				'width': 'width',
				'spacing': 'margin, padding',
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
