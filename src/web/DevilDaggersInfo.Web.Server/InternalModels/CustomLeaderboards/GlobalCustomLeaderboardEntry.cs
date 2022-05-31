namespace DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;

public class GlobalCustomLeaderboardEntry
{
	public int PlayerId { get; init; }

	public string PlayerName { get; init; } = null!;

	public int Points { get; set; }

	public int LeviathanDaggerCount { get; set; }

	public int DevilDaggerCount { get; set; }

	public int GoldenDaggerCount { get; set; }

	public int SilverDaggerCount { get; set; }

	public int BronzeDaggerCount { get; set; }

	public int DefaultDaggerCount { get; set; }

	public int LeaderboardsPlayedCount { get; set; }
}
