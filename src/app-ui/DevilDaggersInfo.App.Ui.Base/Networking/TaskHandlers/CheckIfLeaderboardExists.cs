namespace DevilDaggersInfo.App.Ui.Base.Networking.TaskHandlers;

public static class CheckIfLeaderboardExists
{
	public static async Task<bool?> HandleAsync(byte[] survivalHash)
	{
		try
		{
			HttpResponseMessage hrm = await AsyncHandler.Client.CustomLeaderboardExistsBySpawnsetHash(survivalHash);
			return hrm.IsSuccessStatusCode;
		}
		catch
		{
			return null;
		}
	}
}
