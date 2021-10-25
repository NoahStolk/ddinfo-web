namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Leaderboards;

public class GetEntry : IGetEntryDto
{
	public int Rank { get; init; }

	public int Id { get; init; }

	public string Username { get; init; } = null!;

	public double Time { get; init; }

	public int Kills { get; init; }

	public int Gems { get; init; }

	// Note: cannot use DeathType enum here because of conflict with leaderboard history (which also implements this interface).
	public byte DeathType { get; init; }

	public int DaggersHit { get; init; }

	public int DaggersFired { get; init; }

	public double TimeTotal { get; init; }

	public ulong KillsTotal { get; init; }

	public ulong GemsTotal { get; init; }

	public ulong DeathsTotal { get; init; }

	public ulong DaggersHitTotal { get; init; }

	public ulong DaggersFiredTotal { get; init; }
}
