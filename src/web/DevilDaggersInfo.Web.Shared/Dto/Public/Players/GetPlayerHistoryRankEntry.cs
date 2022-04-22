namespace DevilDaggersInfo.Web.Shared.Dto.Public.Players;

public record GetPlayerHistoryRankEntry
{
	public DateTime DateTime { get; init; }

	public int Rank { get; init; }
}
