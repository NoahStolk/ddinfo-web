using DevilDaggersInfo.Api.App.Updates;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class FetchLatestDistribution
{
	public static async Task<AppVersion?> HandleAsync(AppVersion appVersion, ToolBuildType toolBuildType)
	{
		try
		{
			GetLatestVersion latestVersion = await AsyncHandler.Client.GetLatestVersion(ToolPublishMethod.SelfContained, toolBuildType);
			if (!AppVersion.TryParse(latestVersion.VersionNumber, out AppVersion? onlineVersion))
				return null;

			return onlineVersion > appVersion ? onlineVersion : null;
		}
		catch
		{
			return null;
		}
	}
}
