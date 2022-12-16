namespace DevilDaggersInfo.Api.Admin.Spawnsets;

public record GetSpawnsetForOverview : IAdminOverviewGetDto
{
	public required int Id { get; init; }

	public required string Author { get; init; }

	public required string Name { get; init; }

	public required int? MaxDisplayWaves { get; init; }

	public required string? HtmlDescription { get; init; }

	public required DateTime LastUpdated { get; init; }

	public required bool IsPractice { get; init; }
}
