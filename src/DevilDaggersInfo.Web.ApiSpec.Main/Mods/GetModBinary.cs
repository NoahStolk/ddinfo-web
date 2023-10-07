namespace DevilDaggersInfo.Web.ApiSpec.Main.Mods;

public record GetModBinary
{
	public required string Name { get; init; }

	public required long Size { get; init; }

	public required ModBinaryType ModBinaryType { get; init; }

	public required bool ContainsProhibitedAssets { get; init; }

	public required List<GetModAsset> Assets { get; init; }

	public required List<GetModifiedLoudness>? ModifiedLoudness { get; init; }
}
