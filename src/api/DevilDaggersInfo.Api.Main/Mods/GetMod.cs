using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Main.Mods;

public record GetMod
{
	public string Name { get; init; } = null!;

	public string? HtmlDescription { get; init; }

	public string? Url { get; init; }

	public string? TrailerUrl { get; init; }

	public List<string> Authors { get; init; } = null!;

	public DateTime LastUpdated { get; init; }

	public ModTypes ModTypes { get; init; }

	public bool IsHosted { get; init; }

	public bool? ContainsProhibitedAssets { get; init; }

	public GetModArchive? ModArchive { get; init; }

	public List<string>? ScreenshotFileNames { get; init; }
}
