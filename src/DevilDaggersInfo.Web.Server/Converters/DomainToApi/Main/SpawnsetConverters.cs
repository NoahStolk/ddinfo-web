using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Main.Converters.DomainToApi;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

// TODO: Use domain models.
public static class SpawnsetConverters
{
	public static MainApi.GetSpawnsetOverview ToMainApi(this SpawnsetEntity spawnset)
	{
		if (spawnset.Player == null)
			throw new InvalidOperationException("Player is not included.");

		return new MainApi.GetSpawnsetOverview
		{
			AdditionalGems = spawnset.EffectiveGemsOrHoming,
			GameMode = spawnset.GameMode.ToMainApi(),
			Hand = spawnset.EffectiveHandLevel.ToMainApi(),
			Id = spawnset.Id,
			LoopLength = spawnset.LoopLength.HasValue ? GameTime.FromGameUnits(spawnset.LoopLength.Value).Seconds : null,
			LoopSpawnCount = spawnset.LoopSpawnCount,
			PreLoopLength = spawnset.PreLoopLength.HasValue ? GameTime.FromGameUnits(spawnset.PreLoopLength.Value).Seconds : null,
			PreLoopSpawnCount = spawnset.PreLoopSpawnCount,
			AuthorName = spawnset.Player.PlayerName,
			LastUpdated = spawnset.LastUpdated,
			Name = spawnset.Name,
		};
	}

	public static MainApi.GetSpawnset ToMainApi(this SpawnsetEntity spawnset, int? customLeaderboardId, byte[] fileBytes)
	{
		if (spawnset.Player == null)
			throw new InvalidOperationException("Player is not included.");

		return new MainApi.GetSpawnset
		{
			AuthorName = spawnset.Player.PlayerName,
			FileBytes = fileBytes,
			Id = spawnset.Id,
			LastUpdated = spawnset.LastUpdated,
			Name = spawnset.Name,
			CustomLeaderboardId = customLeaderboardId,
			HtmlDescription = spawnset.HtmlDescription,
			MaxDisplayWaves = spawnset.MaxDisplayWaves,
		};
	}

	public static MainApi.HandLevel ToMainApi(this SpawnsetHandLevel handLevel)
	{
		return handLevel switch
		{
			SpawnsetHandLevel.Level1 => MainApi.HandLevel.Level1,
			SpawnsetHandLevel.Level2 => MainApi.HandLevel.Level2,
			SpawnsetHandLevel.Level3 => MainApi.HandLevel.Level3,
			SpawnsetHandLevel.Level4 => MainApi.HandLevel.Level4,
			_ => throw new UnreachableException(),
		};
	}
}
