namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public sealed record PlayerHistoryRankEntry
{
	public required DateTime DateTime { get; init; }

	public required int Rank { get; init; }
}
