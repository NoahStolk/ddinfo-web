namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Spawnsets;

public record GetSpawnsetName
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;
}
