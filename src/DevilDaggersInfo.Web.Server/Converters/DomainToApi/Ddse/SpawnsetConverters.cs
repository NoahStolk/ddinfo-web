using DevilDaggersInfo.Core.Common.Extensions;
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
		return new()
		{
			Id = spawnset.Id,
			MaxDisplayWaves = spawnset.MaxDisplayWaves,
			HtmlDescription = spawnset.HtmlDescription,
			LastUpdated = spawnset.LastUpdated,
			SpawnsetData = new()
			{
				AdditionalGems = spawnset.EffectiveGemsOrHoming,
				GameMode = spawnset.GameMode.ToDdseApi(),
				Hand = (byte)spawnset.EffectiveHandLevel,
				LoopLength = (float?)spawnset.LoopLength?.ToSecondsTime(),
				LoopSpawnCount = spawnset.LoopSpawnCount,
				NonLoopLength = (float?)spawnset.PreLoopLength?.ToSecondsTime(),
				NonLoopSpawnCount = spawnset.PreLoopSpawnCount,
				SpawnVersion = spawnset.SpawnVersion,
				TimerStart = (float)spawnset.TimerStart.ToSecondsTime(),
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
