using CoreBase3.Services;
using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.Leaderboards;
using DevilDaggersWebsite.Code.Utils;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lb = DevilDaggersCore.Leaderboards.Leaderboard;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class IndexModel : PageModel
	{
		private readonly ICommonObjects commonObjects;

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

		public LeaderboardSearchType LeaderboardSearchType => !string.IsNullOrEmpty(Username) && Username.Length >= 3 ? LeaderboardSearchType.Username : UserId != 0 ? LeaderboardSearchType.UserId : LeaderboardSearchType.Rank;

		public IndexModel(ICommonObjects commonObjects)
		{
			this.commonObjects = commonObjects;
		}

		public async Task OnGetAsync(int rank, string username, int userId)
		{
			try
			{
				Rank = Math.Max(rank, 1);
				Username = username;
				UserId = userId;

				switch (LeaderboardSearchType)
				{
					case LeaderboardSearchType.Username:
						Leaderboard = await Hasmodai.GetUserSearch(Username);
						break;
					case LeaderboardSearchType.UserId:
						Leaderboard = new Lb { Entries = new List<Entry> { await Hasmodai.GetUserById(UserId) } };
						break;
					case LeaderboardSearchType.Rank:
					default:
						Leaderboard = await Hasmodai.GetScores(Rank);
						if (Rank > Leaderboard.Players - 99)
						{
							Rank = Leaderboard.Players - 99;
							Leaderboard.Entries.Clear();
							Leaderboard = await Hasmodai.GetScores(Rank);
						}
						break;
				}

				HasBans = UserUtils.GetBans(commonObjects).Any(b => Leaderboard.Entries.Any(e => e.Id == b.Id));
			}
			catch
			{
			}
		}
	}
}