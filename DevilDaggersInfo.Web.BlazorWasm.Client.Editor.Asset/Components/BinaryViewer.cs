using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Core.Mod.Exceptions;
using DevilDaggersInfo.Web.BlazorWasm.Client.Editor.Asset.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Editor.Asset.Components;

public partial class BinaryViewer
{
	private readonly List<AssetKey> _assets = new();

	[Inject]
	public IErrorReporter ErrorReporter { get; set; } = null!;

	[Inject]
	public IFileSystemService FileSystemService { get; set; } = null!;

	public void ReadBinary()
	{
		_assets.Clear();

		IFileSystemService.FileResult? fileResult = FileSystemService.Open();
		if (fileResult == null)
			return;

		try
		{
			ModBinary modBinary = new(Path.GetFileName(fileResult.Path), fileResult.Contents, ModBinaryReadComprehensiveness.TocOnly);
			_assets.AddRange(modBinary.Chunks.Select(c => new AssetKey(c.AssetType, c.Name)));
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
