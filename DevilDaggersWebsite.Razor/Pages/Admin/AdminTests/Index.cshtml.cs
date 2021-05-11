using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Caches.SpawnsetData;
using DevilDaggersWebsite.Caches.SpawnsetHash;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Admin.AdminTests
{
	public class IndexModel : PageModel
	{
		private readonly IWebHostEnvironment _env;

		public IndexModel(IWebHostEnvironment env)
			=> _env = env;

		public async Task OnPostTestBotAsync()
			=> await DiscordLogger.Instance.TryLog(Channel.TestMonitoring, _env.EnvironmentName, "Hello, this is a test message sent from an external environment.");

		public void OnPostTestException()
			=> throw new("TEST EXCEPTION with 3 inner exceptions", new("Inner exception message", new("Another inner exception message", new("Big Discord embed"))));

		public async Task OnPostInitiateLeaderboardStatisticsCache()
			=> await LeaderboardStatisticsCache.Instance.Initiate(_env);

		public async Task OnPostClearSpawnsetHashCache()
			=> await SpawnsetHashCache.Instance.Clear(_env);

		public async Task OnPostClearLeaderboardHistoryCache()
			=> await LeaderboardHistoryCache.Instance.Clear(_env);

		public async Task OnPostClearSpawnsetDataCache()
			=> await SpawnsetDataCache.Instance.Clear(_env);

		public async Task OnPostClearModDataCache()
			=> await ModArchiveCache.Instance.Clear(_env);

		public async Task OnPostLogCaches()
		{
			StringBuilder sb = new("\n");
			sb.AppendLine("**Static caches:**");
			sb.AppendLine(LeaderboardStatisticsCache.Instance.LogState(_env));
			sb.AppendLine("**Dynamic caches:**");
			sb.AppendLine(LeaderboardHistoryCache.Instance.LogState(_env));
			sb.AppendLine(ModArchiveCache.Instance.LogState(_env));
			sb.AppendLine(SpawnsetDataCache.Instance.LogState(_env));
			sb.AppendLine(SpawnsetHashCache.Instance.LogState(_env));
			await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, _env.EnvironmentName, sb.ToString());
		}

		public async Task OnPostFixModBinaryNames()
		{
			const string n = @"dd-ADDEDSPICE
audio-ADDEDSPICE
dd-ADDEDSUGAR
audio-ADDEDSUGAR
dd_ao
dd-blood-of-the-fang
dd_BLOODRUNS
dd-greenshift
dd-blueshift
dd-daytime
dd-downwell-v1
dd-downwell-v2
dd-greyscale
dd-magenta-lobby-dagger
dd-minimalism
dd-minimalism-v2
dd-papyrus
dd-psychedelic
dd-psychedelic-2
dd-scary
dd-static
dd-tiny
dd-with-stripper-lights
dd-blue-but-with-red-black-and-green-highlights-also-orange-void
dd-but-you-drank-too-much-nyquil
dd-color-out-of-hell
audio-cc-allowed
audio-cc-prohibited
dd-cc
dd-CustomMix2
audio-CustomMix2
dd-CustomMix
dd-dabSkull
audioLESSGO
ddababyLESSGO
ddddd
audioquid-prohibited
ddquid
dd_nohandclamp
dd_noclamp_drylandcompatible
dd_noclamp
dd_devilnails_post_luts
audio_devilnails_level2
dd_devilnails_level3hand
dd_devilnails_level4hand
audio_devilnails_level1
dd_devilnails_daggers_logo
audio_devilnails_slash
dd_devilnails_level2hand
dd_devilnails_level1hand
audio_devilnails_levels3_4
audio_devilnails_lobby_music
audio_devilnails_menu
audio_devilnails_slash_splash
dd_ditherdaggers2
dd_dryland
dd-EGGILEGGERS
audio-EGGILEGGERS(fkaEARBLEEDER)
dd-emerald-trim
audio-EVERYTHING IS ME
dd_frostbite_unprohibited
dd_frostbite_prohibited
dd_frostbite_prohibited_blue
dd-grape daggers
dd_girly
dd_gun
dd_hands
dd_plus
dd_angelarrows
dd_bloons
dd-heart-monitor-splash
dd-HELLISCOLD
dd-HELLTASTESNASTY
dd-Ice
dd-linguchad
dd_goldhoming
dd_hominglogo
dd_tile_duskbox
dd_tile_blacktile
dd_(underline)-darker-vanilla-tile
dd_postlut_deluxepaint
dd_postlut_NES
dd_postlut_gameboy
dd_postlut_VIC20
dd_postlut_C64
dd_postlut_CGA
dd_postlut_EGA
dd_postlut_atari2600
dd_postlut_quake
dd_postlut_MSX2
dd_postlut_MSX
audiocookle
dd-MAGICARROWZ
audio-MAGICARROWZ
dd-MAGICBULLETZ
audio-MAGICBULLETZ
dd-MAGICMISSILEZ
audio-MAGICMISSILEZ
audio-manspaghetti
audio-end
audio-nether
dd-end
dd-nether
audio-trim
audio-no death sound
dd-neon-knights
dd-newports
audio-newports
dd-tiny
audio-tiny
dd-64-color
dd-alien
dd-alien-2
dd-bw-extreme
dd-dark-blue
dd-green
dd-greenscreen
dd-jank
dd-nored
dd-orange-with-green-void
dd-virtual-boy
dd-shadows_die_thousand-textures
dd-shadows_die_thousand-models
dd-shadows_die_thousand-models-prohibited
dd-shootingstarz-models
dd-shootingstarz-models-prohibited
dd-shootingstarz-textures
dd-spidorlingu
dd-supervixen-hand-and-dagger
dd-supervixen-other
dd_transmution
dd_(underline)-crimson-gem-2x
dd_(underline)-custom-tile-and-tileside
dd_(underline)-cyan-gem-2x
dd_(underline)-darker-vanilla-tile
dd_(underline)-green-gem-2x
dd_(underline)-highcontrast-fonts&icons
dd_(underline)-lightning-level3-hands
dd_(underline)-lightning-level4-hands
dd_(underline)-lime-gem-2x
dd_(underline)-magenta-gem-2x
dd_(underline)-old-purple-hand4off
dd_(underline)-orange-gem-2x
dd_(underline)-pink-ghostpede
dd_(underline)-postproc
dd_(underline)-purple-gem-2x
dd_(underline)-purple-orb
dd_(underline)-red-arms-pale-squids
dd_(underline)-red-gem-2x
dd_(underline)-redspiders
dd_(underline)-spidorlingu
dd_(underline)-stronger-transmuted-skulls
dd_(underline)-teal-gem-2x
dd_(underline)-v2-red-centipede
dd_(underline)-white-ghostpede
dd_(underline)-yellow-gem-2x
audio_(underline)-mute-high-stress-sounds
audio_(underline)-silent-menu&startup
dd_(underline)-black-spider-eggs
dd_(underline)-bloddy-ghostpede
dd_(underline)-blue-gem-2x
dd-v2-gigapedes
dd-aetherea
dd-aetherea-shaders";

			const string nn = @"dd-Added Spice-ADDEDSPICE
audio-Added Spice-ADDEDSPICE
dd-Added Sugar-ADDEDSUGAR
audio-Added Sugar-ADDEDSUGAR
dd-Ambient Occlusion-ao
dd-Blood of the Fang-blood-of-the-fang
dd-Blood Runs-BLOODRUNS
dd-Blueshift Ripoffs-greenshift
dd-Blueshift-blueshift
dd-Braden's Gfxpacks-daytime
dd-Braden's Gfxpacks-downwell-v1
dd-Braden's Gfxpacks-downwell-v2
dd-Braden's Gfxpacks-greyscale
dd-Braden's Gfxpacks-magenta-lobby-dagger
dd-Braden's Gfxpacks-minimalism
dd-Braden's Gfxpacks-minimalism-v2
dd-Braden's Gfxpacks-papyrus
dd-Braden's Gfxpacks-psychedelic
dd-Braden's Gfxpacks-psychedelic-2
dd-Braden's Gfxpacks-scary
dd-Braden's Gfxpacks-static
dd-Braden's Gfxpacks-tiny
dd-Braden's Gfxpacks-with-stripper-lights
dd-Braden's Gfxpacks-blue-but-with-red-black-and-green-highlights-also-orange-void
dd-Braden's Gfxpacks-but-you-drank-too-much-nyquil
dd-Color out of Hell-color-out-of-hell
audio-Cursed Cutters-allowed
audio-Cursed Cutters-prohibited
dd-Cursed Cutters-cc
dd-Custom Mix II-CustomMix2
audio-Custom Mix II-CustomMix2
dd-Custom Mix-CustomMix
dd-dabSkull-dabSkull
audio-ddababy-LESSGO
ddababy-ddababy-LESSGO
dd-ddddd-ddddd
audio-ddquid-prohibited
dd-ddquid-ddquid
dd-Devil Nails-nohandclamp
dd-Devil Nails-noclamp_drylandcompatible
dd-Devil Nails-noclamp
dd-Devil Nails-devilnails_post_luts
audio-Devil Nails-devilnails_level2
dd-Devil Nails-devilnails_level3hand
dd-Devil Nails-devilnails_level4hand
audio-Devil Nails-devilnails_level1
dd-Devil Nails-devilnails_daggers_logo
audio-Devil Nails-devilnails_slash
dd-Devil Nails-devilnails_level2hand
dd-Devil Nails-devilnails_level1hand
audio-Devil Nails-devilnails_levels3_4
audio-Devil Nails-devilnails_lobby_music
audio-Devil Nails-devilnails_menu
audio-Devil Nails-devilnails_slash_splash
dd-Dither Daggers-ditherdaggers2
dd-Dryland-dryland
dd-Eggil Eggers-EGGILEGGERS
audio-Eggil Eggers-EGGILEGGERS(fkaEARBLEEDER)
dd-Emerald Daggers-emerald-trim
audio-EVERYTHING IS ME-EVERYTHING IS ME
dd-Frostbite-frostbite_unprohibited
dd-Frostbite-frostbite_prohibited
dd-Frostbite-frostbite_prohibited_blue
dd-Grape Daggers-grape daggers
dd-HealerGirl Mods-girly
dd-HealerGirl Mods-gun
dd-HealerGirl Mods-hands
dd-HealerGirl Mods-plus
dd-HealerGirl Mods-angelarrows
dd-HealerGirl Mods-bloons
dd-Heart Monitor Splash Screen-heart-monitor-splash
dd-Hell is Cold-HELLISCOLD
dd-Hell Tastes Nasty-HELLTASTESNASTY
dd-Ice-Ice
dd-Linguchad-linguchad
dd-LocoCaesar's mini mods-goldhoming
dd-LocoCaesar's mini mods-hominglogo
dd-LocoCaesar's mini mods-tile_duskbox
dd-LocoCaesar's mini mods-tile_blacktile
dd-LocoCaesar's post_luts-(underline)-darker-vanilla-tile
dd-LocoCaesar's post_luts-postlut_deluxepaint
dd-LocoCaesar's post_luts-postlut_NES
dd-LocoCaesar's post_luts-postlut_gameboy
dd-LocoCaesar's post_luts-postlut_VIC20
dd-LocoCaesar's post_luts-postlut_C64
dd-LocoCaesar's post_luts-postlut_CGA
dd-LocoCaesar's post_luts-postlut_EGA
dd-LocoCaesar's post_luts-postlut_atari2600
dd-LocoCaesar's post_luts-postlut_quake
dd-LocoCaesar's post_luts-postlut_MSX2
dd-LocoCaesar's post_luts-postlut_MSX
audio-lossofbraincells-cookle
dd-Magic Arrowz-MAGICARROWZ
audio-Magic Arrowz-MAGICARROWZ
dd-Magic Bulletz-MAGICBULLETZ
audio-Magic Bulletz-MAGICBULLETZ
dd-Magic Missilez-MAGICMISSILEZ
audio-Magic Missilez-MAGICMISSILEZ
audio-Manspaghetti Splash Music-manspaghetti
audio-MCDD (Minecraft)-end
audio-MCDD (Minecraft)-nether
dd-MCDD (Minecraft)-end
dd-MCDD (Minecraft)-nether
audio-Menu Music-trim
audio-Muted death sound-no death sound
dd-Neon Knights-neon-knights
dd-Newports-newports
audio-Newports-newports
dd-Nifty Daggers-tiny
audio-Nifty Daggers-tiny
dd-post_lut Experiments-64-color
dd-post_lut Experiments-alien
dd-post_lut Experiments-alien-2
dd-post_lut Experiments-bw-extreme
dd-post_lut Experiments-dark-blue
dd-post_lut Experiments-green
dd-post_lut Experiments-greenscreen
dd-post_lut Experiments-jank
dd-post_lut Experiments-nored
dd-post_lut Experiments-orange-with-green-void
dd-post_lut Experiments-virtual-boy
dd-Shadows Die Thousand-shadows_die_thousand-textures
dd-Shadows Die Thousand-shadows_die_thousand-models
dd-Shadows Die Thousand-shadows_die_thousand-models-prohibited
dd-ShootingStarz-shootingstarz-models
dd-ShootingStarz-shootingstarz-models-prohibited
dd-ShootingStarz-shootingstarz-textures
dd-Spidorlingu-spidorlingu
dd-Supervixen-supervixen-hand-and-dagger
dd-Supervixen-supervixen-other
dd-Transmutation-transmution
dd-Underline-(underline)-crimson-gem-2x
dd-Underline-(underline)-custom-tile-and-tileside
dd-Underline-(underline)-cyan-gem-2x
dd-Underline-(underline)-darker-vanilla-tile
dd-Underline-(underline)-green-gem-2x
dd-Underline-(underline)-highcontrast-fonts&icons
dd-Underline-(underline)-lightning-level3-hands
dd-Underline-(underline)-lightning-level4-hands
dd-Underline-(underline)-lime-gem-2x
dd-Underline-(underline)-magenta-gem-2x
dd-Underline-(underline)-old-purple-hand4off
dd-Underline-(underline)-orange-gem-2x
dd-Underline-(underline)-pink-ghostpede
dd-Underline-(underline)-postproc
dd-Underline-(underline)-purple-gem-2x
dd-Underline-(underline)-purple-orb
dd-Underline-(underline)-red-arms-pale-squids
dd-Underline-(underline)-red-gem-2x
dd-Underline-(underline)-redspiders
dd-Underline-(underline)-spidorlingu
dd-Underline-(underline)-stronger-transmuted-skulls
dd-Underline-(underline)-teal-gem-2x
dd-Underline-(underline)-v2-red-centipede
dd-Underline-(underline)-white-ghostpede
dd-Underline-(underline)-yellow-gem-2x
audio-Underline-(underline)-mute-high-stress-sounds
audio-Underline-(underline)-silent-menu&startup
dd-Underline-(underline)-black-spider-eggs
dd-Underline-(underline)-bloddy-ghostpede
dd-Underline-(underline)-blue-gem-2x
dd-V2 Gigapedes-v2-gigapedes
dd-AETHEREA-aetherea
dd-AETHEREA-aetherea-shaders";

			List<(string Old, string New)> names = new();
			string[] nSplit = n.Split("\r\n");
			string[] nnSplit = nn.Split("\r\n");
			for (int i = 0; i < nSplit.Length; i++)
				names.Add((nSplit[i], nnSplit[i]));

			foreach (string path in Directory.GetFiles(Path.Combine(_env.WebRootPath, "mods"), "*.zip"))
			{
				using ZipArchive archive = new(System.IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite), ZipArchiveMode.Update);

				List<ZipArchiveEntry> originalEntries = new(archive.Entries);
				for (int i = 0; i < originalEntries.Count; i++)
				{
					ZipArchiveEntry entry = originalEntries[i];
					string? newName = names.Find(n => n.Old == entry.Name).New;
					if (newName == null)
					{
						await DiscordLogger.Instance.TryLog(Channel.TestMonitoring, _env.EnvironmentName, $"Skipping `{entry.Name}`.");
						continue;
					}

					ZipArchiveEntry newEntry = archive.CreateEntry(newName);
					using (Stream a = entry.Open())
					using (Stream b = newEntry.Open())
						a.CopyTo(b);
					entry.Delete();

					await DiscordLogger.Instance.TryLog(Channel.TestMonitoring, _env.EnvironmentName, $"Renaming `{entry.Name}` to `{newName}`.");
				}
			}
		}
	}
}
