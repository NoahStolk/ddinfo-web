using System;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.CustomEntries
{
	public class GetCustomEntry : IGetDto<int>
	{
		public int Id { get; init; }

		[Display(Name = "Spawnset")]
		public string SpawnsetName { get; init; } = null!;

		[Display(Name = "Player")]
		public string PlayerName { get; init; } = null!;

		public float Time { get; init; }

		[Display(Name = "Gems")]
		public int GemsCollected { get; init; }

		[Display(Name = "GemsDesp")]
		public int GemsDespawned { get; init; }

		public int GemsEaten { get; init; }

		public int GemsTotal { get; init; }

		[Display(Name = "Kills")]
		public int EnemiesKilled { get; init; }

		[Display(Name = "Alive")]
		public int EnemiesAlive { get; init; }

		[Display(Name = "Fired")]
		public int DaggersFired { get; init; }

		[Display(Name = "Hit")]
		public int DaggersHit { get; init; }

		[Display(Name = "Homing")]
		public int HomingDaggers { get; init; }

		[Display(Name = "HomingEaten")]
		public int HomingDaggersEaten { get; init; }

		[Display(Name = "Death")]
		public byte DeathType { get; init; }

		[Display(Name = "Level2")]
		public float LevelUpTime2 { get; init; }

		[Display(Name = "Level3")]
		public float LevelUpTime3 { get; init; }

		[Display(Name = "Level4")]
		public float LevelUpTime4 { get; init; }

		public DateTime SubmitDate { get; init; }

		[Display(Name = "Version")]
		public string ClientVersion { get; init; } = null!;
	}
}
