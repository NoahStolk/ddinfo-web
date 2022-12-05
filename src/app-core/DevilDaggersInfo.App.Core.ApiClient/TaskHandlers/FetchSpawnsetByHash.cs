using DevilDaggersInfo.Api.App.Spawnsets;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class FetchSpawnsetByHash
{
	public static async Task<GetSpawnsetByHash?> HandleAsync(byte[] hash)
	{
		try
		{
			return await AsyncHandler.Client.GetSpawnsetByHash(hash);
		}
		catch
		{
			return null;
		}
	}
}
