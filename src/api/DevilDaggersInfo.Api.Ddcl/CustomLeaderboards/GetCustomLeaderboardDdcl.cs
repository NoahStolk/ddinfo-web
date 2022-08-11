using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetCustomLeaderboardDdcl
{
	public string SpawnsetName { get; init; } = null!;

	public GetCustomLeaderboardDaggersDdcl? Daggers { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public bool IsAscending { get; init; }
}
