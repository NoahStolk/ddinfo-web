using DevilDaggersWebsite.Dto.Admin;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class CustomEntry : IAdminUpdatableEntity<AdminCustomEntry>
	{
		[Key]
		public int Id { get; set; }

		public int CustomLeaderboardId { get; set; }

		[ForeignKey(nameof(CustomLeaderboardId))]
		public CustomLeaderboard CustomLeaderboard { get; set; } = null!;

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; } = null!;

		public int Time { get; set; }
		public int GemsCollected { get; set; }
		public int EnemiesKilled { get; set; }
		public int DaggersFired { get; set; }
		public int DaggersHit { get; set; }
		public int EnemiesAlive { get; set; }
		public int HomingDaggers { get; set; }
		public int GemsDespawned { get; set; }
		public int GemsEaten { get; set; }
		public int GemsTotal { get; set; }
		public byte DeathType { get; set; }
		public int LevelUpTime2 { get; set; }
		public int LevelUpTime3 { get; set; }
		public int LevelUpTime4 { get; set; }
		public DateTime SubmitDate { get; set; }
		public string ClientVersion { get; set; } = null!;

		public double Accuracy => DaggersFired == 0 ? 0 : DaggersHit / (double)DaggersFired;

		public void Create(ApplicationDbContext dbContext, AdminCustomEntry adminDto)
		{
			CustomLeaderboardId = adminDto.CustomLeaderboardId;
			PlayerId = adminDto.PlayerId;

			Edit(dbContext, adminDto);

			dbContext.CustomEntries.Add(this);
		}

		public void Edit(ApplicationDbContext dbContext, AdminCustomEntry adminDto)
		{
			Time = adminDto.Time;
			GemsCollected = adminDto.GemsCollected;
			EnemiesKilled = adminDto.EnemiesKilled;
			DaggersFired = adminDto.DaggersFired;
			DaggersHit = adminDto.DaggersHit;
			EnemiesAlive = adminDto.EnemiesAlive;
			HomingDaggers = adminDto.HomingDaggers;
			GemsDespawned = adminDto.GemsDespawned;
			GemsEaten = adminDto.GemsEaten;
			GemsTotal = adminDto.GemsTotal;
			DeathType = adminDto.DeathType;
			LevelUpTime2 = adminDto.LevelUpTime2;
			LevelUpTime3 = adminDto.LevelUpTime3;
			LevelUpTime4 = adminDto.LevelUpTime4;
			SubmitDate = adminDto.SubmitDate;
			ClientVersion = adminDto.ClientVersion;
		}

		public AdminCustomEntry Populate()
		{
			return new()
			{
				ClientVersion = ClientVersion,
				CustomLeaderboardId = CustomLeaderboardId,
				DaggersFired = DaggersFired,
				DaggersHit = DaggersHit,
				DeathType = DeathType,
				EnemiesAlive = EnemiesAlive,
				EnemiesKilled = EnemiesKilled,
				GemsCollected = GemsCollected,
				GemsDespawned = GemsDespawned,
				GemsEaten = GemsEaten,
				GemsTotal = GemsTotal,
				HomingDaggers = HomingDaggers,
				LevelUpTime2 = LevelUpTime2,
				LevelUpTime3 = LevelUpTime3,
				LevelUpTime4 = LevelUpTime4,
				PlayerId = PlayerId,
				SubmitDate = SubmitDate,
				Time = Time,
			};
		}
	}
}
