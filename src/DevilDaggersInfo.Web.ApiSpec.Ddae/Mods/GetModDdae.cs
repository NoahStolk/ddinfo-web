namespace DevilDaggersInfo.Web.ApiSpec.Ddae.Mods;

public record GetModDdae
{
	public required string Name { get; init; }

	public string? HtmlDescription { get; init; }

	public string? TrailerUrl { get; init; }

	public required List<string> Authors { get; init; }

	public DateTime LastUpdated { get; init; }

	public ModTypesDdae AssetModTypes { get; init; }

	public bool IsHostedOnDdInfo { get; init; }

	public bool? ContainsProhibitedAssets { get; init; }

	public GetModArchiveDdae? ModArchive { get; init; }

	public required List<string> ScreenshotFileNames { get; init; }
}
