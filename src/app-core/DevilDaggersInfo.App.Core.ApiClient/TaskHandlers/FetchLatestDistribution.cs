using DevilDaggersInfo.Api.Main.Tools;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public sealed class FetchLatestDistribution : ITaskHandler<AppVersion>
{
	public static async Task<AppVersion?> HandleAsync(AsyncHandler asyncHandler)
	{
		try
		{
			GetToolDistribution distribution = await asyncHandler.Client.GetLatestToolDistribution("ddinfo-tools", ToolPublishMethod.SelfContained, asyncHandler.ToolBuildType);
			if (!AppVersion.TryParse(distribution.VersionNumber, out AppVersion? onlineVersion))
				return null;

			return onlineVersion > asyncHandler.CurrentAppVersion ? onlineVersion : null;
		}
		catch
		{
			return null;
		}
	}
}
