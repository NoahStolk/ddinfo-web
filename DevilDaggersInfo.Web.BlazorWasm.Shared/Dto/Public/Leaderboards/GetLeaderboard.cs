namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Leaderboards;

public class GetLeaderboard : IGetLeaderboardGlobalDto
{
	public DateTime DateTime { get; init; }

	public int TotalPlayers { get; init; }

	public double TimeGlobal { get; init; }

	public ulong KillsGlobal { get; init; }

	public ulong GemsGlobal { get; init; }

	public ulong DeathsGlobal { get; init; }

	public ulong DaggersHitGlobal { get; init; }

	public ulong DaggersFiredGlobal { get; init; }

	public List<GetEntry> Entries { get; set; } = new();
}
