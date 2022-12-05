namespace DevilDaggersInfo.Api.App.Spawnsets;

public record GetSpawnset
{
	public int Id { get; init; }

	public required string Name { get; init; }

	public required string AuthorName { get; init; }

	public bool IsPractice { get; init; }

	public int? CustomLeaderboardId { get; init; }

	public string? HtmlDescription { get; init; }

	public int? MaxDisplayWaves { get; init; }

	public DateTime LastUpdated { get; init; }

	public required byte[] FileBytes { get; init; }
}
