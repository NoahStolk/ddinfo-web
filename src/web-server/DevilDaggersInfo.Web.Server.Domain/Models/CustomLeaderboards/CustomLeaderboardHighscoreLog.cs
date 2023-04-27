namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardHighscoreLog
{
	public required CustomLeaderboardDagger Dagger { get; init; }

	public required int CustomLeaderboardId { get; init; }

	public required string Message { get; init; }

	public required int Rank { get; init; }

	public required int TotalPlayers { get; init; }

	public required int Time { get; init; }
}
