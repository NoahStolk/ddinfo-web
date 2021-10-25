namespace DevilDaggersWebsite.Dto
{
	public class SpawnsetForDdcl
	{
		public int SpawnsetId { get; init; }

		public string Name { get; init; } = null!;

		public string AuthorName { get; init; } = null!;

		public SpawnsetCustomLeaderboardForDdcl? CustomLeaderboard { get; init; }
	}
}
