namespace DevilDaggersInfo.Api.Main.Spawnsets;

public record GetSpawnsetName
{
	public required int Id { get; init; }

	public required string Name { get; init; }
}
