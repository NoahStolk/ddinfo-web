using DevilDaggersInfo.Api.App.ProcessMemory;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class FetchMarker
{
	public static async Task<GetMarker?> HandleAsync(SupportedOperatingSystem supportedOperatingSystem)
	{
		try
		{
			return await AsyncHandler.Client.GetMarker(supportedOperatingSystem);
		}
		catch
		{
			return null;
		}
	}
}
