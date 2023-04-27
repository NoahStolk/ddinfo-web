namespace DevilDaggersInfo.Api.Main.Mods;

public record GetMod
{
	public required string Name { get; init; }

	public required string? HtmlDescription { get; init; }

	public required string? Url { get; init; }

	public required string? TrailerUrl { get; init; }

	public required List<string> Authors { get; init; }

	public required DateTime LastUpdated { get; init; }

	public required ModTypes ModTypes { get; init; }

	public required bool IsHosted { get; init; }

	public required bool? ContainsProhibitedAssets { get; init; }

	public required GetModArchive? ModArchive { get; init; }

	public required List<string>? ScreenshotFileNames { get; init; }
}
