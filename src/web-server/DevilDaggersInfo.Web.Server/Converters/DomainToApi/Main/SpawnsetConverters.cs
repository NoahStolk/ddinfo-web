using MainApi = DevilDaggersInfo.Api.Main.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class SpawnsetConverters
{
	public static MainApi.GetSpawnsetOverview ToGetSpawnsetOverview(this SpawnsetEntity spawnset, SpawnsetSummary spawnsetSummary) => new()
	{
		AdditionalGems = spawnsetSummary.EffectivePlayerSettings.GemsOrHoming,
		GameMode = spawnsetSummary.GameMode,
		Hand = spawnsetSummary.EffectivePlayerSettings.HandLevel,
		Id = spawnset.Id,
		LoopLength = spawnsetSummary.LoopSection.Length,
		LoopSpawnCount = spawnsetSummary.LoopSection.SpawnCount,
		PreLoopLength = spawnsetSummary.PreLoopSection.Length,
		PreLoopSpawnCount = spawnsetSummary.PreLoopSection.SpawnCount,
		AuthorName = spawnset.Player.PlayerName,
		LastUpdated = spawnset.LastUpdated,
		Name = spawnset.Name,
	};

	public static MainApi.GetSpawnset ToGetSpawnset(this SpawnsetEntity spawnset, int? customLeaderboardId, byte[] fileBytes) => new()
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
