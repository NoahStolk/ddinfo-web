using DevilDaggersInfo.Types.Core.Assets;

namespace DevilDaggersInfo.Api.Main.Mods;

public record GetModAsset
{
	public AssetType Type { get; init; }

	public string Name { get; init; } = null!;

	public long Size { get; init; }

	public bool IsProhibited { get; init; }
}
