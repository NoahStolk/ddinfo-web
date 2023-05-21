namespace DevilDaggersInfo.Api.Clubber.Players;

public record GetPlayerHistoryRankEntry
{
	public required DateTime DateTime { get; init; }

	public required int Rank { get; init; }
}
