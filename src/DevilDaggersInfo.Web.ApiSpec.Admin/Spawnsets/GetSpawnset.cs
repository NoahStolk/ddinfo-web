namespace DevilDaggersInfo.Web.ApiSpec.Admin.Spawnsets;

public record GetSpawnset
{
	public required int Id { get; init; }

	public required int PlayerId { get; init; }

	public required string Name { get; init; }

	public required int? MaxDisplayWaves { get; init; }

	public required string? HtmlDescription { get; init; }

	public required DateTime LastUpdated { get; init; }
}
