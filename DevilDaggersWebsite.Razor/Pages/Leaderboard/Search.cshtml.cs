using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Razor.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lb = DevilDaggersWebsite.Clients.Leaderboard;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class SearchModel : PageModel, IDefaultLeaderboardPageModel
	{
		private string _username = string.Empty;

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

		[BindProperty]
		public Lb? Leaderboard { get; set; }

		public async Task OnGetAsync(string username)
		{
			Username = username;

			List<Entry>? entries = await LeaderboardClient.Instance.GetUserSearch(Username);
			Leaderboard = entries == null ? null : new() { Entries = entries.OrderBy(e => e.Rank).ToList() };
		}
	}
}
