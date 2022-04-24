using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Razor.Core.AssetEditor.Pages;
using DevilDaggersInfo.Razor.Core.AssetEditor.State;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Components;

public partial class BinaryEditor
{
	[CascadingParameter]
	public EditBinary Page { get; set; } = null!;

	[Inject]
	public BinaryState BinaryState { get; set; } = null!;

	public static string GetBgColor(AssetType assetType) => $"bg-{GetColor(assetType)}";

	public static string GetTextColor(AssetType assetType) => $"text-{GetColor(assetType)}";

	private static string GetColor(AssetType assetType) => assetType switch
	{
		AssetType.Audio => "audio",
		AssetType.ObjectBinding => "object-binding",
		AssetType.Shader => "shader",
		AssetType.Texture => "texture",
		AssetType.Mesh => "mesh",
		_ => "black",
	};
}
