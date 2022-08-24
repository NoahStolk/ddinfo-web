using DevilDaggersInfo.App.Core.NativeInterface.Services;
using Fluxor;

namespace DevilDaggersInfo.Razor.SurvivalEditor.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;
	private readonly INativeFileSystemService _fileSystemService;
	private readonly NetworkService _networkService;
	private readonly INativeErrorReporter _errorReporter;

	public StateFacade(IDispatcher dispatcher, INativeFileSystemService fileSystemService, NetworkService networkService, INativeErrorReporter errorReporter)
	{
		_dispatcher = dispatcher;
		_fileSystemService = fileSystemService;
		_networkService = networkService;
		_errorReporter = errorReporter;
	}
}
