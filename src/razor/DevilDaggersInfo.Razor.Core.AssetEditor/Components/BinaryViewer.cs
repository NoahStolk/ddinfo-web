using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.AssetEditor.Components;

public partial class BinaryViewer
{
	private ModBinary? _binary;

	[Inject]
	public IErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public IFileSystemService FileSystemService { get; set; } = null!;

	public void NewBinary()
	{
		_binary = null;
	}

	public void OpenBinary()
	{
		_binary = null;

		IFileSystemService.FileResult? fileResult = FileSystemService.Open();
		if (fileResult == null)
			return;

		try
		{
			_binary = new(fileResult.Contents, ModBinaryReadComprehensiveness.All);
		}
		catch (Exception ex)
		{
			ErrorReporter.ReportError(ex);
		}
	}

	public void ExtractBinary()
	{
		if (_binary == null)
			return;

		string outputDirectory = ""; // TODO
		_binary.ExtractAssets(outputDirectory);
	}

	public void ExtractAsset(ModBinaryChunk chunk)
	{
		if (_binary == null)
			return;

		byte[] extractedBuffer;
		try
		{
			extractedBuffer = _binary.ExtractAsset(chunk.Name, chunk.AssetType);
		}
		catch (Exception ex)
		{
			ErrorReporter.ReportError(ex);
			return;
		}

		FileSystemService.Save(extractedBuffer);
	}

	public void CompileBinary()
	{
	}
}
