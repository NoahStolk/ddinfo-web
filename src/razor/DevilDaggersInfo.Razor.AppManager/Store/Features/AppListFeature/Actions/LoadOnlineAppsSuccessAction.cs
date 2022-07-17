using DevilDaggersInfo.Api.Ddiam;

namespace DevilDaggersInfo.Razor.AppManager.Store.Features.AppListFeature.Actions;

public record LoadOnlineAppsSuccessAction(List<GetApp> Apps);
