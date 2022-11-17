namespace DevilDaggersInfo.Api.Ddse.Spawnsets;

public record GetSpawnsetDdse
{
	public int? MaxDisplayWaves { get; init; }

	public string? HtmlDescription { get; init; }

	public DateTime LastUpdated { get; init; }

	public required GetSpawnsetDataDdse SpawnsetData { get; init; }

	public required string Name { get; init; }

	public required string AuthorName { get; init; }

	public bool HasCustomLeaderboard { get; init; }

	public bool IsPractice { get; init; }
}
