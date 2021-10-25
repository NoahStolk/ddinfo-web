namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;

public class GetSpawnset
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public string AuthorName { get; init; } = null!;

	public bool IsPractice { get; init; }

	public DateTime LastUpdated { get; init; }

	public byte[] FileBytes { get; init; } = null!;
}
