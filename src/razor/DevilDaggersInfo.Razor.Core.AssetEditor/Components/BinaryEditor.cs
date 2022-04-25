using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Razor.Core.AssetEditor.Pages;
using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Components;

public partial class BinaryEditor
{
	[CascadingParameter]
	public EditBinary Page { get; set; } = null!;

	[Inject]
	public BinaryState BinaryState { get; set; } = null!;

	[Inject]
	public IErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public IFileSystemService FileSystemService { get; set; } = null!;

	public void ExtractChunks()
	{
		string? directory = FileSystemService.SelectDirectory();
		if (directory == null)
			return;

		try
		{
			BinaryState.ExtractChunks(directory);
		}
		catch (Exception ex)
		{
			ErrorReporter.ReportError(ex);
		}
	}

	public string GetBinaryTypeBackgroundColor()
	{
		if (BinaryState.Binary.ModBinaryType == ModBinaryType.Audio)
			return "bg-audio";

		if (BinaryState.Binary.Chunks.Count == 0)
			return "bg-texture";

		return BinaryState.Binary.Chunks.GroupBy(c => c.AssetType).OrderByDescending(c =>c.Count()).First().Key.GetBgColor();
	}
}
