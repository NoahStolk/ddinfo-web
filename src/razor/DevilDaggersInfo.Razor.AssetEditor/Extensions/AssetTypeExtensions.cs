using DevilDaggersInfo.Types.Core.Assets;

namespace DevilDaggersInfo.Razor.AssetEditor.Extensions;

public static class AssetTypeExtensions
{
	public static string GetBgColor(this AssetType assetType) => $"bg-{assetType.GetColor()}";

	public static string GetTextColor(this AssetType assetType) => $"text-{assetType.GetColor()}";

	private static string GetColor(this AssetType assetType) => assetType switch
	{
		AssetType.Audio => "audio",
		AssetType.ObjectBinding => "object-binding",
		AssetType.Shader => "shader",
		AssetType.Texture => "texture",
		AssetType.Mesh => "mesh",
		_ => "black",
	};
}
