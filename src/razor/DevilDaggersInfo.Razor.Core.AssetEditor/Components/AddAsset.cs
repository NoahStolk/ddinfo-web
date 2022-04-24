using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Razor.Core.AssetEditor.Pages;
using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Components;

public partial class AddAsset
{
	private string? _assetNameSearch;

	private string? _selectedAssetName;
	private AssetType? _selectedAssetType;

	private string? _selectedFileName;
	private byte[]? _selectedAssetData;

	private bool _writing;
	[CascadingParameter]
	public EditBinary Page { get; set; } = null!;

	[Inject]
	public IErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public IFileSystemService FileSystemService { get; set; } = null!;

	[Inject]
	public BinaryState BinaryState { get; set; } = null!;

	protected override void OnParametersSet()
	{
		if (BinaryState.Binary.ModBinaryType == ModBinaryType.Audio)
			_selectedAssetType = AssetType.Audio;
		else
			_selectedAssetType = null;
	}

	private IEnumerable<AssetInfo> GetFilteredAssets()
	{
		if (!_selectedAssetType.HasValue)
			return Enumerable.Empty<AssetInfo>();

		List<AssetInfo> data = AssetContainer.GetAll(_selectedAssetType.Value);
		if (_assetNameSearch == null)
			return data;

		return data.Where(a => a.AssetName.Contains(_assetNameSearch));
	}

	private void SelectAssetType(AssetType assetType)
	{
		_selectedAssetType = assetType;
		_selectedAssetName = null;

		_selectedFileName = null;
		_selectedAssetData = null;
	}

	private void Open(AssetType assetType)
	{
		if (!ModBinary.IsAssetTypeValid(BinaryState.Binary.ModBinaryType, assetType))
		{
			ErrorReporter.ReportError($"Asset type '{assetType}' is not compatible with binary type '{BinaryState.Binary.ModBinaryType}'.");
			return;
		}

		IFileSystemService.FileResult? fileResult = FileSystemService.Open(FileSystemService.GetAssetTypeFilter(assetType));
		if (fileResult == null)
			return;

		_selectedAssetType = assetType;

		_selectedFileName = Path.GetFileName(fileResult.Path);
		_selectedAssetData = fileResult.Contents;
	}

	private void Select(string assetName)
	{
		_selectedAssetName = assetName;
	}

	private async Task WriteToBinaryAsync()
	{
		await Task.Yield();

		_writing = true;
		StateHasChanged();

		if (_selectedAssetName == null || !_selectedAssetType.HasValue || _selectedAssetData == null)
		{
			ErrorReporter.ReportError("No asset was opened.");
			return;
		}

		if (BinaryState.Binary.Chunks.Any(c => c.Name == _selectedAssetName && c.AssetType == _selectedAssetType.Value))
		{
			ErrorReporter.ReportError($"An asset of type '{_selectedAssetType.Value}' with name '{_selectedAssetName}' already exists in this binary.");
			return;
		}

		try
		{
			BinaryState.AddAsset(_selectedAssetName, _selectedAssetType.Value, _selectedAssetData);
		}
		catch (Exception ex)
		{
			ErrorReporter.ReportError(ex);
		}

		Page.AddingNewAsset = false;
	}

	private void Cancel()
	{
		Page.AddingNewAsset = false;
	}
}
