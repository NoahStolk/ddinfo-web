namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public readonly record struct CustomLeaderboardRanking
{
	public required int Rank { get; init; }

	public required int TotalPlayers { get; init; }
}
