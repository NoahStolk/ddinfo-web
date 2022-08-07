namespace DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardStatistics;

public class CompressedEntry
{
	public uint Time { get; init; }
	public ushort Kills { get; init; }
	public ushort Gems { get; init; }
	public ushort DaggersHit { get; init; }
	public uint DaggersFired { get; init; }
	public byte DeathType { get; init; }
}
