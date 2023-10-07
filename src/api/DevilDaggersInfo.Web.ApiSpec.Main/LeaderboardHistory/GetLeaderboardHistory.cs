namespace DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardHistory;

public record GetLeaderboardHistory : IGetLeaderboardGlobalDto
{
	public required DateTime DateTime { get; init; }

	public required int TotalPlayers { get; init; }

	public required double TimeGlobal { get; init; }

	public required ulong KillsGlobal { get; init; }

	public required ulong GemsGlobal { get; init; }

	public required ulong DeathsGlobal { get; init; }

	public required ulong DaggersHitGlobal { get; init; }

	public required ulong DaggersFiredGlobal { get; init; }

	public required List<GetEntryHistory> Entries { get; init; }
}
