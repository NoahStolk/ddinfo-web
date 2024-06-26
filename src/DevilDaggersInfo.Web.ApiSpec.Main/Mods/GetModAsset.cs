namespace DevilDaggersInfo.Web.ApiSpec.Main.Mods;

public record GetModAsset
{
	public required AssetType Type { get; init; }

	public required string Name { get; init; }

	public required long Size { get; init; }

	public required bool IsProhibited { get; init; }
}
