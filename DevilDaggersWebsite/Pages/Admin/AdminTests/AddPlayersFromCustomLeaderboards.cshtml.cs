using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.DataTransferObjects;
using DevilDaggersWebsite.Code.External;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Admin.AdminTests
{
	public class AddPlayersFromCustomLeaderboardsModel : PageModel
	{
		private readonly ApplicationDbContext dbContext;

		public AddPlayersFromCustomLeaderboardsModel(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task OnGetAsync()
		{
			List<int> playerIds = dbContext.Players.Select(p => p.Id).ToList();
			List<int> playerIdsCustom = dbContext.CustomEntries.Select(ce => ce.PlayerId).Distinct().ToList();
			List<int> remainingIds = playerIdsCustom.Where(i => !playerIds.Contains(i)).ToList();

			foreach (int remaining in remainingIds)
			{
				Entry entry = await HasmodaiUtils.GetUserById(remaining);

				dbContext.Players.Add(new Player
				{
					Id = remaining,
					Username = entry.Username,
				});
			}

			dbContext.SaveChanges();
		}
	}
}