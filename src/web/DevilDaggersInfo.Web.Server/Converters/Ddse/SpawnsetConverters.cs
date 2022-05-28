using DevilDaggersInfo.Web.Shared.Dto.Ddse.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.Ddse;

public static class SpawnsetConverters
{
	public static GetSpawnsetDdse ToGetSpawnsetDdse(this SpawnsetEntity spawnset, SpawnsetSummary spawnsetSummary, bool hasCustomLeaderboard) => new()
	{
		MaxDisplayWaves = spawnset.MaxDisplayWaves,
		HtmlDescription = spawnset.HtmlDescription,
		LastUpdated = spawnset.LastUpdated,
		SpawnsetData = spawnsetSummary.ToGetSpawnsetDataDdse(),
		Name = spawnset.Name,
		AuthorName = spawnset.Player.PlayerName,
		HasCustomLeaderboard = hasCustomLeaderboard,
		IsPractice = spawnset.IsPractice,
	};

	private static GetSpawnsetDataDdse ToGetSpawnsetDataDdse(this SpawnsetSummary spawnsetSummary) => new()
	{
		AdditionalGems = spawnsetSummary.EffectivePlayerSettings.GemsOrHoming,
		GameMode = spawnsetSummary.GameMode,
		Hand = (byte)spawnsetSummary.EffectivePlayerSettings.HandLevel,
		LoopLength = spawnsetSummary.LoopSection.Length,
		LoopSpawnCount = spawnsetSummary.LoopSection.SpawnCount,
		NonLoopLength = spawnsetSummary.PreLoopSection.Length,
		NonLoopSpawnCount = spawnsetSummary.PreLoopSection.SpawnCount,
		SpawnVersion = spawnsetSummary.SpawnVersion,
		TimerStart = spawnsetSummary.TimerStart,
		WorldVersion = spawnsetSummary.WorldVersion,
	};
}
