using DevilDaggersWebsite.Entities;

namespace DevilDaggersWebsite.BlazorServer.Extensions
{
	public static class EntityExtensions
	{
		/// <summary>
		/// Returns the CSS class name corresponding to the time in seconds.
		/// </summary>
		/// <param name="customLeaderboard">The custom leaderboard.</param>
		/// <param name="time">The time in tenths of milliseconds.</param>
		/// <returns>The CSS class name for the dagger.</returns>
		public static string GetDagger(this CustomLeaderboard customLeaderboard, int time)
		{
			if (customLeaderboard.IsAscending())
			{
				if (time <= customLeaderboard.TimeLeviathan && customLeaderboard.TimeLeviathan > 0)
					return "leviathan";
				if (time <= customLeaderboard.TimeDevil && customLeaderboard.TimeDevil > 0)
					return "devil";
				if (time <= customLeaderboard.TimeGolden && customLeaderboard.TimeGolden > 0)
					return "golden";
				if (time <= customLeaderboard.TimeSilver && customLeaderboard.TimeSilver > 0)
					return "silver";
				if (time <= customLeaderboard.TimeBronze && customLeaderboard.TimeBronze > 0)
					return "bronze";
				return "default";
			}

			if (time >= customLeaderboard.TimeLeviathan && customLeaderboard.TimeLeviathan > 0)
				return "leviathan";
			if (time >= customLeaderboard.TimeDevil && customLeaderboard.TimeDevil > 0)
				return "devil";
			if (time >= customLeaderboard.TimeGolden && customLeaderboard.TimeGolden > 0)
				return "golden";
			if (time >= customLeaderboard.TimeSilver && customLeaderboard.TimeSilver > 0)
				return "silver";
			if (time >= customLeaderboard.TimeBronze && customLeaderboard.TimeBronze > 0)
				return "bronze";
			return "default";
		}
	}
}
