namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

public record GetCustomLeaderboardDdcl
{
	public string SpawnsetName { get; init; } = null!;

	public GetCustomLeaderboardDaggers? Daggers { get; init; }

	[Obsolete("Use Daggers instead.")]
	public int TimeBronze { get; init; }

	[Obsolete("Use Daggers instead.")]
	public int TimeSilver { get; init; }

	[Obsolete("Use Daggers instead.")]
	public int TimeGolden { get; init; }

	[Obsolete("Use Daggers instead.")]
	public int TimeDevil { get; init; }

	[Obsolete("Use Daggers instead.")]
	public int TimeLeviathan { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public bool IsAscending { get; init; }
}
