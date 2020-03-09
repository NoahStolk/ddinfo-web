using DevilDaggersCore.CustomLeaderboards;
using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Html;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DevilDaggersWebsite.Code.Database.CustomLeaderboards
{
	public class CustomEntry : CustomEntryBase
	{
		public int Id { get; set; }

		public int CustomLeaderboardId { get; set; }

		[ForeignKey("CustomLeaderboardId")]
		public CustomLeaderboard CustomLeaderboard { get; set; }

		public CustomEntry(int playerId, string username, float time, int gems, int kills, int deathType, int shotsHit, int shotsFired, int enemiesAlive, int homing, float levelUpTime2, float levelUpTime3, float levelUpTime4, DateTime submitDate, string clientVersion)
			: base(playerId, username, time, gems, kills, deathType, shotsHit, shotsFired, enemiesAlive, homing, levelUpTime2, levelUpTime3, levelUpTime4, submitDate, clientVersion)
		{
		}

		public HtmlString ToHTMLData(int rank, string flagCode)
		{
			return new HtmlString($@"
				rank='{rank}'
				flag='{flagCode}'
				username='{HttpUtility.HtmlEncode(Username)}'
				time='{(Time * 10000).ToString("0")}'
				kills='{Kills}'
				gems='{Gems}'
				accuracy='{(Accuracy * 10000).ToString("0")}'
				death-type='{GameInfo.GetDeathFromDeathType(DeathType).Name}'
				enemies-alive='{EnemiesAlive}'
				homing='{Homing}'
				level-2='{(LevelUpTime2 == 0 ? "999999999" : (LevelUpTime2 * 10000).ToString("0"))}'
				level-3='{(LevelUpTime3 == 0 ? "999999999" : (LevelUpTime3 * 10000).ToString("0"))}'
				level-4='{(LevelUpTime4 == 0 ? "999999999" : (LevelUpTime4 * 10000).ToString("0"))}'
				submit-date='{SubmitDate.ToString("yyyyMMddHHmm")}'
			");
		}
	}
}