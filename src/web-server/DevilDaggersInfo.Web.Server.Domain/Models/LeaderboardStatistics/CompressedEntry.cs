namespace DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardStatistics;

public record CompressedEntry
{
	public required uint Time { get; init; }

	public required ushort Kills { get; init; }

	public required ushort Gems { get; init; }

	public required ushort DaggersHit { get; init; }

	public required uint DaggersFired { get; init; }

	public required byte DeathType { get; init; }
}
