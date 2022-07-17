using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.Razor.AppManager.Store.Features.AppListFeature.Actions;
using DevilDaggersInfo.Razor.AppManager.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.AppManager.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;
	private readonly INativeFileSystemService _fileSystemService;
	private readonly IState<InstallationDirectoryState> _installationDirectoryState;

	public StateFacade(IDispatcher dispatcher, INativeFileSystemService fileSystemService, IState<InstallationDirectoryState> installationDirectoryState)
	{
		_dispatcher = dispatcher;
		_fileSystemService = fileSystemService;
		_installationDirectoryState = installationDirectoryState;
	}

	public void LoadAppLists()
	{
		_dispatcher.Dispatch(new LoadLocalAppsAction(_installationDirectoryState.Value.InstallationDirectory));
		_dispatcher.Dispatch(new LoadOnlineAppsAction());
	}
}
