namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

public class GetCustomLeaderboardStatisticsForPlayer
{
	public CustomLeaderboardCategory CustomLeaderboardCategory { get; init; }

	public int LeviathanDaggerCount { get; init; }

	public int DevilDaggerCount { get; init; }

	public int GoldenDaggerCount { get; init; }

	public int SilverDaggerCount { get; init; }

	public int BronzeDaggerCount { get; init; }

	public int DefaultDaggerCount { get; init; }

	public int LeaderboardsPlayedCount { get; init; }
}
