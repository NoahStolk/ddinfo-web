using DevilDaggersInfo.Core.Asset.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;

public class GetModAsset
{
	public AssetType Type { get; init; }

	public string Name { get; init; } = null!;

	public long Size { get; init; }
}
