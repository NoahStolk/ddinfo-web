using DevilDaggersInfo.Api.Main.Spawnsets;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class FetchSpawnsetById
{
	public static async Task<GetSpawnset?> HandleAsync(int spawnsetId)
	{
		try
		{
			return await AsyncHandler.Client.GetSpawnsetById(spawnsetId);
		}
		catch
		{
			return null;
		}
	}
}
