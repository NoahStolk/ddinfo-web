namespace DevilDaggersInfo.Web.Shared.Dto.Public.CustomLeaderboards;

public record GetCustomLeaderboardDdcl
{
	public string SpawnsetName { get; init; } = null!;

	public GetCustomLeaderboardDaggers? Daggers { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public bool IsAscending { get; init; }
}
