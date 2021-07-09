using DevilDaggersWebsite.Enumerators;
using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared.CustomLeaderboards
{
	public class GetCustomLeaderboard : IGetDto<int>
	{
		public int Id { get; init; }

		public string SpawnsetName { get; init; } = null!;

		public string SpawnsetAuthorName { get; init; } = null!;

		public float TimeBronze { get; init; }

		public float TimeSilver { get; init; }

		public float TimeGolden { get; init; }

		public float TimeDevil { get; init; }

		public float TimeLeviathan { get; init; }

		public DateTime? DateCreated { get; init; }

		public CustomLeaderboardCategory Category { get; init; }
	}
}
