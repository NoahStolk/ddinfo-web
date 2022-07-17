using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Razor.AppManager.Models;
using DevilDaggersInfo.Razor.AppManager.Store.Features.AppListFeature.Actions;
using DevilDaggersInfo.Razor.AppManager.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.AppManager.Store.Features.AppListFeature;

public static class AppListReducer
{
	[ReducerMethod]
	public static AppListState ReduceLoadLocalAppsAction(AppListState state, LoadLocalAppsAction action)
	{
		if (!Directory.Exists(action.InstallationDirectory))
			return new(new(), state.OnlineApps);

		List<AppEntry> localApps = new();
		foreach (string path in Directory.GetFiles(action.InstallationDirectory))
		{
			string fileName = Path.GetFileNameWithoutExtension(path);
			if (AppEntry.TryParse(fileName, out AppEntry? appEntry))
				localApps.Add(appEntry);
		}

		return new(localApps, state.OnlineApps);
	}

	[ReducerMethod]
	public static AppListState ReduceLoadOnlineAppsAction(AppListState state, LoadOnlineAppsAction action)
		=> new(state.LocalApps, state.OnlineApps);

	[ReducerMethod]
	public static AppListState ReduceLoadOnlineAppsFailureAction(AppListState state, LoadOnlineAppsFailureAction action)
		=> new(state.LocalApps, state.OnlineApps);

	[ReducerMethod]
	public static AppListState ReduceLoadOnlineAppsSuccessAction(AppListState state, LoadOnlineAppsSuccessAction action)
		=> new(state.LocalApps, action.Apps.ConvertAll(a => new AppEntry(a.Name, AppVersion.Parse(a.Version), a.BuildType)));
}
