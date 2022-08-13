using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature.Actions;
using Fluxor;
using System.Security.Cryptography;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature.Effects;

public class DownloadSpawnsetSuccessEffect : Effect<DownloadSpawnsetSuccessAction>
{
	private readonly GameMemoryReaderService _gameMemoryService;
	private readonly INativeErrorReporter _nativeErrorReporter;

	public DownloadSpawnsetSuccessEffect(GameMemoryReaderService gameMemoryService, INativeErrorReporter nativeErrorReporter)
	{
		_gameMemoryService = gameMemoryService;
		_nativeErrorReporter = nativeErrorReporter;
	}

	public override async Task HandleAsync(DownloadSpawnsetSuccessAction action, IDispatcher dispatcher)
	{
		await Task.Yield();

		byte[] spawnsetBytes = action.Spawnset.ToBytes();

		if (_gameMemoryService.IsInitialized)
		{
			string? pathToSurvival = _gameMemoryService.GetPathToSurvivalFile();
			if (pathToSurvival == null)
				_nativeErrorReporter.ReportError("Can't set survival file", "Could not determine the path to the survival file.");
			else
				File.WriteAllBytes(pathToSurvival, spawnsetBytes);
		}

		// MD5 works when running from Photino.
		dispatcher.Dispatch(new DownloadLeaderboardAction(MD5.HashData(spawnsetBytes)));
	}
}
