using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Extensions;
using DevilDaggersWebsite.Code.External;
using DevilDaggersWebsite.Code.Leaderboards;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Users;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lb = DevilDaggersCore.Leaderboards.Leaderboard;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class IndexModel : PageModel, IDefaultLeaderboardPage
	{
		private readonly IWebHostEnvironment env;

		[BindProperty]
		public Lb Leaderboard { get; set; } = new Lb();

		public int Rank { get; set; }

		private string username = string.Empty;
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

		private int userId = 0;
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

		public IndexModel(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public async Task OnGetAsync(int rank, string username, int userId, bool showMoreStats)
		{
			Rank = Math.Max(rank, 1);
			Username = username;
			UserId = userId;
			ShowMoreStats = showMoreStats;

			switch (LeaderboardSearchType)
			{
				case LeaderboardSearchType.Username:
					Leaderboard = await HasmodaiUtils.GetUserSearch(Username);
					break;
				case LeaderboardSearchType.UserId:
					Leaderboard = new Lb { Entries = new List<Entry> { await HasmodaiUtils.GetUserById(UserId) } };
					break;
				case LeaderboardSearchType.Rank:
				default:
					Leaderboard = await HasmodaiUtils.GetScores(Rank);
					if (Rank > Leaderboard.Players - 99)
					{
						Rank = Leaderboard.Players - 99;
						Leaderboard.Entries.Clear();
						Leaderboard = await HasmodaiUtils.GetScores(Rank);
					}
					break;
			}

			HasBans = UserUtils.GetUserObjects<Ban>(env).Any(b => Leaderboard.Entries.Any(e => e.Id == b.Id));
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