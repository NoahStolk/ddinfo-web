using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class CustomLeaderboardSummary
{
	public int Id { get; init; }

	public int SpawnsetId { get; init; }

	public required string SpawnsetName { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public CustomLeaderboardDaggers? Daggers { get; init; }
}
