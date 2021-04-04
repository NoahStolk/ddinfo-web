using DevilDaggersCore.Game;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
		public string? CountryName { get; private set; }
		public int? BestRankRecorded { get; private set; }

		public bool HasValidTop100Graph { get; private set; }
		public List<string> UsernameAliases { get; private set; } = new();

		public async Task OnGetAsync(int id)
		{
			PlayerId = Math.Max(1, id);

			Player = _dbContext.Players.FirstOrDefault(p => p.Id == PlayerId);

			Entry = await LeaderboardClient.Instance.GetUserById(PlayerId);

			if (Entry != null)
			{
				Death = GameInfo.GetDeathByType(GameVersion.V31, Entry.DeathType);
				CountryName = Player?.CountryCode != null ? UserUtils.CountryNames.ContainsKey(Player.CountryCode) ? UserUtils.CountryNames[Player.CountryCode] : "Invalid country code" : null;
				HasValidTop100Graph = Entry.ExistsInHistory(_env);
				UsernameAliases = Entry.GetAllUsernameAliases(_env).Where(s => s != Entry.Username).ToList();
			}

			foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Lb lb = JsonConvert.DeserializeObject<Lb>(Io.File.ReadAllText(leaderboardHistoryPath));
				Entry? entry = lb.Entries.Find(e => e.Id == PlayerId);
				if (entry != null && (!BestRankRecorded.HasValue || BestRankRecorded.Value > entry.Rank))
					BestRankRecorded = entry.Rank;
			}
		}
	}
}
