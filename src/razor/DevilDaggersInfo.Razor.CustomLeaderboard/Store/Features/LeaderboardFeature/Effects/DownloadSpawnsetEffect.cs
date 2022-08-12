using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Effects;

public class DownloadSpawnsetEffect : Effect<DownloadSpawnsetAction>
{
	private readonly NetworkService _networkService;

	public DownloadSpawnsetEffect(NetworkService networkService)
	{
		_networkService = networkService;
	}

	public override async Task HandleAsync(DownloadSpawnsetAction action, IDispatcher dispatcher)
	{
		try
		{
			byte[] spawnset = await _networkService.GetSpawnset(action.SpawnsetId);
			dispatcher.Dispatch(new DownloadSpawnsetSuccessAction(SpawnsetBinary.Parse(spawnset)));
		}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new DownloadSpawnsetFailureAction(ex.Message));
		}
	}
}
