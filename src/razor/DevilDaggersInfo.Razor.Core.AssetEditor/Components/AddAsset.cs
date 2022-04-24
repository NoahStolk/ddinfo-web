using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Components;

public partial class AddAsset
{
	private string? _selectedAssetName;
	private AssetType? _selectedAssetType;
	private byte[]? _selectedAssetData;

	[CascadingParameter]
	public BinaryEditor Editor { get; set; } = null!;

	[Inject]
	public IErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public IFileSystemService FileSystemService { get; set; } = null!;

	private void Open(AssetType assetType)
	{
		if (!ModBinary.IsAssetTypeValid(Editor.Binary.ModBinaryType, assetType))
		{
			ErrorReporter.ReportError($"Asset type '{assetType}' is not compatible with binary type '{Editor.Binary.ModBinaryType}'.");
			return;
		}

		IFileSystemService.FileResult? fileResult = FileSystemService.Open(FileSystemService.GetAssetTypeFilter(assetType));
		if (fileResult == null)
			return;

		_selectedAssetType = assetType;
		_selectedAssetData = fileResult.Contents;
	}

	private void Select(string assetName)
	{
		_selectedAssetName = assetName;
	}

	private void WriteToBinary()
	{
		if (_selectedAssetName == null || !_selectedAssetType.HasValue || _selectedAssetData == null)
		{
			ErrorReporter.ReportError("No asset was opened.");
			return;
		}

		if (Editor.Binary.Chunks.Any(c => c.Name == _selectedAssetName && c.AssetType == _selectedAssetType.Value))
		{
			ErrorReporter.ReportError($"An asset of type '{_selectedAssetType.Value}' with name '{_selectedAssetName}' already exists in this binary.");
			return;
		}

		try
		{
			Editor.Binary.AddAsset(_selectedAssetName, _selectedAssetType.Value, _selectedAssetData);
		}
		catch (Exception ex)
		{
			ErrorReporter.ReportError(ex);
		}

		Editor.AddingNewAsset = false;
	}

	private void Cancel()
	{
		Editor.AddingNewAsset = false;
	}
}
