namespace DevilDaggersInfo.Api.Admin.Spawnsets;

public record GetSpawnsetName
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;
}
