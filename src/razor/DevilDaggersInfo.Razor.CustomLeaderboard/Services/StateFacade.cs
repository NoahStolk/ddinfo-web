using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardListFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;
using System.Security.Cryptography;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;
	private readonly IState<LeaderboardState> _leaderboardState;
	private readonly IState<SpawnsetState> _spawnsetState;
	private readonly GameMemoryReaderService _gameMemoryReaderService;

	public StateFacade(IDispatcher dispatcher, IState<LeaderboardState> leaderboardState, IState<SpawnsetState> spawnsetState, GameMemoryReaderService gameMemoryReaderService)
	{
		_dispatcher = dispatcher;
		_leaderboardState = leaderboardState;
		_spawnsetState = spawnsetState;
		_gameMemoryReaderService = gameMemoryReaderService;
	}

	public void LoadLeaderboards()
	{
		_dispatcher.Dispatch(new FetchLeaderboardsAction());
	}

	public void SetPageIndex(int pageIndex)
	{
		_dispatcher.Dispatch(new SetPageIndexAction(pageIndex));
		_dispatcher.Dispatch(new FetchLeaderboardsAction());
	}

	public void SetLeaderboard(int spawnsetId)
	{
		_dispatcher.Dispatch(new DownloadSpawnsetAction(spawnsetId));

		if (_spawnsetState.Value.Spawnset == null)
			return;

		byte[] spawnsetBytes = _spawnsetState.Value.Spawnset.ToBytes();

		// TODO: Get path from running process and refactor to platform-specific DI service interface.
		File.WriteAllBytes(@"C:\Program Files (x86)\Steam\steamapps\common\devildaggers\mods\survival", spawnsetBytes);

		byte[] hash = MD5.HashData(spawnsetBytes); // TODO: Find different way to hash.
		_dispatcher.Dispatch(new DownloadLeaderboardAction(hash));
	}

	public void SetReplay(int customEntryId)
	{
		//byte[]? replayData = await NetworkService.GetReplay(customEntryId);
		//if (replayData != null)
		//	_gameMemoryReaderService.WriteReplayToMemory(replayData);
	}
}
