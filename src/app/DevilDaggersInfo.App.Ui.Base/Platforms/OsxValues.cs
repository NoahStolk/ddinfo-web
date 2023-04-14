using DevilDaggersInfo.Api.App.ProcessMemory;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Ui.Base.Platforms;

public class OsxValues : IPlatformSpecificValues
{
	public ToolBuildType BuildType => ToolBuildType.OsxWarp;

	public SupportedOperatingSystem OperatingSystem => SupportedOperatingSystem.Osx;

	public string DefaultInstallationPath => string.Empty;
}
