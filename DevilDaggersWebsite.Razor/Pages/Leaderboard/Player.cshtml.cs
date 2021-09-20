using DevilDaggersCore.Game;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Clients;
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
using Lb = DevilDaggersWebsite.Clients.Leaderboard;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class PlayerModel : PageModel
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

		private int _playerId;

		public PlayerModel(IWebHostEnvironment environment, ApplicationDbContext dbContext, LeaderboardHistoryCache leaderboardHistoryCache)
		{
			_environment = environment;
			_dbContext = dbContext;
			_leaderboardHistoryCache = leaderboardHistoryCache;
		}

		public static string[] DaggerNames { get; } = new[] { "leviathan", "devil", "golden", "silver", "bronze", "default" };

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
			Player = _dbContext.Players.AsNoTracking().FirstOrDefault(p => p.Id == PlayerId);

			Entry = await LeaderboardClient.Instance.GetUserById(PlayerId);
			if (Entry != null)
			{
				Death = GameInfo.GetDeathByType(GameVersion.V31, Entry.DeathType);
				Dagger = GameInfo.GetDaggerFromTenthsOfMilliseconds(GameVersion.V31, Entry.Time);
				CountryName = Player?.CountryCode != null ? UserUtils.CountryNames.ContainsKey(Player.CountryCode) ? UserUtils.CountryNames[Player.CountryCode] : null : null;

				Dictionary<string, int> aliases = new();
				foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(_environment.WebRootPath, "leaderboard-history"), "*.json"))
				{
					Lb leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
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
				var customEntriesQuery = _dbContext.CustomEntries
					.AsNoTracking()
					.Include(ce => ce.CustomLeaderboard)
					.Select(ce => new { ce.Time, ce.CustomLeaderboard, ce.PlayerId })
					.Where(ce => ce.PlayerId == PlayerId && !ce.CustomLeaderboard.IsArchived);

				foreach (var customEntry in customEntriesQuery)
				{
					int daggerIndex = (int)customEntry.CustomLeaderboard.GetDaggerFromTime(customEntry.Time);
					int[] daggerCounts = customEntry.CustomLeaderboard.Category switch
					{
						CustomLeaderboardCategory.Default => CustomDaggerCountsDefault,
						CustomLeaderboardCategory.TimeAttack => CustomDaggerCountsTimeAttack,
						CustomLeaderboardCategory.Speedrun => CustomDaggerCountsSpeedrun,
						_ => throw new NotSupportedException($"Dagger count for custom leaderboard category '{customEntry.CustomLeaderboard.Category}' is not supported."),
					};
					daggerCounts[daggerIndex]++;
				}

				var customLeaderboardsQuery = _dbContext.CustomLeaderboards
					.AsNoTracking()
					.Where(cl => !cl.IsArchived)
					.Select(cl => new { cl.Category });

				TotalDefaultCustomLeaderboards = customLeaderboardsQuery.Count(cl => cl.Category == CustomLeaderboardCategory.Default);
				TotalTimeAttackCustomLeaderboards = customLeaderboardsQuery.Count(cl => cl.Category == CustomLeaderboardCategory.TimeAttack);
				TotalSpeedrunCustomLeaderboards = customLeaderboardsQuery.Count(cl => cl.Category == CustomLeaderboardCategory.Speedrun);
				Mods = _dbContext.AssetMods
					.AsNoTracking()
					.Include(am => am.PlayerAssetMods)
					.Where(am => am.PlayerAssetMods.Any(pam => pam.PlayerId == PlayerId) && !am.IsHidden)
					.OrderByDescending(am => am.LastUpdated)
					.ToList();
				Spawnsets = _dbContext.SpawnsetFiles
					.AsNoTracking()
					.Where(sf => sf.PlayerId == PlayerId)
					.OrderByDescending(sf => sf.LastUpdated)
					.ToList();
			}
		}
	}
}
