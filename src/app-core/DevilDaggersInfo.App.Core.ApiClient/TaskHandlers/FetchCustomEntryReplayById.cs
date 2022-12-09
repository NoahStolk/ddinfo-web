using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class FetchCustomEntryReplayById
{
	public static async Task<GetCustomEntryReplayBuffer?> HandleAsync(int customEntryId)
	{
		try
		{
			return await AsyncHandler.Client.GetCustomEntryReplayBufferById(customEntryId);
		}
		catch
		{
			return null;
		}
	}
}
