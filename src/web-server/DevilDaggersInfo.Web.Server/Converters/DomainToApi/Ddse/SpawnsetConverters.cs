using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.Summary;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DdseApi = DevilDaggersInfo.Api.Ddse.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddse;

// TODO: Use domain models.
public static class SpawnsetConverters
{
	// ! Navigation property.
	public static DdseApi.GetSpawnsetDdse ToDdseApi(this SpawnsetEntity spawnset, SpawnsetSummary spawnsetSummary, bool hasCustomLeaderboard) => new()
	{
		MaxDisplayWaves = spawnset.MaxDisplayWaves,
		HtmlDescription = spawnset.HtmlDescription,
		LastUpdated = spawnset.LastUpdated,
		SpawnsetData = spawnsetSummary.ToDdseApi(),
		Name = spawnset.Name,
		AuthorName = spawnset.Player!.PlayerName,
		HasCustomLeaderboard = hasCustomLeaderboard,
		IsPractice = spawnset.IsPractice,
	};

	private static DdseApi.GetSpawnsetDataDdse ToDdseApi(this SpawnsetSummary spawnsetSummary) => new()
	{
		AdditionalGems = spawnsetSummary.EffectivePlayerSettings.GemsOrHoming,
		GameMode = spawnsetSummary.GameMode.ToDdseApi(),
		Hand = (byte)spawnsetSummary.EffectivePlayerSettings.HandLevel,
		LoopLength = spawnsetSummary.LoopSection.Length,
		LoopSpawnCount = spawnsetSummary.LoopSection.SpawnCount,
		NonLoopLength = spawnsetSummary.PreLoopSection.Length,
		NonLoopSpawnCount = spawnsetSummary.PreLoopSection.SpawnCount,
		SpawnVersion = spawnsetSummary.SpawnVersion,
		TimerStart = spawnsetSummary.TimerStart,
		WorldVersion = spawnsetSummary.WorldVersion,
	};

	private static DdseApi.GameModeDdse ToDdseApi(this GameMode gameMode) => gameMode switch
	{
		GameMode.Survival => DdseApi.GameModeDdse.Survival,
		GameMode.TimeAttack => DdseApi.GameModeDdse.TimeAttack,
		GameMode.Race => DdseApi.GameModeDdse.Race,
		_ => throw new($"Unknown game mode {gameMode}."),
	};
}
