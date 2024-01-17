namespace DevilDaggersInfo.Web.ApiSpec.Ddse.Spawnsets;

public record GetSpawnsetDdse
{
	public required int Id { get; init; }

	public required int? MaxDisplayWaves { get; init; }

	public required string? HtmlDescription { get; init; }

	public required DateTime LastUpdated { get; init; }

	public required GetSpawnsetDataDdse SpawnsetData { get; init; }

	public required string Name { get; init; }

	public required string AuthorName { get; init; }

	public required bool HasCustomLeaderboard { get; init; }

	public required bool IsPractice { get; init; }
}
