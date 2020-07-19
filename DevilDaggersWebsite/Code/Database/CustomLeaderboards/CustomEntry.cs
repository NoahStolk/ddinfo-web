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

		[ForeignKey(nameof(CustomLeaderboardId))]
		public CustomLeaderboard CustomLeaderboard { get; set; }

		public CustomEntry(int playerId, string username, int time, int gems, int kills, int deathType, int shotsHit, int shotsFired, int enemiesAlive, int homing, int levelUpTime2, int levelUpTime3, int levelUpTime4, DateTime submitDate, string clientVersion)
			: base(playerId, username, time, gems, kills, deathType, shotsHit, shotsFired, enemiesAlive, homing, levelUpTime2, levelUpTime3, levelUpTime4, submitDate, clientVersion)
		{
		}

		public HtmlString ToHtmlData(int rank, string flagCode) => new HtmlString($@"
			rank='{rank}'
			flag='{flagCode}'
			username='{HttpUtility.HtmlEncode(Username)}'
			time='{Time}'
			kills='{Kills}'
			gems='{Gems}'
			accuracy='{Accuracy * 10000:0}'
			death-type='{GameInfo.GetDeathFromDeathType(DeathType).Name}'
			enemies-alive='{EnemiesAlive}'
			homing='{Homing}'
			level-2='{(LevelUpTime2 == 0 ? 999999999 : LevelUpTime2)}'
			level-3='{(LevelUpTime3 == 0 ? 999999999 : LevelUpTime3)}'
			level-4='{(LevelUpTime4 == 0 ? 999999999 : LevelUpTime4)}'
			submit-date='{SubmitDate:yyyyMMddHHmm}'
		");
	}
}