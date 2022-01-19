namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public class GetPlayerHistoryActivityEntry
{
	public DateTime DateTime { get; init; }

	public double DeathsIncrement { get; init; }

	public double TimeIncrement { get; init; }
}
