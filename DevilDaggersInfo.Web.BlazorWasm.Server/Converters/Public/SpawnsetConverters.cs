using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.Summary;
using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;

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

	public static GetSpawnsetDataDdse ToGetSpawnsetDataDdse(this SpawnsetSummary spawnsetSummary) => new()
	{
		AdditionalGems = spawnsetSummary.AdditionalGems,
		GameMode = spawnsetSummary.GameMode,
		Hand = (byte)spawnsetSummary.HandLevel,
		LoopLength = spawnsetSummary.LoopSection.Length,
		LoopSpawnCount = spawnsetSummary.LoopSection.SpawnCount,
		NonLoopLength = spawnsetSummary.PreLoopSection.Length,
		NonLoopSpawnCount = spawnsetSummary.PreLoopSection.SpawnCount,
		SpawnVersion = spawnsetSummary.SpawnVersion,
		TimerStart = spawnsetSummary.TimerStart,
		WorldVersion = spawnsetSummary.WorldVersion,
	};

	public static GetSpawnsetOverview ToGetSpawnsetOverview(this SpawnsetEntity spawnset, SpawnsetSummary spawnsetSummary) => new()
	{
		AdditionalGems = spawnsetSummary.AdditionalGems,
		GameMode = spawnsetSummary.GameMode,
		Hand = (byte)spawnsetSummary.HandLevel,
		Id = spawnset.Id,
		LoopLength = spawnsetSummary.LoopSection.Length,
		LoopSpawnCount = spawnsetSummary.LoopSection.SpawnCount,
		PreLoopLength = spawnsetSummary.PreLoopSection.Length,
		PreLoopSpawnCount = spawnsetSummary.PreLoopSection.SpawnCount,
		GameVersion = Spawnset.GetGameVersionString(spawnsetSummary.WorldVersion, spawnsetSummary.SpawnVersion),
		TimerStart = spawnsetSummary.TimerStart,
		AuthorName = spawnset.Player.PlayerName,
		LastUpdated = spawnset.LastUpdated,
		Name = spawnset.Name,
	};

	public static GetSpawnset ToGetSpawnset(this SpawnsetEntity spawnset, byte[] fileBytes) => new()
	{
		AuthorName = spawnset.Player.PlayerName,
		FileBytes = fileBytes,
		Id = spawnset.Id,
		LastUpdated = spawnset.LastUpdated,
		Name = spawnset.Name,
	};
}
