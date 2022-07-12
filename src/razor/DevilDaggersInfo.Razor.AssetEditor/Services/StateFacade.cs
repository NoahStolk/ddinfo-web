using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Core.NativeInterface.Services;
using DevilDaggersInfo.Razor.AssetEditor.Store.Features.BinaryEditor.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.AssetEditor.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;
	private readonly INativeFileSystemService _fileSystemService;

	public StateFacade(IDispatcher dispatcher, INativeFileSystemService fileSystemService)
	{
		_dispatcher = dispatcher;
		_fileSystemService = fileSystemService;
	}

	public void NewBinary(ModBinaryType modBinaryType)
{
		_dispatcher.Dispatch(new OpenBinaryAction(new(modBinaryType), "(Untitled)"));
	}

	public void OpenBinary()
	{
		INativeFileSystemService.FileResult? fileResult = _fileSystemService.OpenFile(string.Empty);
		if (fileResult == null)
			return;

		try
		{
			ModBinary binary = new(fileResult.Contents, ModBinaryReadComprehensiveness.All);
			string name = Path.GetFileName(fileResult.Path);

			_dispatcher.Dispatch(new OpenBinaryAction(binary, name));
		}
		catch (Exception ex)
		{
			// TODO: Dispatch error action.
			//ErrorReporter.ReportError(ex);
		}
	}
}
