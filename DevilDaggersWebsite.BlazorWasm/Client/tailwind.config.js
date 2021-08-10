module.exports = {
	theme: {
		extend: {
			colors: {
				'color-link': '#f11',
				'color-link-hover': '#c11',
				'red': '#f00',
				'orange': '#f80',
				'green': '#0f0',
				'dark-red': '#600',
				'dark-orange': '#630',
				'dark-green': '#060',
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
				'tooltip-background': '#000b',
			},
			gridTemplateColumns: {
				'admin-custom-entries': createMinmaxGrid([1, 4, 3, 1.5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 2.5, 1, 1]),
				'admin-custom-leaderboards': createMinmaxGrid([1, 4, 4, 1, 1, 1, 1, 1, 4, 2, 1, 1]),
				'admin-donations': createMinmaxGrid([1, 3, 1, 1, 3, 2, 4, 1, 1, 1]),
				'admin-mods': createMinmaxGrid([1, 4, 1, 2, 5, 5, 3, 5, 1, 1]),
				'admin-players': createMinmaxGrid([1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 2, 1, 1, 1, 1, 1, 1]),
				'admin-spawnsets': createMinmaxGrid([1, 1, 4, 2, 8, 2, 1, 1, 1]),
				'admin-titles': createMinmaxGrid([1, 8, 1, 1]),
				'admin-users': createMinmaxGrid([3, 2, 1, 1, 1, 1, 1, 1, 1, 1]),
				'custom-leaderboard-xl': createMinmaxGrid([2, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1]),
				'custom-leaderboard-lg': createMinmaxGrid([2, 2, 1, 2, 2, 1]),
				'custom-leaderboard-md': createMinmaxGrid([2, 2, 1, 2]),
				'custom-leaderboard-sm': createMinmaxGrid([1]),
				'leaderboard-xl': createMinmaxGrid([4, 2, 14, 7, 4, 4, 8, 11, 9, 9, 9, 10, 9], "32px"),
				'leaderboard-lg': createMinmaxGrid([6, 3, 8, 10, 6, 6, 8, 10.5, 10.5, 10, 8, 10, 10], "32px"),
				'leaderboard-md': createMinmaxGrid([9, 4.5, 35, 14, 14, 14, 14], "32px"),
				'leaderboard-sm': createMinmaxGrid([18, 9, 41, 32], "32px"),
			},
			maxHeight: {
				'screen-half': '50vh',
				'lb-name': '1.5rem',
			}
		},
		fontFamily: {
			'calibri': ['calibri', 'sans-serif'],
			'goethe': ['goethe'],
		}
	}
};

function createMinmaxGrid(array, min = "0") {
	let total = 0;
	for (let i = 0; i < array.length; i++) {
		total += array[i];
	}

	const multiplier = 100 / total;

	return array.map(value => "minmax(" + min + ", " + (value * multiplier) + "%)").join(' ');
}
