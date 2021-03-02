using DevilDaggersWebsite.Enumerators;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminCustomLeaderboard
	{
		public CustomLeaderboardCategory Category { get; init; }
		public int SpawnsetFileId { get; init; }

		public int TimeBronze { get; init; }
		public int TimeSilver { get; init; }
		public int TimeGolden { get; init; }
		public int TimeDevil { get; init; }
		public int TimeLeviathan { get; init; }
	}
}
