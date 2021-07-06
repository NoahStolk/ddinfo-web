using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class PlayerSettingsModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public PlayerSettingsModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public List<Entry>? Entries { get; private set; }

		public List<Player> Players { get; private set; } = new();

		public async Task OnGetAsync()
		{
			List<Player> playersWithVisibleSettings = _dbContext.Players
				.AsNoTracking()
				.Include(p => p.PlayerTitles)
					.ThenInclude(pt => pt.Title)
				.Where(p => !p.HideSettings)
				.ToList();

			Players = playersWithVisibleSettings.Where(p => p.HasSettings()).ToList();

			Entries = await LeaderboardClient.Instance.GetUsersByIds(Players.Select(p => p.Id));
			if (Entries != null)
				Entries = Entries.OrderBy(e => e.Rank).ToList();
		}
	}
}
