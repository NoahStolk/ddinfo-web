using DevilDaggersInfo.Razor.AppManager.Models;

namespace DevilDaggersInfo.Razor.AppManager.Store.State;

public record AppListState(List<AppEntry> LocalApps, List<AppEntry> OnlineApps);
