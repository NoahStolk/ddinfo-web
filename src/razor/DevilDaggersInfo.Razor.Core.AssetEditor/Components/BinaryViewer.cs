using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Core.Mod.Exceptions;
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

	public void ReadBinary()
	{
		_binary = null;

		IFileSystemService.FileResult? fileResult = FileSystemService.Open();
		if (fileResult == null)
			return;

		try
		{
			_binary = new(fileResult.Contents, ModBinaryReadComprehensiveness.TocOnly);
		}
		catch (InvalidModBinaryException ex)
		{
			ErrorReporter.ReportError(ex);
		}
		catch (Exception ex)
		{
			ErrorReporter.ReportError(ex);
		}
	}
}
