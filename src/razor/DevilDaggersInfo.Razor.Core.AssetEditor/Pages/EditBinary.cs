using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Pages;

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
	public IErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public IFileSystemService FileSystemService { get; set; } = null!;

	[Inject]
	public BinaryState BinaryState { get; set; } = null!;

	public void NewBinary(ModBinaryType modBinaryType)
	{
		BinaryState.SetBinary(new(modBinaryType));
		BinaryState.BinaryName = "(Untitled)";
	}

	public void OpenBinary()
	{
		IFileSystemService.FileResult? fileResult = FileSystemService.Open(string.Empty);
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
		if (BinaryState.Binary.Chunks.Count == 0)
			return;

		byte[] compiledBinary = BinaryState.Binary.Compile();
		FileSystemService.Save(compiledBinary);
	}
}
