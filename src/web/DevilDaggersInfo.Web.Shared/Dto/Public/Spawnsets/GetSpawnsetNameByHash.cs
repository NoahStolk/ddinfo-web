namespace DevilDaggersInfo.Web.Shared.Dto.Public.Spawnsets;

public record GetSpawnsetNameByHash
{
	public string Name { get; init; } = null!;
}
