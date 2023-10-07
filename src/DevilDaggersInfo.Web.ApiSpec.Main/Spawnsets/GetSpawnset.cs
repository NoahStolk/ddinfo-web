namespace DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

public record GetSpawnset
{
	public required int Id { get; init; }

	public required string Name { get; init; }

	public required string AuthorName { get; init; }

	public required bool IsPractice { get; init; }

	public required int? CustomLeaderboardId { get; init; }

	public required string? HtmlDescription { get; init; }

	public required int? MaxDisplayWaves { get; init; }

	public required DateTime LastUpdated { get; init; }

	public required byte[] FileBytes { get; init; }
}
