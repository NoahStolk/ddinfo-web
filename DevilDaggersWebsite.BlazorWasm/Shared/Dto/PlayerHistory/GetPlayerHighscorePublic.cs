using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.PlayerHistory
{
	public class GetPlayerHighscorePublic
	{
		public DateTime DateTime { get; init; }

		public int Rank { get; init; }

		public double Time { get; init; }

		public string Username { get; init; } = null!;

		public int Kills { get; init; }

		public int Gems { get; init; }

		public DeathType DeathType { get; init; }

		public int DaggersHit { get; init; }

		public int DaggersFired { get; init; }
	}
}
