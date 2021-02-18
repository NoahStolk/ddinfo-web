using DevilDaggersCore.Game;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Html;
using System.Web;

namespace DevilDaggersWebsite.Razor.Extensions
{
	public static class EntityExtensions
	{
		public static HtmlString ToHtmlData(this CustomEntry customEntry, int rank, string username, string flagCode)
		{
			return new($@"
rank='{rank}'
flag='{flagCode}'
username='{HttpUtility.HtmlEncode(username)}'
time='{customEntry.Time}'
kills='{customEntry.EnemiesKilled}'
gems='{customEntry.GemsCollected}'
accuracy='{customEntry.Accuracy * 10000:0}'
death-type='{GameInfo.GetDeathByType(customEntry.DeathType, GameVersion.V3)}'
enemies-alive='{customEntry.EnemiesAlive}'
homing='{customEntry.HomingDaggers}'
level-2='{(customEntry.LevelUpTime2 == 0 ? 999999999 : customEntry.LevelUpTime2)}'
level-3='{(customEntry.LevelUpTime3 == 0 ? 999999999 : customEntry.LevelUpTime3)}'
level-4='{(customEntry.LevelUpTime4 == 0 ? 999999999 : customEntry.LevelUpTime4)}'
submit-date='{customEntry.SubmitDate:yyyyMMddHHmm}'");
		}

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
