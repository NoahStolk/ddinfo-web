using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class StatisticsModel : PageModel
	{
		public static Dictionary<Enemy[], (int? Time, Dagger Dagger)> EnemyTimes { get; } = new()
		{
			{ new[] { GameInfo.V2Andras }, (null, GameInfo.V31LeviathanDagger) },
			{ new[] { GameInfo.V31Thorn }, (4470000, GameInfo.V31LeviathanDagger) },
			{ new[] { GameInfo.V31Ghostpede }, (4420000, GameInfo.V31LeviathanDagger) },
			{ new[] { GameInfo.V31Leviathan }, (3500000, GameInfo.V31Devil) },
			{ new[] { GameInfo.V31Spider2 }, (2740000, GameInfo.V31Golden) },
			{ new[] { GameInfo.V31Gigapede }, (2590000, GameInfo.V31Golden) },
			{ new[] { GameInfo.V31Squid3 }, (2440000, GameInfo.V31Golden) },
			{ new[] { GameInfo.V31Centipede }, (1140000, GameInfo.V31Silver) },
			{ new[] { GameInfo.V31Squid2, GameInfo.V31Spider1 }, (390000, GameInfo.V31Bronze) },
			{ new[] { GameInfo.V31Squid1 }, (30000, GameInfo.V31Default) },
		};
	}
}
