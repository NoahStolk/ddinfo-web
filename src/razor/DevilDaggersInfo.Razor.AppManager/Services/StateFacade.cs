using DevilDaggersInfo.App.Core.NativeInterface.Services;
using Fluxor;

namespace DevilDaggersInfo.Razor.AppManager.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;
	private readonly INativeFileSystemService _fileSystemService;

	public StateFacade(IDispatcher dispatcher, INativeFileSystemService fileSystemService)
	{
		_dispatcher = dispatcher;
		_fileSystemService = fileSystemService;
	}
}
