module.exports = {
	theme: {
		colors: {
			'color-link': '#f11',
			'color-link-hover': '#c11',
			'black': '#000',
			'red': '#f00',
			'orange': '#f80',
			'green': '#0a0',
			'dark-red': '#600',
			'dark-orange': '#630',
			'dark-green': '#060',
			'color-text': '#ddd',
			'gray-0b': '#0b0b0b',
			'gray-16': '#161616',
			'gray-22': '#222',
			'color-ban': '#666',
			'color-path': '#f66',
			'no-data': '#666',
			'underline-leviathan': '#810017',
			'underline-devil': '#CB2012',
			'underline-golden': '#FCBE34',
			'underline-silver': '#9CA4AA',
			'underline-bronze': '#765A46',
			'underline-default': '#434343',
			'tooltip-background': '#4449',
		},
		extend: {
			gridTemplateColumns: {
				// Admin
				'admin-custom-entries': createMinmaxGrid([1, 4, 3, 1.5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 2.5, 1, 1]),
				'admin-custom-leaderboards': createMinmaxGrid([1, 4, 4, 1, 1, 1, 1, 1, 4, 2, 1, 1]),
				'admin-donations': createMinmaxGrid([1, 3, 1, 1, 3, 2, 4, 1, 1, 1]),
				'admin-mods': createMinmaxGrid([1, 4, 1, 2, 5, 5, 3, 5, 1, 1]),
				'admin-players': createMinmaxGrid([1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 2, 1, 1, 1, 1, 1, 1]),
				'admin-spawnsets': createMinmaxGrid([1, 1, 4, 2, 8, 2, 1, 1, 1]),
				'admin-titles': createMinmaxGrid([1, 8, 1, 1]),
				'admin-users': createMinmaxGrid([3, 2, 1, 1, 1, 1, 1, 1, 1, 1]),

				// Overviews
				'custom-leaderboards-2xl': createMinmaxGrid([2, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 1]),
				'custom-leaderboards-xl': createMinmaxGrid([2, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1]),
				'custom-leaderboards-lg': createMinmaxGrid([2, 2, 1, 2, 2, 1]),
				'custom-leaderboards-md': createMinmaxGrid([2, 2, 1.5, 1.5]),
				'custom-leaderboards-sm': createMinmaxGrid([1]),

				'spawnsets-xl': createMinmaxGrid([2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1]),
				'spawnsets-lg': createMinmaxGrid([2, 2, 1, 1.2, 1, 1, 1]),
				'spawnsets-md': createMinmaxGrid([2, 2, 1, 1.2]),
				'spawnsets-sm': createMinmaxGrid([1]),

				'mods-xl': createMinmaxGrid([3, 3, 2, 2.5, 1, 1]),
				'mods-lg': createMinmaxGrid([2, 2, 1, 1.5]),
				'mods-md': createMinmaxGrid([2, 2, 1, 2]),
				'mods-sm': createMinmaxGrid([1]),

				// Leaderboards
				'custom-leaderboard-2xl': '36px 32px minmax(64px, 100%) minmax(120px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(48px, 5%) minmax(120px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(72px, 5%) minmax(72px, 5%) minmax(72px, 5%) minmax(144px, 5%)',
				'custom-leaderboard-xl': '36px 32px minmax(64px, 100%) minmax(120px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(48px, 5%) minmax(120px, 5%) minmax(40px, 5%) minmax(40px, 5%)',
				'custom-leaderboard-lg': '36px 32px minmax(64px, 100%) minmax(120px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(48px, 5%) minmax(120px, 5%)',
				'custom-leaderboard-md': '36px 32px minmax(64px, 100%) minmax(120px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(40px, 5%) minmax(48px, 5%)',
				'custom-leaderboard-sm': '36px 32px minmax(64px, 100%) minmax(120px, 5%)',

				'leaderboard-xl': 'minmax(36px, 72px) 32px minmax(64px, 100%) minmax(80px, 80px) minmax(40px, 5%) minmax(40px, 5%) minmax(48px, 5%) minmax(120px, 8%) minmax(112px, 8%) minmax(80px, 8%) minmax(80px, 8%) minmax(48px, 8%) minmax(56px, 8%)',
				'leaderboard-lg': 'minmax(36px, 72px) 32px minmax(64px, 100%) minmax(80px, 80px) minmax(40px, 5%) minmax(40px, 5%) minmax(48px, 5%) minmax(120px, 8%)',
				'leaderboard-md': 'minmax(36px, 72px) 32px minmax(64px, 100%) minmax(80px, 80px) minmax(40px, 5%) minmax(40px, 5%) minmax(48px, 5%)',
				'leaderboard-sm': 'minmax(36px, 72px) 32px minmax(64px, 100%) minmax(80px, 80px)',

				'player-settings-xl': 'minmax(36px, 72px) 32px minmax(64px, 100%) minmax(80px, 80px) minmax(70px, 5%) minmax(50px, 5%) minmax(50px, 5%) minmax(50px, 5%) minmax(70px, 8%) minmax(90px, 8%) minmax(60px, 8%) minmax(110px, 8%)',
				'player-settings-lg': 'minmax(36px, 72px) 32px minmax(64px, 100%) minmax(80px, 80px) minmax(70px, 5%) minmax(50px, 5%) minmax(50px, 5%) minmax(50px, 5%)',
				'player-settings-md': 'minmax(36px, 72px) 32px minmax(64px, 100%) minmax(80px, 80px) minmax(70px, 5%) minmax(50px, 5%) minmax(50px, 5%)',
				'player-settings-sm': 'minmax(36px, 72px) 32px minmax(64px, 100%) minmax(80px, 80px)',

				// Mod binaries
				'mod-binaries-xl': 'minmax(128px, 100%) 64px 128px 64px 128px',
				'mod-binaries-lg': 'minmax(128px, 100%) 64px 128px 64px 128px',
				'mod-binaries-md': 'minmax(128px, 100%) 64px 128px',
				'mod-binaries-sm': 'minmax(128px, 100%) 64px',
				'mod-binaries-xs': '100%',
			},
			maxHeight: {
				'screen-half': '50vh',
				'lb-name': '1.5rem',
			},
			spacing: {
				'128': '32rem',
				'160': '40rem',
				'192': '48rem',
				'224': '56rem',
				'256': '64rem',
			}
		},
		fontFamily: {
			'calibri': ['calibri', 'sans-serif'],
			'goethe': ['goethe'],
		}
	},
	variants: {
		extend: {
			backgroundColor: ['even', 'odd'],
			opacity: ['disabled'],
		}
	},
};

function createMinmaxGrid(array, min = "0") {
	let total = 0;
	for (let i = 0; i < array.length; i++) {
		total += array[i];
	}

	const multiplier = 100 / total;

	return array.map(value => "minmax(" + min + ", " + (value * multiplier) + "%)").join(' ');
}
