namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class CheckIfLeaderboardExists
{
	public static async Task<bool?> HandleAsync(byte[] survivalHash)
	{
		try
		{
			HttpResponseMessage hrm = await AsyncHandler.DdclClient.CustomLeaderboardExistsBySpawnsetHash(survivalHash);
			return hrm.IsSuccessStatusCode;
		}
		catch
		{
			return null;
		}
	}
}
