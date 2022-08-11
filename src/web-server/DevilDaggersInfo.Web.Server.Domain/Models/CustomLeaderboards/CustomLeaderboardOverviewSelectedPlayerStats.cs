using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class CustomLeaderboardOverviewSelectedPlayerStats
{
	public int Rank { get; init; }

	public int Time { get; init; }

	public CustomLeaderboardDagger? Dagger { get; init; }
}
