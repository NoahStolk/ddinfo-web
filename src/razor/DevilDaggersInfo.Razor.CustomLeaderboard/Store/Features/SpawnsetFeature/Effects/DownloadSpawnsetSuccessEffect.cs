using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature.Actions;
using Fluxor;
using System.Security.Cryptography;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature.Effects;

public class DownloadSpawnsetSuccessEffect : Effect<DownloadSpawnsetSuccessAction>
{
	public override async Task HandleAsync(DownloadSpawnsetSuccessAction action, IDispatcher dispatcher)
	{
		await Task.Yield();

		byte[] spawnsetBytes = action.Spawnset.ToBytes();

		// TODO: Get path from running process and refactor to platform-specific DI service interface.
		File.WriteAllBytes(@"C:\Program Files (x86)\Steam\steamapps\common\devildaggers\mods\survival", spawnsetBytes);

		// MD5 works when running from Photino.
		dispatcher.Dispatch(new DownloadLeaderboardAction(MD5.HashData(spawnsetBytes)));
	}
}
