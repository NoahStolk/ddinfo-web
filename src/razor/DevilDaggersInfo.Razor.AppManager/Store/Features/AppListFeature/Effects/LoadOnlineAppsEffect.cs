using DevilDaggersInfo.Api.Ddiam;
using DevilDaggersInfo.Razor.AppManager.Services;
using DevilDaggersInfo.Razor.AppManager.Store.Features.AppListFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.AppManager.Store.Features.AppListFeature.Effects;

public class LoadOnlineAppsEffect : Effect<LoadOnlineAppsAction>
{
	private readonly NetworkService _networkService;

	public LoadOnlineAppsEffect(NetworkService networkService)
	{
		_networkService = networkService;
	}

	public override async Task HandleAsync(LoadOnlineAppsAction action, IDispatcher dispatcher)
	{
		try
		{
			List<GetApp> apps = await _networkService.GetApps(OperatingSystemType.Windows); // TODO

			dispatcher.Dispatch(new LoadOnlineAppsSuccessAction(apps));
		}
		catch
		{
			dispatcher.Dispatch(new LoadOnlineAppsFailureAction());
		}
	}
}
