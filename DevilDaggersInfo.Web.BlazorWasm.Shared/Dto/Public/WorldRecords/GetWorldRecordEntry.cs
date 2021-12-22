namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.WorldRecords;

public class GetWorldRecordEntry
{
	public DateTime DateTime { get; init; }

	public int Id { get; init; }

	public string Username { get; init; } = null!;

	public double Time { get; init; }

	public int Kills { get; init; }

	public int Gems { get; init; }

	public byte DeathType { get; init; }

	public int DaggersHit { get; init; }

	public int DaggersFired { get; init; }
}
