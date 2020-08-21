using DevilDaggersWebsite.Code.Leaderboards;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Core.Clients;
using DevilDaggersWebsite.Core.Dto;
using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lb = DevilDaggersWebsite.Core.Dto.Leaderboard;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class IndexModel : PageModel, IDefaultLeaderboardPage
	{
		private readonly IWebHostEnvironment env;
		private readonly ApplicationDbContext dbContext;

		private string username = string.Empty;
		private int userId;

		public IndexModel(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			this.env = env;
			this.dbContext = dbContext;
		}

		[BindProperty]
		public Lb Leaderboard { get; set; } = new Lb();

		public int Rank { get; set; }

		public string Username
		{
			get => username;
			set
			{
				if (string.IsNullOrEmpty(value))
					return;
				username = value.Length > 16 ? value.Substring(0, 16) : value;
			}
		}

		public int UserId
		{
			get => userId;
			set
			{
				if (value <= 0)
					return;
				userId = value;
			}
		}

		public bool HasBans { get; private set; }
		public bool IsValidTop100Graph { get; private set; }
		public string UsernameAliases { get; private set; }
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
					Leaderboard = new Lb { Entries = new List<Entry> { await DdHasmodaiClient.GetUserById(UserId) } };
					break;
				case LeaderboardSearchType.Rank:
				default:
					Leaderboard = await DdHasmodaiClient.GetScores(Rank);
					if (Rank > Leaderboard.Players - 99)
					{
						Rank = Leaderboard.Players - 99;
						Leaderboard.Entries.Clear();
						Leaderboard = await DdHasmodaiClient.GetScores(Rank);
					}

					break;
			}

			IEnumerable<int> playerIds = Leaderboard.Entries.Select(p => p.Id);
			HasBans = dbContext.Players.Where(p => playerIds.Contains(p.Id)).Any(p => p.IsBanned);
			if (LeaderboardSearchType == LeaderboardSearchType.UserId)
			{
				Entry entry = Leaderboard.Entries[0];
				IsValidTop100Graph = UserId > 0 && entry.ExistsInHistory(env);
				IEnumerable<string> aliases = entry.GetAllUsernameAliases(env).Where(s => s != entry.Username);
				UsernameAliases = aliases.Any() ? $" (also known as: {string.Join(", ", aliases)})" : string.Empty;
			}
		}
	}
}