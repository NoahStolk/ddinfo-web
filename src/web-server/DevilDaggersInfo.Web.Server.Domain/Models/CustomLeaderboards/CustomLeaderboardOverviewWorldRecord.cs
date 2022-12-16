using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardOverviewWorldRecord
{
	public required int Time { get; init; }

	public required int PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public required CustomLeaderboardDagger? Dagger { get; init; }
}
