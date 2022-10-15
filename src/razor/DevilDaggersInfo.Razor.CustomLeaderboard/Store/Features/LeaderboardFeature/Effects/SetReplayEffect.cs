using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Effects;

public class SetReplayEffect : Effect<SetReplayAction>
{
	private readonly NetworkService _networkService;
	private readonly GameMemoryService _gameMemoryService;

	public SetReplayEffect(NetworkService networkService, GameMemoryService gameMemoryService)
	{
		_networkService = networkService;
		_gameMemoryService = gameMemoryService;
	}

	public override async Task HandleAsync(SetReplayAction action, IDispatcher dispatcher)
	{
		try
		{
			byte[]? replayData = await _networkService.GetReplay(action.CustomEntryId);
			if (replayData != null)
				_gameMemoryService.WriteReplayToMemory(replayData);
		}
		catch
		{
			// Do nothing for now.
		}
	}
}
