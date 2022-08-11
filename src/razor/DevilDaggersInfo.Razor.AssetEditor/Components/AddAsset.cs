using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Razor.AssetEditor.Pages;
using DevilDaggersInfo.Razor.AssetEditor.Services;
using DevilDaggersInfo.Razor.AssetEditor.Store.State;
using DevilDaggersInfo.Types.Core.Assets;
using DevilDaggersInfo.Types.Core.Mods;
using Fluxor;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace DevilDaggersInfo.Razor.AssetEditor.Components;

public partial class AddAsset
{
	private string? _assetNameSearch;

	private AssetType? _selectedAssetType;
	private string? _selectedAssetName;

	private string? _selectedFileName;
	private byte[]? _selectedAssetData;

	private string? _selectedVertFileName;
	private byte[]? _selectedVertData;

	private string? _selectedFragFileName;
	private byte[]? _selectedFragData;

	private bool _writing;

	[CascadingParameter]
	public EditBinary Page { get; set; } = null!;

	[Inject]
	public INativeErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public INativeFileSystemService FileSystemService { get; set; } = null!;

	[Inject]
	public IAssetEditorFileFilterService FileFilterService { get; set; } = null!;

	[Inject]
	public IState<BinaryEditorState> BinaryState { get; set; } = null!;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		if (BinaryState.Value.Binary.ModBinaryType == ModBinaryType.Audio)
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

		_selectedVertFileName = null;
		_selectedVertData = null;

		_selectedFragFileName = null;
		_selectedFragData = null;
	}

	private void Open(AssetType assetType)
	{
		if (!ModBinary.IsAssetTypeValid(BinaryState.Value.Binary.ModBinaryType, assetType))
		{
			ErrorReporter.ReportError("Invalid asset type", $"Asset type '{assetType}' is not compatible with binary type '{BinaryState.Value.Binary.ModBinaryType}'.");
			return;
		}

		INativeFileSystemService.FileResult? fileResult = FileSystemService.OpenFile(FileFilterService.GetAssetTypeFilter(assetType));
		if (fileResult == null)
			return;

		_selectedAssetType = assetType;

		_selectedFileName = Path.GetFileName(fileResult.Path);
		_selectedAssetData = fileResult.Contents;
	}

	private void OpenVert() => OpenShader((s) => _selectedVertFileName = s, (d) => _selectedVertData = d, FileFilterService.GetVertexShaderFilter());

	private void OpenFrag() => OpenShader((s) => _selectedFragFileName = s, (d) => _selectedFragData = d, FileFilterService.GetFragmentShaderFilter());

	private void OpenShader(Action<string> setFileName, Action<byte[]> setData, string fileExtension)
	{
		if (!ModBinary.IsAssetTypeValid(BinaryState.Value.Binary.ModBinaryType, AssetType.Shader))
		{
			ErrorReporter.ReportError("Invalid asset type", $"Asset type '{AssetType.Shader}' is not compatible with binary type '{BinaryState.Value.Binary.ModBinaryType}'.");
			return;
		}

		INativeFileSystemService.FileResult? fileResult = FileSystemService.OpenFile(fileExtension);
		if (fileResult == null)
			return;

		_selectedAssetType = AssetType.Shader;

		setFileName(Path.GetFileName(fileResult.Path));
		setData(fileResult.Contents);

		if (_selectedAssetName != null && _selectedVertData != null && _selectedFragData != null)
		{
			using MemoryStream ms = new();
			using BinaryWriter bw = new(ms);

			bw.Write(_selectedAssetName.Length);
			bw.Write(_selectedVertData.Length);
			bw.Write(_selectedFragData.Length);
			bw.Write(Encoding.UTF8.GetBytes(_selectedAssetName));
			bw.Write(_selectedVertData);
			bw.Write(_selectedFragData);

			_selectedAssetData = ms.ToArray();
		}
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
			ErrorReporter.ReportError("No asset", "No asset was opened.");
			return;
		}

		if (BinaryState.Value.Binary.Chunks.Any(c => c.Name == _selectedAssetName && c.AssetType == _selectedAssetType.Value))
		{
			ErrorReporter.ReportError("Conflicting asset", $"An asset of type '{_selectedAssetType.Value}' with name '{_selectedAssetName}' already exists in this binary.");
			return;
		}

		try
		{
			BinaryState.Value.AddAsset(_selectedAssetName, _selectedAssetType.Value, _selectedAssetData);
		}
		catch (Exception ex)
		{
			ErrorReporter.ReportError("Could not add asset", "An error occurred while adding the asset.", ex);
			return;
		}

		Page.AddingNewAsset = false;
	}

	private void Cancel()
	{
		Page.AddingNewAsset = false;
	}
}
