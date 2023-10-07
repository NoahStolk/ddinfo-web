namespace DevilDaggersInfo.Web.ApiSpec.Admin.Mods;

public record GetMod
{
	public required int Id { get; init; }

	public required string Name { get; init; }

	public required bool IsHidden { get; init; }

	public required DateTime LastUpdated { get; init; }

	public required string? TrailerUrl { get; init; }

	public required string? HtmlDescription { get; init; }

	public required ModTypes ModTypes { get; init; }

	public required string? Url { get; init; }

	public required List<int>? PlayerIds { get; init; }

	public required List<string>? BinaryNames { get; init; }

	public required List<string>? ScreenshotNames { get; init; }
}
