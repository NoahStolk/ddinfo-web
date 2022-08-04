namespace DevilDaggersInfo.Api.Admin.Spawnsets;

public record GetSpawnsetForOverview : IAdminOverviewGetDto
{
	public int Id { get; init; }

	public string Author { get; init; } = null!;

	public string Name { get; init; } = null!;

	public int? MaxDisplayWaves { get; init; }

	public string? HtmlDescription { get; init; }

	public DateTime LastUpdated { get; init; }

	public bool IsPractice { get; init; }
}
