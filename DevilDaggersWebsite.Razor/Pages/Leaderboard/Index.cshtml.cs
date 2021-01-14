using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Leaderboards;
using DevilDaggersWebsite.Razor.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lb = DevilDaggersWebsite.Dto.Leaderboard;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class IndexModel : PageModel, IDefaultLeaderboardPage
	{
		private readonly IWebHostEnvironment _env;
		private readonly ApplicationDbContext _dbContext;

		private string _username = string.Empty;
		private int _userId;

		public IndexModel(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			_env = env;
			_dbContext = dbContext;
		}

		[BindProperty]
		public Lb? Leaderboard { get; set; }

		public int Rank { get; set; }

		public string Username
		{
			get => _username;
			set
			{
				if (string.IsNullOrEmpty(value))
					return;
				_username = value.Length > 16 ? value.Substring(0, 16) : value;
			}
		}

		public int UserId
		{
			get => _userId;
			set
			{
				if (value <= 0)
					return;
				_userId = value;
			}
		}

		public bool HasBans { get; private set; }
		public bool IsValidTop100Graph { get; private set; }
		public string? UsernameAliases { get; private set; }
		public bool ShowMoreStats { get; private set; }

		public LeaderboardSearchType LeaderboardSearchType => !string.IsNullOrEmpty(Username) && Username.Length >= 3 ? LeaderboardSearchType.Username : UserId != 0 ? LeaderboardSearchType.UserId : LeaderboardSearchType.Rank;

		public async Task OnGetAsync(int rank, string username, int userId, bool showMoreStats)
		{
			Rank = Math.Max(rank, 1);
			Username = username;
			UserId = userId;
			ShowMoreStats = showMoreStats;

			switch (LeaderboardSearchType)
			{
				case LeaderboardSearchType.Username:
					Leaderboard = await DdHasmodaiClient.GetUserSearch(Username);
					break;
				case LeaderboardSearchType.UserId:
					Entry? entry = await DdHasmodaiClient.GetUserById(UserId);
					if (entry != null)
						Leaderboard = new Lb { Entries = new List<Entry> { entry } };
					else
						Leaderboard = null;
					break;
				case LeaderboardSearchType.Rank:
				default:
					Leaderboard = await DdHasmodaiClient.GetScores(Rank);
					if (Leaderboard != null && Rank > Leaderboard.Players - 99)
					{
						Rank = Leaderboard.Players - 99;
						Leaderboard.Entries.Clear();
						Leaderboard = await DdHasmodaiClient.GetScores(Rank);
					}

					break;
			}

			if (Leaderboard != null)
			{
				IEnumerable<int> playerIds = Leaderboard.Entries.Select(p => p.Id);
				HasBans = _dbContext.Players.Where(p => playerIds.Contains(p.Id)).Any(p => p.IsBanned);
				if (LeaderboardSearchType == LeaderboardSearchType.UserId)
				{
					Entry entry = Leaderboard.Entries[0];
					IsValidTop100Graph = UserId > 0 && entry.ExistsInHistory(_env);
					IEnumerable<string> aliases = entry.GetAllUsernameAliases(_env).Where(s => s != entry.Username);
					UsernameAliases = aliases.Any() ? $" (also known as: {string.Join(", ", aliases)})" : string.Empty;
				}
			}
		}
	}
}
