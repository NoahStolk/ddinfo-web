namespace DevilDaggersInfo.Web.ApiSpec.Main.Players;

public record GetPlayerHistoryActivityEntry
{
	public required DateTime DateTime { get; init; }

	public required double DeathsIncrement { get; init; }

	public required double TimeIncrement { get; init; }
}
