using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class FetchCustomLeaderboardById
{
	public static async Task<GetCustomLeaderboard?> HandleAsync(int customLeaderboardId)
	{
		try
		{
			return await AsyncHandler.Client.GetCustomLeaderboardById(customLeaderboardId);
		}
		catch
		{
			return null;
		}
	}
}