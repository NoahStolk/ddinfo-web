using DevilDaggersInfo.Api.Main.Tools;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class FetchLatestDistribution
{
	public static async Task<AppVersion?> HandleAsync(AppVersion appVersion, ToolBuildType toolBuildType)
	{
		try
		{
			GetToolDistribution distribution = await AsyncHandler.Client.GetLatestToolDistribution("ddinfo-tools", ToolPublishMethod.SelfContained, toolBuildType);
			if (!AppVersion.TryParse(distribution.VersionNumber, out AppVersion? onlineVersion))
				return null;

			return onlineVersion > appVersion ? onlineVersion : null;
		}
		catch
		{
			return null;
		}
	}
}
