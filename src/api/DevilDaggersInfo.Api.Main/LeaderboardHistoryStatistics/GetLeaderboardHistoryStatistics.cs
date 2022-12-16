namespace DevilDaggersInfo.Api.Main.LeaderboardHistoryStatistics;

public record GetLeaderboardHistoryStatistics
{
	public required DateTime DateTime { get; init; }

	public required int TotalPlayers { get; init; }

	public required double TimeGlobal { get; init; }

	public required ulong KillsGlobal { get; init; }

	public required ulong GemsGlobal { get; init; }

	public required ulong DeathsGlobal { get; init; }

	public required ulong DaggersHitGlobal { get; init; }

	public required ulong DaggersFiredGlobal { get; init; }

	public required double Top1Entrance { get; init; }

	public required double Top2Entrance { get; init; }

	public required double Top3Entrance { get; init; }

	public required double Top10Entrance { get; init; }

	public required double Top100Entrance { get; init; }

	public required bool TotalPlayersUpdated { get; init; }

	public required bool TimeGlobalUpdated { get; init; }

	public required bool KillsGlobalUpdated { get; init; }

	public required bool GemsGlobalUpdated { get; init; }

	public required bool DeathsGlobalUpdated { get; init; }

	public required bool DaggersHitGlobalUpdated { get; init; }

	public required bool DaggersFiredGlobalUpdated { get; init; }

	public required bool Top1EntranceUpdated { get; init; }

	public required bool Top2EntranceUpdated { get; init; }

	public required bool Top3EntranceUpdated { get; init; }

	public required bool Top10EntranceUpdated { get; init; }

	public required bool Top100EntranceUpdated { get; init; }
}
