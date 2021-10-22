namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;

public class GetSpawnsetByHash
{
	public int SpawnsetId { get; init; }

	public string Name { get; init; } = null!;

	public string AuthorName { get; init; } = null!;

	public GetSpawnsetByHashCustomLeaderboard? CustomLeaderboard { get; init; }
}
