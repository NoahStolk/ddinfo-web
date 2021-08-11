using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.CustomEntries
{
	public class GetCustomEntry
	{
		public int Rank { get; init; }

		public int PlayerId { get; init; }

		public string PlayerName { get; init; } = null!;

		public string? CountryCode { get; init; }

		public double Time { get; init; }

		public CustomLeaderboardDagger CustomLeaderboardDagger { get; init; }

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

		public double LevelUpTime2 { get; init; }

		public double LevelUpTime3 { get; init; }

		public double LevelUpTime4 { get; init; }

		public DateTime SubmitDate { get; init; }

		public string? ClientVersion { get; init; }
	}
}
