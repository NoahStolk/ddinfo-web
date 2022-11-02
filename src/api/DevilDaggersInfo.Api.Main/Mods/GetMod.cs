using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Main.Mods;

public record GetMod
{
	public required string Name { get; init; }

	public string? HtmlDescription { get; init; }

	public string? Url { get; init; }

	public string? TrailerUrl { get; init; }

	public required List<string> Authors { get; init; }

	public DateTime LastUpdated { get; init; }

	public ModTypes ModTypes { get; init; }

	public bool IsHosted { get; init; }

	public bool? ContainsProhibitedAssets { get; init; }

	public GetModArchive? ModArchive { get; init; }

	public List<string>? ScreenshotFileNames { get; init; }
}
