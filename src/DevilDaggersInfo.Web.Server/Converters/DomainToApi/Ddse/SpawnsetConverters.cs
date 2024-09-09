using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using System.Diagnostics;
using DdseApi = DevilDaggersInfo.Web.ApiSpec.Ddse.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddse;

// TODO: Use domain models.
public static class SpawnsetConverters
{
	public static DdseApi.GetSpawnsetDdse ToDdseApi(this SpawnsetEntity spawnset, bool hasCustomLeaderboard)
	{
		// ! Navigation property.
		return new DdseApi.GetSpawnsetDdse
		{
			Id = spawnset.Id,
			MaxDisplayWaves = spawnset.MaxDisplayWaves,
			HtmlDescription = spawnset.HtmlDescription,
			LastUpdated = spawnset.LastUpdated,
			SpawnsetData = new DdseApi.GetSpawnsetDataDdse
			{
				AdditionalGems = spawnset.EffectiveGemsOrHoming,
				GameMode = spawnset.GameMode.ToDdseApi(),
				Hand = (byte)spawnset.EffectiveHandLevel,
				LoopLength = spawnset.LoopLength.HasValue ? (float)GameTime.FromGameUnits(spawnset.LoopLength.Value).Seconds : null,
				LoopSpawnCount = spawnset.LoopSpawnCount,
				NonLoopLength = spawnset.PreLoopLength.HasValue ? (float)GameTime.FromGameUnits(spawnset.PreLoopLength.Value).Seconds : null,
				NonLoopSpawnCount = spawnset.PreLoopSpawnCount,
				SpawnVersion = spawnset.SpawnVersion,
				TimerStart = (float)GameTime.FromGameUnits(spawnset.TimerStart).Seconds,
				WorldVersion = spawnset.WorldVersion,
			},
			Name = spawnset.Name,
			AuthorName = spawnset.Player!.PlayerName,
			HasCustomLeaderboard = hasCustomLeaderboard,
			IsPractice = spawnset.IsPractice,
		};
	}

	private static DdseApi.GameModeDdse ToDdseApi(this SpawnsetGameMode gameMode)
	{
		return gameMode switch
		{
			SpawnsetGameMode.Survival => DdseApi.GameModeDdse.Survival,
			SpawnsetGameMode.TimeAttack => DdseApi.GameModeDdse.TimeAttack,
			SpawnsetGameMode.Race => DdseApi.GameModeDdse.Race,
			_ => throw new UnreachableException(),
		};
	}
}
