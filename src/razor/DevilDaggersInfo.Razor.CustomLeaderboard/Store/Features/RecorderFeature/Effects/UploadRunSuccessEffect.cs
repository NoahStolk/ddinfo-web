using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Effects;

public class UploadRunSuccessEffect : Effect<UploadRunSuccessAction>
{
	private readonly NetworkService _networkService;
	private readonly StateFacade _stateFacade;

	public UploadRunSuccessEffect(NetworkService networkService, StateFacade stateFacade)
	{
		_networkService = networkService;
		_stateFacade = stateFacade;
	}

	public override async Task HandleAsync(UploadRunSuccessAction action, IDispatcher dispatcher)
	{
#if TODO
		try
		{
			int spawnsetId = await _networkService.GetSpawnsetIdByHash(ReaderService.MainBlock.SurvivalHashMd5);
			_stateFacade.SetSpawnset(lb.SpawnsetId);

			dispatcher.Dispatch(new SetLeaderboardAction());
		}
		catch (Exception ex)
		{
			// dispatcher.Dispatch(new UploadRunFailureAction(ex.Message));
		}
#endif
	}
}
