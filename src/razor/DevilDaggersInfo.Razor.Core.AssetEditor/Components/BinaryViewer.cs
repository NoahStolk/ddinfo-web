using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Components;

public partial class BinaryViewer
{
	[Parameter]
	[EditorRequired]
	public string? BinaryName { get; set; }

	[Parameter]
	[EditorRequired]
	public ModBinary Binary { get; set; } = null!;

	[Inject]
	public IErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public IFileSystemService FileSystemService { get; set; } = null!;

	public void ExtractAsset(ModBinaryChunk chunk)
	{
		if (Binary == null)
			return;

		byte[] extractedBuffer;
		try
		{
			extractedBuffer = Binary.ExtractAsset(chunk.Name, chunk.AssetType);
		}
		catch (Exception ex)
		{
			ErrorReporter.ReportError(ex);
			return;
		}

		FileSystemService.Save(extractedBuffer);
	}

	public void AddAsset()
	{

	}
}
