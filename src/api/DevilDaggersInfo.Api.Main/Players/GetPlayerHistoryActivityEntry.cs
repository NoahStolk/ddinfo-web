namespace DevilDaggersInfo.Api.Main.Players;

public record GetPlayerHistoryActivityEntry
{
	public DateTime DateTime { get; init; }

	public double DeathsIncrement { get; init; }

	public double TimeIncrement { get; init; }
}
