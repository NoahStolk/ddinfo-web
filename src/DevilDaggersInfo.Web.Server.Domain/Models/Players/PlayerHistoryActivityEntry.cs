namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record PlayerHistoryActivityEntry
{
	public required DateTime DateTime { get; init; }

	public required double DeathsIncrement { get; init; }

	public required double TimeIncrement { get; init; }
}
