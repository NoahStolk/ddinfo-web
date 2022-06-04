using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;

public class CustomLeaderboardSummary
{
	public int Id { get; init; }

	public int SpawnsetId { get; init; }

	public string SpawnsetName { get; init; } = null!;

	public CustomLeaderboardCategory Category { get; init; }

	public CustomLeaderboardDaggers? Daggers { get; init; }
}
