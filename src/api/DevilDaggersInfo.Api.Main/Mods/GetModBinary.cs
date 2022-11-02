using DevilDaggersInfo.Types.Core.Mods;

namespace DevilDaggersInfo.Api.Main.Mods;

public record GetModBinary
{
	public required string Name { get; init; }

	public long Size { get; init; }

	public ModBinaryType ModBinaryType { get; init; }

	public bool ContainsProhibitedAssets { get; init; }

	public required List<GetModAsset> Assets { get; init; }

	public List<GetModifiedLoudness>? ModifiedLoudness { get; init; }
}
