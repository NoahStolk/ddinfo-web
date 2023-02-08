using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Launcher;

public static class Constants
{
#if WINDOWS
	public const ToolBuildType BuildType = ToolBuildType.WindowsWarp;
#elif LINUX
	public const ToolBuildType BuildType = ToolBuildType.LinuxWarp;
#endif

	public const ToolPublishMethod PublishMethod = ToolPublishMethod.SelfContained;

	public const string InstallationDirectory = "DDINFO TOOLS";
}
