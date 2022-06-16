using DevilDaggersInfo.Api.Ddcl.ProcessMemory;
using DevilDaggersInfo.Api.Ddcl.Tools;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Services;

public interface IClientConfiguration
{
	string GetHostBaseUrl();

	string GetApplicationName();

	string GetApplicationVersion();

	SupportedOperatingSystem GetOperatingSystem();

	string GetBuildMode();

	ToolPublishMethod GetToolPublishMethod();

	ToolBuildType GetToolBuildType();
}
