using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardsTotalData
{
	public required Dictionary<SpawnsetGameMode, int> LeaderboardsPerGameMode { get; init; }

	public required Dictionary<SpawnsetGameMode, int> ScoresPerGameMode { get; init; }

	public required Dictionary<SpawnsetGameMode, int> SubmitsPerGameMode { get; init; }

	public required Dictionary<SpawnsetGameMode, int> PlayersPerGameMode { get; init; }

	public required int TotalPlayers { get; init; }
}
