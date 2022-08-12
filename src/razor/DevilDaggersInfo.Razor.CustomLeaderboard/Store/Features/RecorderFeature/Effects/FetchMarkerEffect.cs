using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Effects;

public class FetchMarkerEffect : Effect<FetchMarkerAction>
{
	private readonly NetworkService _networkService;
	private readonly IClientConfiguration _clientConfiguration;
	private readonly GameMemoryReaderService _gameMemoryService;

	public FetchMarkerEffect(NetworkService networkService, IClientConfiguration clientConfiguration, GameMemoryReaderService gameMemoryService)
	{
		_networkService = networkService;
		_clientConfiguration = clientConfiguration;
		_gameMemoryService = gameMemoryService;
	}

	public override async Task HandleAsync(FetchMarkerAction action, IDispatcher dispatcher)
	{
		try
		{
			long marker = await _networkService.GetMarker(_clientConfiguration.GetOperatingSystem());
			_gameMemoryService.Initialize(marker);
			dispatcher.Dispatch(new FetchMarkerSuccessAction(marker));
		}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new FetchMarkerFailureAction(ex.Message));
		}
	}
}
