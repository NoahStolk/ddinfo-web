namespace DevilDaggersInfo.Api.Main.WorldRecords;

public record GetWorldRecordHolder
{
	public int Id { get; init; }

	public List<string> Usernames { get; init; } = null!;

	public TimeSpan TotalTimeHeld { get; init; }

	public TimeSpan LongestTimeHeldConsecutively { get; init; }

	public int WorldRecordCount { get; init; }

	public DateTime FirstHeld { get; init; }

	public DateTime LastHeld { get; init; }

	public string MostRecentUsername { get; init; } = null!;
}
