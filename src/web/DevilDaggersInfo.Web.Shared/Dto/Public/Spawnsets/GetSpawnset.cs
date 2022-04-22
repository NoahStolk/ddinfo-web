namespace DevilDaggersInfo.Web.Shared.Dto.Public.Spawnsets;

public record GetSpawnset
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public string AuthorName { get; init; } = null!;

	public bool IsPractice { get; init; }

	public int? CustomLeaderboardId { get; init; }

	public string? HtmlDescription { get; init; }

	public int? MaxDisplayWaves { get; init; }

	public DateTime LastUpdated { get; init; }

	public byte[] FileBytes { get; init; } = null!;
}
