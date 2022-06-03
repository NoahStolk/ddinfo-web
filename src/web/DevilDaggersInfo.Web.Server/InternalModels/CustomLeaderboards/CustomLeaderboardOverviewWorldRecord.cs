using DevilDaggersInfo.Web.Server.Enums;

namespace DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;

public class CustomLeaderboardOverviewWorldRecord
{
	public int Time { get; init; }

	public int PlayerId { get; init; }

	public string PlayerName { get; init; } = null!;

	public CustomLeaderboardDagger? Dagger { get; init; }
}
