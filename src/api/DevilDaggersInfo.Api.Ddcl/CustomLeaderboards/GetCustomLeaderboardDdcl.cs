using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetCustomLeaderboardDdcl
{
	public required string SpawnsetName { get; init; }

	public GetCustomLeaderboardDaggersDdcl? Daggers { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public bool IsAscending { get; init; }
}
