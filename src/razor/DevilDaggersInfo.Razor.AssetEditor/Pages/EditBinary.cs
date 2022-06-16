using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Core.NativeInterface;
using DevilDaggersInfo.Razor.AssetEditor.Services;
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
	public INativeErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public INativeFileSystemService FileSystemService { get; set; } = null!;

	[Inject]
	public BinaryState BinaryState { get; set; } = null!;

	public void NewBinary(ModBinaryType modBinaryType)
	{
		BinaryState.SetBinary(new(modBinaryType));
		BinaryState.BinaryName = "(Untitled)";
	}

	public void OpenBinary()
	{
		INativeFileSystemService.FileResult? fileResult = FileSystemService.OpenFile(string.Empty);
		if (fileResult == null)
			return;

		try
		{
			BinaryState.SetBinary(new(fileResult.Contents, ModBinaryReadComprehensiveness.All));
			BinaryState.BinaryName = Path.GetFileName(fileResult.Path);
		}
		catch (Exception ex)
		{
			ErrorReporter.ReportError(ex);
		}
	}

	public void SaveBinary()
	{
		// TODO: Implement.
		SaveBinaryAs();
	}

	public void SaveBinaryAs()
	{
		if (BinaryState.Binary.Chunks.Count == 0)
			return;

		byte[] compiledBinary = BinaryState.Binary.Compile();
		FileSystemService.SaveDataToFile(compiledBinary);
	}
}
