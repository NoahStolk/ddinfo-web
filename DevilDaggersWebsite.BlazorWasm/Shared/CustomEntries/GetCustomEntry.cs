using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.CustomEntries
{
	public class GetCustomEntry : IGetDto<int>
	{
		public int Id { get; init; }

		public string SpawnsetName { get; init; } = null!;

		public string PlayerName { get; init; } = null!;

		public float Time { get; init; }

		public int GemsCollected { get; init; }

		public int GemsDespawned { get; init; }

		public int GemsEaten { get; init; }

		public int GemsTotal { get; init; }

		public int EnemiesKilled { get; init; }

		public int EnemiesAlive { get; init; }

		public int DaggersFired { get; init; }

		public int DaggersHit { get; init; }

		public int HomingDaggers { get; init; }

		public int HomingDaggersEaten { get; init; }

		public byte DeathType { get; init; }

		public float LevelUpTime2 { get; init; }

		public float LevelUpTime3 { get; init; }

		public float LevelUpTime4 { get; init; }

		public DateTime SubmitDate { get; init; }

		public string ClientVersion { get; init; } = null!;
	}
}
