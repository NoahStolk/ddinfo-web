using DevilDaggersInfo.Api.Ddcl;
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class FetchCustomLeaderboards
{
	public static async Task<Page<GetCustomLeaderboardForOverview>?> HandleAsync(CustomLeaderboardCategory category, int pageIndex, int pageSize, int selectedPlayerId, bool onlyFeatured)
	{
		try
		{
			return await AsyncHandler.DdclClient.GetCustomLeaderboardOverview(category, pageIndex, pageSize, selectedPlayerId, onlyFeatured);
		}
		catch
		{
			return null;
		}
	}
}
