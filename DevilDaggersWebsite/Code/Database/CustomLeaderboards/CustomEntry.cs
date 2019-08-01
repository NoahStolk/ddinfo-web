using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Html;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DevilDaggersWebsite.Code.Database.CustomLeaderboards
{
	public class CustomEntry
	{
		public int ID { get; set; }
		public int PlayerID { get; set; }
		public string Username { get; set; }
		public float Time { get; set; }
		public int Gems { get; set; }
		public int Kills { get; set; }
		public int DeathType { get; set; }
		public int ShotsHit { get; set; }
		public int ShotsFired { get; set; }
		public int EnemiesAlive { get; set; }
		public int Homing { get; set; }
		public float LevelUpTime2 { get; set; }
		public float LevelUpTime3 { get; set; }
		public float LevelUpTime4 { get; set; }
		public DateTime SubmitDate { get; set; }
		public string ClientVersion { get; set; }

		public int CustomLeaderboardID { get; set; }

		[ForeignKey("CustomLeaderboardID")]
		public CustomLeaderboard CustomLeaderboard { get; set; }

		public double Accuracy => ShotsFired == 0 ? 0 : ShotsHit / (double)ShotsFired;

		public CustomEntry(int playerID, string username, float time, int gems, int kills, int deathType, int shotsHit, int shotsFired, int enemiesAlive, int homing, float levelUpTime2, float levelUpTime3, float levelUpTime4, DateTime submitDate, string clientVersion)
		{
			PlayerID = playerID;
			Username = username;
			Time = time;
			Gems = gems;
			Kills = kills;
			DeathType = deathType;
			ShotsHit = shotsHit;
			ShotsFired = shotsFired;
			EnemiesAlive = enemiesAlive;
			Homing = homing;
			LevelUpTime2 = levelUpTime2;
			LevelUpTime3 = levelUpTime3;
			LevelUpTime4 = levelUpTime4;
			SubmitDate = submitDate;
			ClientVersion = clientVersion;
		}

		public CustomEntry()
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