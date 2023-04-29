using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using System.Diagnostics;
using DdseApi = DevilDaggersInfo.Api.Ddse.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddse;

// TODO: Use domain models.
public static class SpawnsetConverters
{
	// ! Navigation property.
	public static DdseApi.GetSpawnsetDdse ToDdseApi(this SpawnsetEntity spawnset, bool hasCustomLeaderboard) => new()
	{
		MaxDisplayWaves = spawnset.MaxDisplayWaves,
		HtmlDescription = spawnset.HtmlDescription,
		LastUpdated = spawnset.LastUpdated,
		SpawnsetData = new()
		{
			AdditionalGems = spawnset.EffectiveGemsOrHoming,
			GameMode = spawnset.GameMode.ToDdseApi(),
			Hand = (byte)spawnset.EffectiveHandLevel,
			LoopLength = spawnset.LoopLength,
			LoopSpawnCount = spawnset.LoopSpawnCount,
			NonLoopLength = spawnset.PreLoopLength,
			NonLoopSpawnCount = spawnset.PreLoopSpawnCount,
			SpawnVersion = spawnset.SpawnVersion,
			TimerStart = spawnset.TimerStart,
			WorldVersion = spawnset.WorldVersion,
		},
		Name = spawnset.Name,
		AuthorName = spawnset.Player!.PlayerName,
		HasCustomLeaderboard = hasCustomLeaderboard,
		IsPractice = spawnset.IsPractice,
	};

	private static DdseApi.GameModeDdse ToDdseApi(this SpawnsetGameMode gameMode) => gameMode switch
	{
		SpawnsetGameMode.Survival => DdseApi.GameModeDdse.Survival,
		SpawnsetGameMode.TimeAttack => DdseApi.GameModeDdse.TimeAttack,
		SpawnsetGameMode.Race => DdseApi.GameModeDdse.Race,
		_ => throw new UnreachableException(),
	};
}
