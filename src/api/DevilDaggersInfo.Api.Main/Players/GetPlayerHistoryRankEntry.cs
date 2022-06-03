namespace DevilDaggersInfo.Api.Main.Players;

public record GetPlayerHistoryRankEntry
{
	public DateTime DateTime { get; init; }

	public int Rank { get; init; }
}
