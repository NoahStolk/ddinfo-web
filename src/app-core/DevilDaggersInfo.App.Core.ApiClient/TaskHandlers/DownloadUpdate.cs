using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public class DownloadUpdate
{
	public static async Task<byte[]?> HandleAsync(ToolBuildType toolBuildType)
	{
		try
		{
			return await AsyncHandler.Client.GetFile(ToolPublishMethod.SelfContained, toolBuildType);
		}
		catch
		{
			return null;
		}
	}
}
