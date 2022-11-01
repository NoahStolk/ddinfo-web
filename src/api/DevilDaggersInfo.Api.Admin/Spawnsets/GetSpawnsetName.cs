namespace DevilDaggersInfo.Api.Admin.Spawnsets;

public record GetSpawnsetName
{
	public int Id { get; init; }

	public required string Name { get; init; }
}
