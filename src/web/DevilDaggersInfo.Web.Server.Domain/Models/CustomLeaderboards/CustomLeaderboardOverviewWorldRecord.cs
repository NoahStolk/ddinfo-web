namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class CustomLeaderboardOverviewWorldRecord
{
	public int Time { get; init; }

	public int PlayerId { get; init; }

	public string PlayerName { get; init; } = null!;

	public CustomLeaderboardDagger? Dagger { get; init; }
}
