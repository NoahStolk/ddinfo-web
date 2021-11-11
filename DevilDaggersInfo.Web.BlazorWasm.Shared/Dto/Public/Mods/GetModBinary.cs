using DevilDaggersInfo.Core.Mod.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;

public class GetModBinary
{
	public string Name { get; init; } = null!;

	public long Size { get; init; }

	public ModBinaryType ModBinaryType { get; init; }

	public bool ContainsProhibitedAssets { get; init; }

	public List<GetModAsset> Assets { get; init; } = new();

	public List<GetModifiedLoudness>? ModifiedLoudness { get; init; }
}
