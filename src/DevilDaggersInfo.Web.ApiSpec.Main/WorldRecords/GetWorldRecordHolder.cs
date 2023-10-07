namespace DevilDaggersInfo.Web.ApiSpec.Main.WorldRecords;

public record GetWorldRecordHolder
{
	public required int Id { get; init; }

	public required List<string> Usernames { get; init; }

	public required TimeSpan TotalTimeHeld { get; init; }

	public required TimeSpan LongestTimeHeldConsecutively { get; init; }

	public required int WorldRecordCount { get; init; }

	public required DateTime FirstHeld { get; init; }

	public required DateTime LastHeld { get; init; }

	public required string MostRecentUsername { get; init; }
}
