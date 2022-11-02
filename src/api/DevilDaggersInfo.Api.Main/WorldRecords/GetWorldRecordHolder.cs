namespace DevilDaggersInfo.Api.Main.WorldRecords;

public record GetWorldRecordHolder
{
	public int Id { get; init; }

	public required List<string> Usernames { get; init; }

	public TimeSpan TotalTimeHeld { get; init; }

	public TimeSpan LongestTimeHeldConsecutively { get; init; }

	public int WorldRecordCount { get; init; }

	public DateTime FirstHeld { get; init; }

	public DateTime LastHeld { get; init; }

	public required string MostRecentUsername { get; init; }
}
