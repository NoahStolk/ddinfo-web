using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class StatisticsModel : PageModel
	{
		public static Dictionary<Enemy, (int Time, string DaggerColorCode)> EnemyTimes { get; } = new()
		{
			{ GameInfo.V2Andras, (20000000, GameInfo.V2Andras.ColorCode) },
			{ GameInfo.V31Thorn, (4470000, GameInfo.V31LeviathanDagger.ColorCode) },
			{ GameInfo.V31Ghostpede, (4420000, GameInfo.V31LeviathanDagger.ColorCode) },
			{ GameInfo.V31Leviathan, (3500000, GameInfo.V31Devil.ColorCode) },
			{ GameInfo.V31Spider2, (2740000, GameInfo.V31Golden.ColorCode) },
			{ GameInfo.V31Gigapede, (2590000, GameInfo.V31Golden.ColorCode) },
			{ GameInfo.V31Squid3, (2440000, GameInfo.V31Golden.ColorCode) },
			{ GameInfo.V31Centipede, (1140000, GameInfo.V31Silver.ColorCode) },
			{ GameInfo.V31Squid2, (390000, GameInfo.V31Bronze.ColorCode) },
			{ GameInfo.V31Squid1, (30000, GameInfo.V31Default.ColorCode) },
		};
	}
}
