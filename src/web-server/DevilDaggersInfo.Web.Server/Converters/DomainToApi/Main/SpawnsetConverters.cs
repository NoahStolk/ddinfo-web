using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Api.Main.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

// TODO: Use domain models.
public static class SpawnsetConverters
{
	public static MainApi.GetSpawnsetOverview ToMainApi(this SpawnsetEntity spawnset)
	{
		if (spawnset.Player == null)
			throw new InvalidOperationException("Player is not included.");

		return new()
		{
			AdditionalGems = spawnset.EffectiveGemsOrHoming,
			GameMode = spawnset.GameMode.ToMainApi(),
			Hand = spawnset.EffectiveHandLevel.ToMainApi(),
			Id = spawnset.Id,
			LoopLength = spawnset.LoopLength,
			LoopSpawnCount = spawnset.LoopSpawnCount,
			PreLoopLength = spawnset.PreLoopLength,
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

		return new()
		{
			AuthorName = spawnset.Player.PlayerName,
			FileBytes = fileBytes,
			Id = spawnset.Id,
			IsPractice = spawnset.IsPractice,
			LastUpdated = spawnset.LastUpdated,
			Name = spawnset.Name,
			CustomLeaderboardId = customLeaderboardId,
			HtmlDescription = spawnset.HtmlDescription,
			MaxDisplayWaves = spawnset.MaxDisplayWaves,
		};
	}

	private static MainApi.GameMode ToMainApi(this SpawnsetGameMode gameMode) => gameMode switch
	{
		SpawnsetGameMode.Survival => MainApi.GameMode.Survival,
		SpawnsetGameMode.TimeAttack => MainApi.GameMode.TimeAttack,
		SpawnsetGameMode.Race => MainApi.GameMode.Race,
		_ => throw new UnreachableException(),
	};

	public static DevilDaggersInfo.Api.Main.Spawnsets.HandLevel ToMainApi(this SpawnsetHandLevel handLevel) => handLevel switch
	{
		SpawnsetHandLevel.Level1 => Api.Main.Spawnsets.HandLevel.Level1,
		SpawnsetHandLevel.Level2 => Api.Main.Spawnsets.HandLevel.Level2,
		SpawnsetHandLevel.Level3 => Api.Main.Spawnsets.HandLevel.Level3,
		SpawnsetHandLevel.Level4 => Api.Main.Spawnsets.HandLevel.Level4,
		_ => throw new UnreachableException(),
	};
}
