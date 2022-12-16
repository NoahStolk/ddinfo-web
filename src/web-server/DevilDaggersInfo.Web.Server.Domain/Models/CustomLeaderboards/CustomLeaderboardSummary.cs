using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardSummary
{
	public required int Id { get; init; }

	public required int SpawnsetId { get; init; }

	public required string SpawnsetName { get; init; }

	public required CustomLeaderboardCategory Category { get; init; }

	public required CustomLeaderboardDaggers? Daggers { get; init; }
}
