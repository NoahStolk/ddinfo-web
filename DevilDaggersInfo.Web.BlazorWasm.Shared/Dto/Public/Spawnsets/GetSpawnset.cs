namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;

public class GetSpawnset
{
	public int? MaxDisplayWaves { get; init; }

	public string? HtmlDescription { get; init; }

	public DateTime LastUpdated { get; init; }

	public GetSpawnsetData SpawnsetData { get; init; } = null!;

	public string Name { get; init; } = null!;

	public string AuthorName { get; init; } = null!;

	public bool HasCustomLeaderboard { get; init; }

	public bool IsPractice { get; init; }
}
