using DevilDaggersCore.Game;
using DevilDaggersWebsite.BlazorServer.Extensions;
using DevilDaggersWebsite.Entities;
using System;

namespace DevilDaggersWebsite.BlazorServer.Data
{
	public class CustomLeaderboardEntryData
	{
		public CustomLeaderboardEntryData(int rank, CustomEntry customEntry, CustomLeaderboard customLeaderboard)
		{
			DaggerCssClass = $"text-{customLeaderboard.GetDagger(customEntry.Time)}";

			Death? death = GameInfo.GetDeathByType(customEntry.DeathType, GameVersion.V3);
			DeathName = death?.Name ?? "Unknown";
			DeathHexColor = $"#{death?.ColorCode ?? "444"}";

			CustomEntryId = customEntry.Id;
			Rank = rank;
			PlayerId = customEntry.PlayerId;
			FlagCode = customEntry.Player?.CountryCode ?? string.Empty;
			Username = customEntry.Player?.PlayerName ?? "[Unknown player]";
			Time = customEntry.Time;
			Gems = customEntry.GemsCollected;
			Kills = customEntry.EnemiesKilled;
			DeathType = customEntry.DeathType;
			DaggersHit = customEntry.DaggersHit;
			DaggersFired = customEntry.DaggersFired;
			EnemiesAlive = customEntry.EnemiesAlive;
			Homing = customEntry.HomingDaggers;
			LevelUpTime2 = customEntry.LevelUpTime2;
			LevelUpTime3 = customEntry.LevelUpTime3;
			LevelUpTime4 = customEntry.LevelUpTime4;
			SubmitDateUtc = customEntry.SubmitDate.ToUniversalTime();
			ClientVersion = customEntry.ClientVersion ?? "Pre-0.2.5.0";
		}

		public string DaggerCssClass { get; }

		public string DeathName { get; }
		public string DeathHexColor { get; }

		public int CustomEntryId { get; set; }
		public int Rank { get; set; }
		public int PlayerId { get; set; }
		public string FlagCode { get; }
		public string Username { get; set; }
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
		public DateTime SubmitDateUtc { get; set; }
		public string ClientVersion { get; set; }

		public double Accuracy => DaggersFired == 0 ? 0 : DaggersHit / (double)DaggersFired;
	}
}
