namespace DevilDaggersInfo.Web.ApiSpec.Tools.Spawnsets;

public record GetSpawnset
{
	public required int Id { get; init; }

	public required string Name { get; init; }

	public required string AuthorName { get; init; }

	// TODO: Remove when ddinfo-tools 0.13.5.0 and older are deprecated. ddinfo-tools 0.13.5.0 and older are compiled with this property being required.
	[Obsolete("Practice spawnsets are now always generated and there's no need for them to be marked as such.")]
	public bool IsPractice { get; init; }

	public required int? CustomLeaderboardId { get; init; }

	public required string? HtmlDescription { get; init; }

	public required int? MaxDisplayWaves { get; init; }

	public required DateTime LastUpdated { get; init; }

	public required byte[] FileBytes { get; init; }
}
