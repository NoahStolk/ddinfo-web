using DevilDaggersCore.CustomLeaderboards;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the list of all available custom leaderboards on the site.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetCustomLeaderboardsModel : ApiPageModel
	{
		private readonly ApplicationDbContext _context;

		public GetCustomLeaderboardsModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public FileResult OnGet(bool formatted = false)
        {
			List<CustomLeaderboardBase> leaderboards = new List<CustomLeaderboardBase>();
			foreach (CustomLeaderboard leaderboard in _context.CustomLeaderboards)
				leaderboards.Add(new CustomLeaderboardBase(
					leaderboard.SpawnsetFileName,
					leaderboard.Bronze,
					leaderboard.Silver,
					leaderboard.Golden,
					leaderboard.Devil,
					leaderboard.Homing == 0 ? 0 :
					_context.CustomEntries
						.Where(e => e.CustomLeaderboard == leaderboard)
						.Any(e => e.Time > leaderboard.Homing) ? leaderboard.Homing : -1,
					leaderboard.Category,
					leaderboard.DateLastPlayed,
					leaderboard.DateCreated));

			return JsonFile(leaderboards, formatted ? Formatting.Indented : Formatting.None);
        }
    }
}