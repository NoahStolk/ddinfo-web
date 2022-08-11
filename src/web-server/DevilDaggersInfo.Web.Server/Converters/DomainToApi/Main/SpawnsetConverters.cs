using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Types.Core.Spawnsets;
using MainApi = DevilDaggersInfo.Api.Main.Spawnsets;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class SpawnsetConverters
{
	public static MainApi.GetSpawnsetOverview ToGetSpawnsetOverview(this SpawnsetEntity spawnset, SpawnsetSummary spawnsetSummary) => new()
	{
		AdditionalGems = spawnsetSummary.EffectivePlayerSettings.GemsOrHoming,
		GameMode = spawnsetSummary.GameMode.ToMainApi(),
		Hand = spawnsetSummary.EffectivePlayerSettings.HandLevel.ToMainApi(),
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

	private static MainApi.GameMode ToMainApi(this GameMode gameMode) => gameMode switch
	{
		GameMode.Survival => MainApi.GameMode.Survival,
		GameMode.TimeAttack => MainApi.GameMode.TimeAttack,
		GameMode.Race => MainApi.GameMode.Race,
		_ => throw new InvalidEnumConversionException(gameMode),
	};

	public static MainApi.HandLevel ToMainApi(this HandLevel handLevel) => handLevel switch
	{
		HandLevel.Level1 => MainApi.HandLevel.Level1,
		HandLevel.Level2 => MainApi.HandLevel.Level2,
		HandLevel.Level3 => MainApi.HandLevel.Level3,
		HandLevel.Level4 => MainApi.HandLevel.Level4,
		_ => throw new InvalidEnumConversionException(handLevel),
	};
}
