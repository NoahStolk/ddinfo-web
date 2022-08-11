using DevilDaggersInfo.Types.Core;

namespace DevilDaggersInfo.Razor.AssetEditor.Services;

public interface IAssetEditorFileFilterService
{
	string GetAssetTypeFilter(AssetType assetType);

	string GetVertexShaderFilter();

	string GetFragmentShaderFilter();
}
