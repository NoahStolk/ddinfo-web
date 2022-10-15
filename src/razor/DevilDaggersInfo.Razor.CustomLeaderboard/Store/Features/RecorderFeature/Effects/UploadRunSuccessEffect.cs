using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Effects;

public class UploadRunSuccessEffect : Effect<UploadRunSuccessAction>
{
	private readonly GameMemoryService _gameMemoryService;

	public UploadRunSuccessEffect(GameMemoryService gameMemoryService)
	{
		_gameMemoryService = gameMemoryService;
	}

	public override async Task HandleAsync(UploadRunSuccessAction action, IDispatcher dispatcher)
	{
		await Task.Yield();
		dispatcher.Dispatch(new DownloadLeaderboardAction(_gameMemoryService.MainBlock.SurvivalHashMd5));
	}
}
