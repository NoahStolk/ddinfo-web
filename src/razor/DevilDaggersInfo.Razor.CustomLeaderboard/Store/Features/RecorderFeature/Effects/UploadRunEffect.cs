using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Effects;

public class UploadRunEffect : Effect<UploadRunAction>
{
	private readonly UploadService _uploadService;
	private readonly GameMemoryReaderService _gameMemoryService;

	public UploadRunEffect(UploadService uploadService, GameMemoryReaderService gameMemoryService)
	{
		_uploadService = uploadService;
		_gameMemoryService = gameMemoryService;
	}

	public override async Task HandleAsync(UploadRunAction action, IDispatcher dispatcher)
	{
		try
		{
			await _uploadService.UploadRun(_gameMemoryService.MainBlock);
			dispatcher.Dispatch(new UploadRunSuccessAction());
		}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new UploadRunFailureAction(ex.Message));
		}
	}
}
