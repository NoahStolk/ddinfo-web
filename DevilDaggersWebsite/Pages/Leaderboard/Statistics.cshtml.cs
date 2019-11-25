using DevilDaggersWebsite.Code.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class StatisticsModel : PageModel
	{
		public RetrieveEntireLeaderboardTask Task => (RetrieveEntireLeaderboardTask)TaskInstanceKeeper.Instances[typeof(RetrieveEntireLeaderboardTask)];
	}
}