module.exports = {
	content: [
		'./**/*.{html,razor,hack}',
	],
	theme: {
		colors: {
			'color-link': '#f11',
			'color-link-hover': '#911',
			'white': '#fff',
			'black': '#000',
			'red': '#f00',
			'orange': '#fa0',
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
			'gray-5': '#3C3C3C',
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
				'admin-users': createMinmaxGrid([3, 2, 1, 1, 1, 1, 1, 1, 1]),

				// Overviews
				'spawnsets-xl': createMinmaxGrid([2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1]),
				'spawnsets-lg': createMinmaxGrid([2, 2, 1, 1.2, 1, 1, 1]),
				'spawnsets-md': createMinmaxGrid([2, 2, 1, 1.2]),
				'spawnsets-sm': createMinmaxGrid([1]),

				'custom-leaderboards-2xl': createMinmaxGrid([2, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 1]),
				'custom-leaderboards-xl': createMinmaxGrid([2, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1]),
				'custom-leaderboards-lg': createMinmaxGrid([2, 2, 1, 2, 2, 1]),
				'custom-leaderboards-md': createMinmaxGrid([2, 2, 1.5, 1.5]),
				'custom-leaderboards-sm': createMinmaxGrid([1]),

				'mods-xl': createMinmaxGrid([3, 3, 2, 2.5, 1, 1]),
				'mods-lg': createMinmaxGrid([2, 2, 1, 1.5]),
				'mods-md': createMinmaxGrid([2, 2, 1, 2]),
				'mods-sm': createMinmaxGrid([1]),

				// Spawnsets
				'spawnset-lg': '28rem auto',

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

				// Mods
				'mod-binaries-lg': 'minmax(160px, 100%) 160px 160px 160px 160px',
				'mod-binaries-md': 'minmax(160px, 100%) 160px 160px',
				'mod-binaries-sm': 'minmax(160px, 100%) 160px',
				'mod-binaries-xs': '100%',

				'mod-assets-lg': 'minmax(160px, 50%) minmax(160px, 50%) 160px 160px 160px',
				'mod-assets-md': 'minmax(160px, 50%) minmax(160px, 50%) 160px',
				'mod-assets-sm': 'minmax(160px, 50%) minmax(160px, 50%)',
				'mod-assets-xs': '100%',

				'mod-loudness-lg': 'minmax(160px, 50%) minmax(160px, 50%) 160px 160px 160px',
				'mod-loudness-md': 'minmax(160px, 50%) minmax(160px, 50%) 160px',
				'mod-loudness-sm': 'minmax(160px, 50%) minmax(160px, 50%)',
				'mod-loudness-xs': '100%',

				// Wiki (enemies)
				'enemies-summary-xs': createMinmaxGrid([5, 1, 1, 4]),
				'enemies-summary-md': createMinmaxGrid([5, 1, 1, 4, 2, 2]),
				'enemies-summary-lg': createMinmaxGrid([5, 1, 1, 3, 2, 2, 1.25, 1.25, 5]),

				'enemies-damage-stats-lg': createMinmaxGrid([5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1]),
				'enemies-damage-stats-lg-v1': createMinmaxGrid([5, 1, 1, 1, 1, 1, 1, 1, 1]),
			},
			maxWidth: {
				'arena-table': '410px', // 8 * 51 + 2
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
				'16-9': '56.25%',
				'16-9/2': '28.125%',
				'screen-width': 'calc(100% - 3rem)',
			},
			transitionProperty: {
				'width': 'width',
				'spacing': 'margin, padding',
			},
		},
		fontFamily: {
			'calibri': ['calibri', 'sans-serif'],
			'goethe': ['goethe'],
		},
	},
	variants: {
		extend: {
			backgroundColor: ['even', 'odd'],
			opacity: ['disabled'],
		},
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
