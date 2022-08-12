using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Effects;

public class DownloadLeaderboardEffect : Effect<DownloadLeaderboardAction>
{
	private readonly NetworkService _networkService;

	public DownloadLeaderboardEffect(NetworkService networkService)
	{
		_networkService = networkService;
	}

	public override async Task HandleAsync(DownloadLeaderboardAction action, IDispatcher dispatcher)
	{
		try
		{
			GetCustomLeaderboard leaderboard = await _networkService.GetLeaderboard(action.SpawnsetHash);
			dispatcher.Dispatch(new DownloadLeaderboardSuccessAction(leaderboard));
		}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new DownloadLeaderboardFailureAction(ex.Message));
		}
	}
}
