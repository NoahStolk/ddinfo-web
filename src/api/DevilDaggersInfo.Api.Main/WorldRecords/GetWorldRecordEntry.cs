namespace DevilDaggersInfo.Api.Main.WorldRecords;

public record GetWorldRecordEntry
{
	public DateTime DateTime { get; init; }

	public int Id { get; init; }

	public required string Username { get; init; }

	public double Time { get; init; }

	public int Kills { get; init; }

	public int Gems { get; init; }

	public byte DeathType { get; init; }

	public int DaggersHit { get; init; }

	public int DaggersFired { get; init; }
}
