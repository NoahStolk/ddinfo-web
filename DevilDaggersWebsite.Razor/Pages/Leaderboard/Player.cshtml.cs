using DevilDaggersCore.Game;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

		public Dictionary<string, int> CustomDaggerCounts { get; } = new()
		{
			{ "leviathan", 0 },
			{ "devil", 0 },
			{ "golden", 0 },
			{ "silver", 0 },
			{ "bronze", 0 },
			{ "default", 0 },
		};

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
				HasValidTop100Graph = Entry.ExistsInHistory(_env); // TODO: Optimize.
				UsernameAliases = Entry.GetAllUsernameAliases(_env).Where(s => s != Entry.Username).ToList();
			}

			if (HasValidTop100Graph)
			{
				foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
				{
					Lb lb = JsonConvert.DeserializeObject<Lb>(Io.File.ReadAllText(leaderboardHistoryPath)) ?? throw new($"Corrupt leaderboard history file: {Io.Path.GetFileName(leaderboardHistoryPath)}");
					Entry? entry = lb.Entries.Find(e => e.Id == PlayerId);
					if (entry != null && (!BestRankRecorded.HasValue || BestRankRecorded.Value > entry.Rank))
						BestRankRecorded = entry.Rank;
				}
			}

			if (Player != null)
			{
				foreach (Entities.CustomEntry customEntry in _dbContext.CustomEntries.Include(ce => ce.CustomLeaderboard).Where(ce => ce.PlayerId == PlayerId))
				{
					string dagger = customEntry.CustomLeaderboard.GetDagger(customEntry.Time);
					if (CustomDaggerCounts.ContainsKey(dagger))
						CustomDaggerCounts[dagger]++;
				}

				Mods = _dbContext.AssetMods.Include(am => am.PlayerAssetMods).Where(am => am.PlayerAssetMods.Any(pam => pam.PlayerId == PlayerId)).ToList();
				Spawnsets = _dbContext.SpawnsetFiles.Where(sf => sf.PlayerId == PlayerId).ToList();
			}
		}
	}
}
