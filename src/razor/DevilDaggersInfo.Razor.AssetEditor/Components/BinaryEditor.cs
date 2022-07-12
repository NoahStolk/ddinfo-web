using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Core.NativeInterface.Services;
using DevilDaggersInfo.Razor.AssetEditor.Pages;
using DevilDaggersInfo.Razor.AssetEditor.Store.State;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.AssetEditor.Components;

public partial class BinaryEditor
{
	[CascadingParameter]
	public EditBinary Page { get; set; } = null!;

	[Inject]
	public IState<BinaryEditorState> BinaryState { get; set; } = null!;

	[Inject]
	public INativeErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public INativeFileSystemService FileSystemService { get; set; } = null!;

	public void ExtractChunks()
	{
		string? directory = FileSystemService.SelectDirectory();
		if (directory == null)
			return;

		try
		{
			BinaryState.Value.ExtractChunks(directory);
		}
		catch (Exception ex)
		{
			ErrorReporter.ReportError(ex);
		}
	}

	public string GetBinaryTypeBackgroundColor()
	{
		if (BinaryState.Value.Binary.ModBinaryType == ModBinaryType.Audio)
			return "bg-audio";

		if (BinaryState.Value.Binary.Chunks.Count == 0)
			return "bg-texture";

		return BinaryState.Value.Binary.Chunks.GroupBy(c => c.AssetType).OrderByDescending(c => c.Count()).First().Key.GetBgColor();
	}
}
