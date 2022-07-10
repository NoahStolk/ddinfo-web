using DevilDaggersInfo.Core.NativeInterface;
using DevilDaggersInfo.Razor.AssetEditor.Services;
using DevilDaggersInfo.Razor.AssetEditor.Store.State;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.AssetEditor.Pages;

public partial class EditBinary
{
	private bool _addingNewAsset;

	public bool AddingNewAsset
	{
		get => _addingNewAsset;
		set
		{
			_addingNewAsset = value;
			StateHasChanged();
		}
	}

	[Inject]
	public INativeFileSystemService FileSystemService { get; set; } = null!;

	[Inject]
	public StateFacade StateFacade { get; set; } = null!;

	[Inject]
	public IState<BinaryEditorState> BinaryEditorState { get; set; } = null!;

	public void SaveBinary()
	{
		// TODO: Implement.
		SaveBinaryAs();
	}

	public void SaveBinaryAs()
	{
		if (BinaryEditorState.Value.Binary.Chunks.Count == 0)
			return;

		byte[] compiledBinary = BinaryEditorState.Value.Binary.Compile();
		FileSystemService.SaveDataToFile(compiledBinary);
	}
}
