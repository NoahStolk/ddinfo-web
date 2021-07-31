module.exports = {
	theme: {
		extend: {
			colors: {
				'red': '#f00',
				'color-link': '#f11',
				'color-link-hover': '#c11',
				'dark-red': '#600',
				'dark-orange': '#630',
				'color-text': '#ddd',
				'gray08': '#080808',
				'gray0B': '#0b0b0b',
				'gray11': '#111',
				'gray18': '#181818',
				'gray22': '#222',
				'gray66': '#666',
				'color-ban': '#666',
				'underline-leviathan': '#810017',
				'underline-devil': '#CB2012',
				'underline-golden': '#FCBE34',
				'underline-silver': '#9CA4AA',
				'underline-bronze': '#765A46',
				'underline-default': '#434343',
				'tooltip-background': '#000b'
			},
			gridTemplateColumns: {
				'leaderboard-xl': createMinmaxGrid([4, 2, 14, 7, 4, 4, 8, 11, 9, 9, 9, 10, 9]),
				'leaderboard-lg': createMinmaxGrid([6, 3, 8, 10, 6, 6, 8, 10.5, 10.5, 10, 8, 10, 10]),
				'leaderboard-md': createMinmaxGrid([9, 4.5, 35, 14, 14, 14, 14]),
				'leaderboard-sm': createMinmaxGrid([18, 9, 41, 32]),
				'admin-custom-entries': createMinmaxGrid([2, 10, 6, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 6, 5]),
				'admin-custom-leaderboards': createMinmaxGrid([4, 20, 10, 8, 8, 8, 8, 8, 8, 8, 5, 5])
			}
		},
		fontFamily: {
			'calibri': ['calibri', 'sans-serif'],
			'goethe': ['goethe']
		}
	}
};

function createMinmaxGrid(array) {
	return array.map(value => "minmax(0, " + value + "%)").join(' ');
}
