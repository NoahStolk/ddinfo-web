using DevilDaggersCore.Game;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Io = System.IO;
using Lb = DevilDaggersWebsite.Dto.Leaderboard;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class PlayerModel : PageModel
	{
		private readonly IWebHostEnvironment _env;
		private readonly ApplicationDbContext _dbContext;

		private int _playerId;

		public PlayerModel(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			_env = env;
			_dbContext = dbContext;
		}

		public static string[] DaggerNames { get; } = new string[] { "leviathan", "devil", "golden", "silver", "bronze", "default" };

		public int PlayerId
		{
			get => _playerId;
			set
			{
				if (value <= 0)
					return;
				_playerId = value;
			}
		}

		public Player? Player { get; private set; }

		[BindProperty]
		public Entry? Entry { get; set; }

		public Death? Death { get; private set; }
		public Dagger? Dagger { get; private set; }
		public string? CountryName { get; private set; }

		public bool HasValidTop100Graph { get; private set; }
		public List<string> UsernameAliases { get; private set; } = new();

		public int? BestRankRecorded { get; private set; }

		public int[] CustomDaggerCountsDefault { get; } = new int[DaggerNames.Length];
		public int[] CustomDaggerCountsTimeAttack { get; } = new int[DaggerNames.Length];
		public int[] CustomDaggerCountsSpeedrun { get; } = new int[DaggerNames.Length];
		public int TotalDefaultCustomLeaderboards { get; private set; }
		public int TotalTimeAttackCustomLeaderboards { get; private set; }
		public int TotalSpeedrunCustomLeaderboards { get; private set; }

		public List<AssetMod>? Mods { get; private set; }
		public List<Entities.SpawnsetFile>? Spawnsets { get; private set; }

		public async Task OnGetAsync(int id)
		{
			PlayerId = Math.Max(1, id);
			Player = _dbContext.Players.FirstOrDefault(p => p.Id == PlayerId);

			Entry = await LeaderboardClient.Instance.GetUserById(PlayerId);
			if (Entry != null)
			{
				Death = GameInfo.GetDeathByType(GameVersion.V31, Entry.DeathType);
				Dagger = GameInfo.GetDaggerFromTime(GameVersion.V31, Entry.Time);
				CountryName = Player?.CountryCode != null ? UserUtils.CountryNames.ContainsKey(Player.CountryCode) ? UserUtils.CountryNames[Player.CountryCode] : null : null;

				Dictionary<string, int> aliases = new();
				foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
				{
					Lb leaderboard = LeaderboardHistoryCache.Instance.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
					Entry? historyEntry = leaderboard.Entries.Find(e => e.Id == PlayerId);
					if (historyEntry != null)
					{
						HasValidTop100Graph = true;

						if (!BestRankRecorded.HasValue || BestRankRecorded.Value > historyEntry.Rank)
							BestRankRecorded = historyEntry.Rank;

						if (Player?.HidePastUsernames != true && !string.IsNullOrWhiteSpace(historyEntry.Username) && historyEntry.Username != Entry.Username)
						{
							if (aliases.ContainsKey(historyEntry.Username))
								aliases[historyEntry.Username]++;
							else
								aliases.Add(historyEntry.Username, 1);
						}
					}
				}

				UsernameAliases = aliases.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
			}

			if (Player != null && _dbContext.CustomEntries.Any(ce => ce.PlayerId == PlayerId))
			{
				foreach (Entities.CustomEntry customEntry in _dbContext.CustomEntries.Include(ce => ce.CustomLeaderboard).Where(ce => ce.PlayerId == PlayerId))
				{
					if (customEntry.CustomLeaderboard.IsArchived)
						continue;

					switch (customEntry.CustomLeaderboard.Category)
					{
						case CustomLeaderboardCategory.Default:
							if (customEntry.Time >= customEntry.CustomLeaderboard.TimeLeviathan)
								CustomDaggerCountsDefault[0]++;
							else if (customEntry.Time >= customEntry.CustomLeaderboard.TimeDevil)
								CustomDaggerCountsDefault[1]++;
							else if (customEntry.Time >= customEntry.CustomLeaderboard.TimeGolden)
								CustomDaggerCountsDefault[2]++;
							else if (customEntry.Time >= customEntry.CustomLeaderboard.TimeSilver)
								CustomDaggerCountsDefault[3]++;
							else if (customEntry.Time >= customEntry.CustomLeaderboard.TimeBronze)
								CustomDaggerCountsDefault[4]++;
							else
								CustomDaggerCountsDefault[5]++;
							break;
						case CustomLeaderboardCategory.TimeAttack:
							if (customEntry.Time <= customEntry.CustomLeaderboard.TimeLeviathan)
								CustomDaggerCountsTimeAttack[0]++;
							else if (customEntry.Time <= customEntry.CustomLeaderboard.TimeDevil)
								CustomDaggerCountsTimeAttack[1]++;
							else if (customEntry.Time <= customEntry.CustomLeaderboard.TimeGolden)
								CustomDaggerCountsTimeAttack[2]++;
							else if (customEntry.Time <= customEntry.CustomLeaderboard.TimeSilver)
								CustomDaggerCountsTimeAttack[3]++;
							else if (customEntry.Time <= customEntry.CustomLeaderboard.TimeBronze)
								CustomDaggerCountsTimeAttack[4]++;
							else
								CustomDaggerCountsTimeAttack[5]++;
							break;
						case CustomLeaderboardCategory.Speedrun:
							if (customEntry.Time <= customEntry.CustomLeaderboard.TimeLeviathan)
								CustomDaggerCountsSpeedrun[0]++;
							else if (customEntry.Time <= customEntry.CustomLeaderboard.TimeDevil)
								CustomDaggerCountsSpeedrun[1]++;
							else if (customEntry.Time <= customEntry.CustomLeaderboard.TimeGolden)
								CustomDaggerCountsSpeedrun[2]++;
							else if (customEntry.Time <= customEntry.CustomLeaderboard.TimeSilver)
								CustomDaggerCountsSpeedrun[3]++;
							else if (customEntry.Time <= customEntry.CustomLeaderboard.TimeBronze)
								CustomDaggerCountsSpeedrun[4]++;
							else
								CustomDaggerCountsSpeedrun[5]++;
							break;
					}
				}

				TotalDefaultCustomLeaderboards = _dbContext.CustomLeaderboards.Count(cl => cl.Category == CustomLeaderboardCategory.Default && !cl.IsArchived);
				TotalTimeAttackCustomLeaderboards = _dbContext.CustomLeaderboards.Count(cl => cl.Category == CustomLeaderboardCategory.TimeAttack && !cl.IsArchived);
				TotalSpeedrunCustomLeaderboards = _dbContext.CustomLeaderboards.Count(cl => cl.Category == CustomLeaderboardCategory.Speedrun && !cl.IsArchived);
				Mods = _dbContext.AssetMods.Include(am => am.PlayerAssetMods).Where(am => am.PlayerAssetMods.Any(pam => pam.PlayerId == PlayerId)).OrderByDescending(am => am.LastUpdated).ToList();
				Spawnsets = _dbContext.SpawnsetFiles.Where(sf => sf.PlayerId == PlayerId).OrderByDescending(sf => sf.LastUpdated).ToList();
			}
		}
	}
}
