namespace DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardHistory;

public record GetEntryHistory : IGetEntryDto
{
	public required DateTime DateTime { get; init; }

	public required int Rank { get; init; }

	public required int Id { get; init; }

	public required string Username { get; init; }

	public required double Time { get; init; }

	public required int Kills { get; init; }

	public required int Gems { get; init; }

	public required byte DeathType { get; init; }

	public required int DaggersHit { get; init; }

	public required int DaggersFired { get; init; }

	public required double TimeTotal { get; init; }

	public required ulong KillsTotal { get; init; }

	public required ulong GemsTotal { get; init; }

	public required ulong DeathsTotal { get; init; }

	public required ulong DaggersHitTotal { get; init; }

	public required ulong DaggersFiredTotal { get; init; }
}
