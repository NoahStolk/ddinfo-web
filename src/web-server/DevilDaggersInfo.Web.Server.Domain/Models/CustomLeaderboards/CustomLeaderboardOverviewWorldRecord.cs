using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class CustomLeaderboardOverviewWorldRecord
{
	public int Time { get; init; }

	public int PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public CustomLeaderboardDagger? Dagger { get; init; }
}
