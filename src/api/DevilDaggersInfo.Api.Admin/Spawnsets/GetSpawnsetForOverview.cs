namespace DevilDaggersInfo.Api.Admin.Spawnsets;

public record GetSpawnsetForOverview : IAdminOverviewGetDto
{
	public int Id { get; init; }

	public required string Author { get; init; }

	public required string Name { get; init; }

	public int? MaxDisplayWaves { get; init; }

	public string? HtmlDescription { get; init; }

	public DateTime LastUpdated { get; init; }

	public bool IsPractice { get; init; }
}
