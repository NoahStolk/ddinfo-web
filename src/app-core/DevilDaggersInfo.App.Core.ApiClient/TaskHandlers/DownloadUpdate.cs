using DevilDaggersInfo.Api.App.Updates;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public class DownloadUpdate
{
	public static async Task<GetLatestVersionFile?> HandleAsync(ToolBuildType toolBuildType)
	{
		try
		{
			return await AsyncHandler.Client.GetLatestVersionFile(ToolPublishMethod.SelfContained, toolBuildType);
		}
		catch
		{
			return null;
		}
	}
}
