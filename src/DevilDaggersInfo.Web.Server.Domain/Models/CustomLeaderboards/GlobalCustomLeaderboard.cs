namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public sealed record GlobalCustomLeaderboard
{
	public required List<GlobalCustomLeaderboardEntry> Entries { get; init; }

	public required int TotalLeaderboards { get; init; }

	public required int TotalPoints { get; init; }
}
