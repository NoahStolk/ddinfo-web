using DevilDaggersInfo.Razor.AppManager.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.AppManager.Store.Features.ManagerFeature;

public class ManagerFeature : Feature<ManagerState>
{
	public override string GetName() => "Manager";

	protected override ManagerState GetInitialState()
	{
		string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		return new(Path.Combine(appData, "DevilDaggersInfo"));
	}
}
