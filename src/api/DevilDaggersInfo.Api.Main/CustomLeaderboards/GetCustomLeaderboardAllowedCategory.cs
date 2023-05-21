using DevilDaggersInfo.Api.Main.Spawnsets;

namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetCustomLeaderboardAllowedCategory
{
	public required GameMode GameMode { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }
}
