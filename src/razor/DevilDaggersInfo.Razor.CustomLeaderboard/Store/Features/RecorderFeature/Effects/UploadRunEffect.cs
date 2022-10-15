using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Effects;

public class UploadRunEffect : Effect<UploadRunAction>
{
	private readonly UploadService _uploadService;
	private readonly GameMemoryService _gameMemoryService;

	public UploadRunEffect(UploadService uploadService, GameMemoryService gameMemoryService)
	{
		_uploadService = uploadService;
		_gameMemoryService = gameMemoryService;
	}

	public override async Task HandleAsync(UploadRunAction action, IDispatcher dispatcher)
	{
		try
		{
			GetUploadSuccess uploadSuccess = await _uploadService.UploadRun(_gameMemoryService.MainBlock);
			dispatcher.Dispatch(new UploadRunSuccessAction(uploadSuccess));
		}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new UploadRunFailureAction(ex.Message));
		}
	}
}
