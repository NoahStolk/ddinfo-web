namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;

public class GetSpawnsetByHashCustomLeaderboard
{
	public int CustomLeaderboardId { get; init; }

	public List<GetSpawnsetByHashCustomEntry> CustomEntries { get; init; } = new();
}
