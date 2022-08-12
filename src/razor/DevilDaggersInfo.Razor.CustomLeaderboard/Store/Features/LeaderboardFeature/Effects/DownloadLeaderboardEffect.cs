using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature.Actions;
using Fluxor;
using System.Security.Cryptography;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Effects;

public class DownloadLeaderboardEffect : Effect<DownloadSpawnsetSuccessAction>
{
	private readonly NetworkService _networkService;

	public DownloadLeaderboardEffect(NetworkService networkService)
	{
		_networkService = networkService;
	}

	public override async Task HandleAsync(DownloadSpawnsetSuccessAction action, IDispatcher dispatcher)
	{
		byte[] spawnsetBytes = action.Spawnset.ToBytes();

		// TODO: Get path from running process and refactor to platform-specific DI service interface.
		File.WriteAllBytes(@"C:\Program Files (x86)\Steam\steamapps\common\devildaggers\mods\survival", spawnsetBytes);

		byte[] hash = MD5.HashData(spawnsetBytes); // This works when running from Photino.

		try
		{
			GetCustomLeaderboard leaderboard = await _networkService.GetLeaderboard(hash);
			dispatcher.Dispatch(new DownloadLeaderboardSuccessAction(leaderboard));
		}
		catch (Exception ex)
		{
			dispatcher.Dispatch(new DownloadLeaderboardFailureAction(ex.Message));
		}
	}
}
