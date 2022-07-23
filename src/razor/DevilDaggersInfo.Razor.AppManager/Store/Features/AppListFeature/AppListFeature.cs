using DevilDaggersInfo.Razor.AppManager.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.AppManager.Store.Features.AppListFeature;

public class AppListFeature : Feature<AppListState>
{
	public override string GetName() => "App list";

	protected override AppListState GetInitialState()
	{
		return new(new(), new());
	}
}
