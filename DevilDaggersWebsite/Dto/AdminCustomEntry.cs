using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Dto
{
	public class AdminCustomEntry : IAdminDto
	{
		public int CustomLeaderboardId { get; init; }

		public int PlayerId { get; init; }

		public int Time { get; init; }

		public int GemsCollected { get; init; }

		public int EnemiesKilled { get; init; }

		public int DaggersFired { get; init; }

		public int DaggersHit { get; init; }

		public int EnemiesAlive { get; init; }

		public int HomingDaggers { get; init; }

		public int HomingDaggersEaten { get; init; }

		public int GemsDespawned { get; init; }

		public int GemsEaten { get; init; }

		public int GemsTotal { get; init; }

		public byte DeathType { get; init; }

		public int LevelUpTime2 { get; init; }

		public int LevelUpTime3 { get; init; }

		public int LevelUpTime4 { get; init; }

		public DateTime SubmitDate { get; init; }

		[StringLength(16)]
		public string ClientVersion { get; init; } = null!;

		public Dictionary<string, string> Log()
		{
			Dictionary<string, string> dictionary = new();
			dictionary.Add(nameof(CustomLeaderboardId), CustomLeaderboardId.ToString());
			dictionary.Add(nameof(PlayerId), PlayerId.ToString());
			dictionary.Add(nameof(Time), Time.ToString());
			dictionary.Add(nameof(GemsCollected), GemsCollected.ToString());
			dictionary.Add(nameof(EnemiesKilled), EnemiesKilled.ToString());
			dictionary.Add(nameof(DaggersFired), DaggersFired.ToString());
			dictionary.Add(nameof(DaggersHit), DaggersHit.ToString());
			dictionary.Add(nameof(EnemiesAlive), EnemiesAlive.ToString());
			dictionary.Add(nameof(HomingDaggers), HomingDaggers.ToString());
			dictionary.Add(nameof(HomingDaggersEaten), HomingDaggersEaten.ToString());
			dictionary.Add(nameof(GemsDespawned), GemsDespawned.ToString());
			dictionary.Add(nameof(GemsEaten), GemsEaten.ToString());
			dictionary.Add(nameof(GemsTotal), GemsTotal.ToString());
			dictionary.Add(nameof(DeathType), DeathType.ToString());
			dictionary.Add(nameof(LevelUpTime2), LevelUpTime2.ToString());
			dictionary.Add(nameof(LevelUpTime3), LevelUpTime3.ToString());
			dictionary.Add(nameof(LevelUpTime4), LevelUpTime4.ToString());
			dictionary.Add(nameof(SubmitDate), SubmitDate.ToString("dd MMM yyyy"));
			dictionary.Add(nameof(ClientVersion), ClientVersion);
			return dictionary;
		}
	}
}
