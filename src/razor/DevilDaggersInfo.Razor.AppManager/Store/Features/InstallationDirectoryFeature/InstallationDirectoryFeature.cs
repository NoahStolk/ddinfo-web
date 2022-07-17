using DevilDaggersInfo.Razor.AppManager.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.AppManager.Store.Features.ManagerFeature;

public class InstallationDirectoryFeature : Feature<InstallationDirectoryState>
{
	public override string GetName() => "Manager";

	protected override InstallationDirectoryState GetInitialState()
	{
		string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		string installationDirectory = Path.Combine(appData, "DevilDaggersInfo");
		return new(installationDirectory);
	}
}
