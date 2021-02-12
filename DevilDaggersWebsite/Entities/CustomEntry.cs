using DevilDaggersCore.Game;
using Microsoft.AspNetCore.Html;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DevilDaggersWebsite.Entities
{
	public class CustomEntry
	{
		[Key]
		public int Id { get; set; }

		public int CustomLeaderboardId { get; set; }

		[ForeignKey(nameof(CustomLeaderboardId))]
		public CustomLeaderboard CustomLeaderboard { get; set; }

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; }

		public int Time { get; set; }
		public int Gems { get; set; }
		public int Kills { get; set; }
		public int DeathType { get; set; }
		public int DaggersHit { get; set; }
		public int DaggersFired { get; set; }
		public int EnemiesAlive { get; set; }
		public int Homing { get; set; }
		public int LevelUpTime2 { get; set; }
		public int LevelUpTime3 { get; set; }
		public int LevelUpTime4 { get; set; }
		public DateTime SubmitDate { get; set; }
		public string? ClientVersion { get; set; }

		public string? GemsData { get; set; }
		public string? KillsData { get; set; }
		public string? HomingData { get; set; }
		public string? EnemiesAliveData { get; set; }
		public string? DaggersFiredData { get; set; }
		public string? DaggersHitData { get; set; }

		public double Accuracy => DaggersFired == 0 ? 0 : DaggersHit / (double)DaggersFired;

		public HtmlString ToHtmlData(int rank, string username, string flagCode) => new HtmlString($@"
			rank='{rank}'
			flag='{flagCode}'
			username='{HttpUtility.HtmlEncode(username)}'
			time='{Time}'
			kills='{Kills}'
			gems='{Gems}'
			accuracy='{Accuracy * 10000:0}'
			death-type='{GameInfo.GetDeathByType(DeathType, GameVersion.V3)}'
			enemies-alive='{EnemiesAlive}'
			homing='{Homing}'
			level-2='{(LevelUpTime2 == 0 ? 999999999 : LevelUpTime2)}'
			level-3='{(LevelUpTime3 == 0 ? 999999999 : LevelUpTime3)}'
			level-4='{(LevelUpTime4 == 0 ? 999999999 : LevelUpTime4)}'
			submit-date='{SubmitDate:yyyyMMddHHmm}'
		");
	}
}
