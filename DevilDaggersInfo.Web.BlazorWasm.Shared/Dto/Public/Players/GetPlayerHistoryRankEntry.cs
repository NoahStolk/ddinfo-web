namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public record GetPlayerHistoryRankEntry
{
	public DateTime DateTime { get; init; }

	public int Rank { get; init; }
}
