using DevilDaggersInfo.Core.Spawnset.Summary;
using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;

public static class SpawnsetConverters
{
	public static GetSpawnset ToGetSpawnsetPublic(this SpawnsetEntity spawnset, SpawnsetSummary spawnsetSummary, bool hasCustomLeaderboard) => new()
	{
		MaxDisplayWaves = spawnset.MaxDisplayWaves,
		HtmlDescription = spawnset.HtmlDescription,
		LastUpdated = spawnset.LastUpdated,
		SpawnsetData = spawnsetSummary.ToGetSpawnsetDataPublic(),
		Name = spawnset.Name,
		AuthorName = spawnset.Player.PlayerName,
		HasCustomLeaderboard = hasCustomLeaderboard,
		IsPractice = spawnset.IsPractice,
	};

	public static GetSpawnsetData ToGetSpawnsetDataPublic(this SpawnsetSummary spawnsetSummary) => new()
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
}
