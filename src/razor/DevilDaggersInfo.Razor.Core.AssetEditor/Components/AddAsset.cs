using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Razor.Core.AssetEditor.Pages;
using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Components;

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

		_selectedVertFileName = null;
		_selectedVertData = null;

		_selectedFragFileName = null;
		_selectedFragData = null;
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

	private void OpenVert() => OpenShader((s) => _selectedVertFileName = s, (d) => _selectedVertData = d, FileSystemService.GetVertexShaderFilter());

	private void OpenFrag() => OpenShader((s) => _selectedFragFileName = s, (d) => _selectedFragData = d, FileSystemService.GetFragmentShaderFilter());

	private void OpenShader(Action<string> setFileName, Action<byte[]> setData, string fileExtension)
	{
		if (!ModBinary.IsAssetTypeValid(BinaryState.Binary.ModBinaryType, AssetType.Shader))
		{
			ErrorReporter.ReportError($"Asset type '{AssetType.Shader}' is not compatible with binary type '{BinaryState.Binary.ModBinaryType}'.");
			return;
		}

		IFileSystemService.FileResult? fileResult = FileSystemService.Open(fileExtension);
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
			bw.Write(Encoding.Default.GetBytes(_selectedAssetName));
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
