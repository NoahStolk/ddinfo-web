using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardSummary
{
	public required int Id { get; init; }

	public required int SpawnsetId { get; init; }

	public required string SpawnsetName { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }

	public required CustomLeaderboardDaggers? Daggers { get; init; }
}
