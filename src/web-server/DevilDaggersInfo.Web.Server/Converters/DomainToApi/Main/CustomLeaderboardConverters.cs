using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.CriteriaExpression;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Api.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class CustomLeaderboardConverters
{
	public static MainApi.GetCustomLeaderboardOverview ToMainApi(this CustomLeaderboardOverview customLeaderboard) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetAuthorName = customLeaderboard.SpawnsetAuthorName,
		SpawnsetName = customLeaderboard.SpawnsetName,
		Daggers = customLeaderboard.Daggers?.ToMainApi(),
		IsFeatured = customLeaderboard.Daggers != null,
		DateCreated = customLeaderboard.DateCreated,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		PlayerCount = customLeaderboard.PlayerCount,
		TopPlayer = customLeaderboard.WorldRecord?.PlayerName,
		WorldRecord = customLeaderboard.WorldRecord?.Time.ToSecondsTime(),
		WorldRecordDagger = customLeaderboard.WorldRecord?.Dagger?.ToMainApi(),
	};

	public static MainApi.GetCustomLeaderboard ToMainApi(this SortedCustomLeaderboard customLeaderboard) => new()
	{
		SpawnsetId = customLeaderboard.SpawnsetId,
		SpawnsetAuthorName = customLeaderboard.SpawnsetAuthorName,
		SpawnsetHtmlDescription = customLeaderboard.SpawnsetHtmlDescription,
		SpawnsetName = customLeaderboard.SpawnsetName,
		Daggers = customLeaderboard.Daggers?.ToMainApi(),
		DateCreated = customLeaderboard.DateCreated,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		RankSorting = customLeaderboard.RankSorting.ToMainApi(),
		IsFeatured = customLeaderboard.Daggers != null,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		CustomEntries = customLeaderboard.CustomEntries.ConvertAll(ce => ce.ToMainApi(customLeaderboard.GameMode)),
		Criteria = customLeaderboard.Criteria.ConvertAll(clc => clc.ToMainApi()),
		SpawnsetGameMode = customLeaderboard.GameMode.ToMainApi(),
	};

	private static MainApi.GetCustomEntry ToMainApi(this CustomEntry customEntry, SpawnsetGameMode gameMode) => new()
	{
		Id = customEntry.Id,
		Rank = customEntry.Rank,
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.PlayerName,
		CountryCode = customEntry.CountryCode,
		Client = customEntry.Client.ToMainApi(),
		ClientVersion = customEntry.ClientVersion,
		DeathType = gameMode is SpawnsetGameMode.TimeAttack or SpawnsetGameMode.Race ? null : customEntry.DeathType,
		EnemiesAlive = customEntry.EnemiesAlive,
		GemsCollected = customEntry.GemsCollected,
		GemsDespawned = customEntry.GemsDespawned,
		GemsEaten = customEntry.GemsEaten,
		HomingStored = customEntry.HomingStored,
		HomingEaten = customEntry.HomingEaten,
		EnemiesKilled = customEntry.EnemiesKilled,
		LevelUpTime2 = customEntry.LevelUpTime2.ToSecondsTime(),
		LevelUpTime3 = customEntry.LevelUpTime3.ToSecondsTime(),
		LevelUpTime4 = customEntry.LevelUpTime4.ToSecondsTime(),
		DaggersFired = customEntry.DaggersFired,
		DaggersHit = customEntry.DaggersHit,
		SubmitDate = customEntry.SubmitDate,
		Time = customEntry.Time.ToSecondsTime(),
		CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger?.ToMainApi(),
		HasGraphs = customEntry.HasGraphs,
	};

	private static MainApi.GetCustomLeaderboardDaggers ToMainApi(this CustomLeaderboardDaggers customLeaderboard) => new()
	{
		Bronze = customLeaderboard.Bronze.ToSecondsTime(),
		Silver = customLeaderboard.Silver.ToSecondsTime(),
		Golden = customLeaderboard.Golden.ToSecondsTime(),
		Devil = customLeaderboard.Devil.ToSecondsTime(),
		Leviathan = customLeaderboard.Leviathan.ToSecondsTime(),
	};

	private static MainApi.GetCustomLeaderboardCriteria ToMainApi(this CustomLeaderboardCriteria criteria) => new()
	{
		Type = criteria.Type.ToMainApi(),
		Operator = criteria.Operator.ToMainApi(),
		Expression = criteria.Expression,
	};

	private static MainApi.CustomLeaderboardsClient ToMainApi(this CustomLeaderboardsClient client) => client switch
	{
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => MainApi.CustomLeaderboardsClient.DevilDaggersCustomLeaderboards,
		CustomLeaderboardsClient.DdstatsRust => MainApi.CustomLeaderboardsClient.DdstatsRust,
		CustomLeaderboardsClient.DdinfoTools => MainApi.CustomLeaderboardsClient.DdinfoTools,
		_ => throw new ArgumentOutOfRangeException(nameof(client), client, null),
	};

	public static MainApi.CustomLeaderboardRankSorting ToMainApi(this CustomLeaderboardRankSorting rankSorting) => rankSorting switch
	{
		CustomLeaderboardRankSorting.TimeDesc => MainApi.CustomLeaderboardRankSorting.TimeDesc,
		CustomLeaderboardRankSorting.TimeAsc => MainApi.CustomLeaderboardRankSorting.TimeAsc,
		_ => throw new UnreachableException(),
	};

	private static MainApi.CustomLeaderboardDagger ToMainApi(this CustomLeaderboardDagger dagger) => dagger switch
	{
		CustomLeaderboardDagger.Default => MainApi.CustomLeaderboardDagger.Default,
		CustomLeaderboardDagger.Bronze => MainApi.CustomLeaderboardDagger.Bronze,
		CustomLeaderboardDagger.Silver => MainApi.CustomLeaderboardDagger.Silver,
		CustomLeaderboardDagger.Golden => MainApi.CustomLeaderboardDagger.Golden,
		CustomLeaderboardDagger.Devil => MainApi.CustomLeaderboardDagger.Devil,
		CustomLeaderboardDagger.Leviathan => MainApi.CustomLeaderboardDagger.Leviathan,
		_ => throw new UnreachableException(),
	};

	private static MainApi.CustomLeaderboardCriteriaType ToMainApi(this CustomLeaderboardCriteriaType criteriaType) => criteriaType switch
	{
		CustomLeaderboardCriteriaType.GemsCollected => MainApi.CustomLeaderboardCriteriaType.GemsCollected,
		CustomLeaderboardCriteriaType.GemsDespawned => MainApi.CustomLeaderboardCriteriaType.GemsDespawned,
		CustomLeaderboardCriteriaType.GemsEaten => MainApi.CustomLeaderboardCriteriaType.GemsEaten,
		CustomLeaderboardCriteriaType.EnemiesKilled => MainApi.CustomLeaderboardCriteriaType.EnemiesKilled,
		CustomLeaderboardCriteriaType.DaggersFired => MainApi.CustomLeaderboardCriteriaType.DaggersFired,
		CustomLeaderboardCriteriaType.DaggersHit => MainApi.CustomLeaderboardCriteriaType.DaggersHit,
		CustomLeaderboardCriteriaType.HomingStored => MainApi.CustomLeaderboardCriteriaType.HomingStored,
		CustomLeaderboardCriteriaType.HomingEaten => MainApi.CustomLeaderboardCriteriaType.HomingEaten,
		CustomLeaderboardCriteriaType.Skull1Kills => MainApi.CustomLeaderboardCriteriaType.Skull1Kills,
		CustomLeaderboardCriteriaType.Skull2Kills => MainApi.CustomLeaderboardCriteriaType.Skull2Kills,
		CustomLeaderboardCriteriaType.Skull3Kills => MainApi.CustomLeaderboardCriteriaType.Skull3Kills,
		CustomLeaderboardCriteriaType.Skull4Kills => MainApi.CustomLeaderboardCriteriaType.Skull4Kills,
		CustomLeaderboardCriteriaType.SpiderlingKills => MainApi.CustomLeaderboardCriteriaType.SpiderlingKills,
		CustomLeaderboardCriteriaType.SpiderEggKills => MainApi.CustomLeaderboardCriteriaType.SpiderEggKills,
		CustomLeaderboardCriteriaType.Squid1Kills => MainApi.CustomLeaderboardCriteriaType.Squid1Kills,
		CustomLeaderboardCriteriaType.Squid2Kills => MainApi.CustomLeaderboardCriteriaType.Squid2Kills,
		CustomLeaderboardCriteriaType.Squid3Kills => MainApi.CustomLeaderboardCriteriaType.Squid3Kills,
		CustomLeaderboardCriteriaType.CentipedeKills => MainApi.CustomLeaderboardCriteriaType.CentipedeKills,
		CustomLeaderboardCriteriaType.GigapedeKills => MainApi.CustomLeaderboardCriteriaType.GigapedeKills,
		CustomLeaderboardCriteriaType.GhostpedeKills => MainApi.CustomLeaderboardCriteriaType.GhostpedeKills,
		CustomLeaderboardCriteriaType.Spider1Kills => MainApi.CustomLeaderboardCriteriaType.Spider1Kills,
		CustomLeaderboardCriteriaType.Spider2Kills => MainApi.CustomLeaderboardCriteriaType.Spider2Kills,
		CustomLeaderboardCriteriaType.LeviathanKills => MainApi.CustomLeaderboardCriteriaType.LeviathanKills,
		CustomLeaderboardCriteriaType.OrbKills => MainApi.CustomLeaderboardCriteriaType.OrbKills,
		CustomLeaderboardCriteriaType.ThornKills => MainApi.CustomLeaderboardCriteriaType.ThornKills,
		CustomLeaderboardCriteriaType.Skull1sAlive => MainApi.CustomLeaderboardCriteriaType.Skull1sAlive,
		CustomLeaderboardCriteriaType.Skull2sAlive => MainApi.CustomLeaderboardCriteriaType.Skull2sAlive,
		CustomLeaderboardCriteriaType.Skull3sAlive => MainApi.CustomLeaderboardCriteriaType.Skull3sAlive,
		CustomLeaderboardCriteriaType.Skull4sAlive => MainApi.CustomLeaderboardCriteriaType.Skull4sAlive,
		CustomLeaderboardCriteriaType.SpiderlingsAlive => MainApi.CustomLeaderboardCriteriaType.SpiderlingsAlive,
		CustomLeaderboardCriteriaType.SpiderEggsAlive => MainApi.CustomLeaderboardCriteriaType.SpiderEggsAlive,
		CustomLeaderboardCriteriaType.Squid1sAlive => MainApi.CustomLeaderboardCriteriaType.Squid1sAlive,
		CustomLeaderboardCriteriaType.Squid2sAlive => MainApi.CustomLeaderboardCriteriaType.Squid2sAlive,
		CustomLeaderboardCriteriaType.Squid3sAlive => MainApi.CustomLeaderboardCriteriaType.Squid3sAlive,
		CustomLeaderboardCriteriaType.CentipedesAlive => MainApi.CustomLeaderboardCriteriaType.CentipedesAlive,
		CustomLeaderboardCriteriaType.GigapedesAlive => MainApi.CustomLeaderboardCriteriaType.GigapedesAlive,
		CustomLeaderboardCriteriaType.GhostpedesAlive => MainApi.CustomLeaderboardCriteriaType.GhostpedesAlive,
		CustomLeaderboardCriteriaType.Spider1sAlive => MainApi.CustomLeaderboardCriteriaType.Spider1sAlive,
		CustomLeaderboardCriteriaType.Spider2sAlive => MainApi.CustomLeaderboardCriteriaType.Spider2sAlive,
		CustomLeaderboardCriteriaType.LeviathansAlive => MainApi.CustomLeaderboardCriteriaType.LeviathansAlive,
		CustomLeaderboardCriteriaType.OrbsAlive => MainApi.CustomLeaderboardCriteriaType.OrbsAlive,
		CustomLeaderboardCriteriaType.ThornsAlive => MainApi.CustomLeaderboardCriteriaType.ThornsAlive,
		CustomLeaderboardCriteriaType.DeathType => MainApi.CustomLeaderboardCriteriaType.DeathType,
		CustomLeaderboardCriteriaType.Time => MainApi.CustomLeaderboardCriteriaType.Time,
		CustomLeaderboardCriteriaType.LevelUpTime2 => MainApi.CustomLeaderboardCriteriaType.LevelUpTime2,
		CustomLeaderboardCriteriaType.LevelUpTime3 => MainApi.CustomLeaderboardCriteriaType.LevelUpTime3,
		CustomLeaderboardCriteriaType.LevelUpTime4 => MainApi.CustomLeaderboardCriteriaType.LevelUpTime4,
		CustomLeaderboardCriteriaType.EnemiesAlive => MainApi.CustomLeaderboardCriteriaType.EnemiesAlive,
		_ => throw new UnreachableException(),
	};

	private static MainApi.CustomLeaderboardCriteriaOperator ToMainApi(this CustomLeaderboardCriteriaOperator @operator) => @operator switch
	{
		CustomLeaderboardCriteriaOperator.Any => MainApi.CustomLeaderboardCriteriaOperator.Any,
		CustomLeaderboardCriteriaOperator.Equal => MainApi.CustomLeaderboardCriteriaOperator.Equal,
		CustomLeaderboardCriteriaOperator.LessThan => MainApi.CustomLeaderboardCriteriaOperator.LessThan,
		CustomLeaderboardCriteriaOperator.GreaterThan => MainApi.CustomLeaderboardCriteriaOperator.GreaterThan,
		CustomLeaderboardCriteriaOperator.LessThanOrEqual => MainApi.CustomLeaderboardCriteriaOperator.LessThanOrEqual,
		CustomLeaderboardCriteriaOperator.GreaterThanOrEqual => MainApi.CustomLeaderboardCriteriaOperator.GreaterThanOrEqual,
		CustomLeaderboardCriteriaOperator.Modulo => MainApi.CustomLeaderboardCriteriaOperator.Modulo,
		CustomLeaderboardCriteriaOperator.NotEqual => MainApi.CustomLeaderboardCriteriaOperator.NotEqual,
		_ => throw new UnreachableException(),
	};

	// TODO: Use domain models?
	public static MainApi.GetCustomEntryData ToMainApi(this CustomEntryEntity customEntry, CustomEntryDataEntity? customEntryData, SpawnsetHandLevel startingLevel, bool hasReplay)
	{
		if (customEntry.Player == null)
			throw new InvalidOperationException("Player is not included.");

		if (customEntry.CustomLeaderboard == null)
			throw new InvalidOperationException("Custom leaderboard is not included.");

		if (customEntry.CustomLeaderboard.Spawnset == null)
			throw new InvalidOperationException("Custom leaderboard spawnset is not included.");

		return new()
		{
			CustomEntryId = customEntry.Id,
			PlayerId = customEntry.PlayerId,
			PlayerName = customEntry.Player.PlayerName,
			SpawnsetName = customEntry.CustomLeaderboard.Spawnset.Name,
			ClientVersion = customEntry.ClientVersion,
			DeathType = customEntry.DeathType,
			EnemiesAlive = customEntry.EnemiesAlive,
			GemsCollected = customEntry.GemsCollected,
			GemsDespawned = customEntry.GemsDespawned,
			GemsEaten = customEntry.GemsEaten,
			GemsTotal = customEntry.GemsTotal,
			HomingStored = customEntry.HomingStored,
			HomingEaten = customEntry.HomingEaten,
			EnemiesKilled = customEntry.EnemiesKilled,
			LevelUpTime2 = customEntry.LevelUpTime2.ToSecondsTime(),
			LevelUpTime3 = customEntry.LevelUpTime3.ToSecondsTime(),
			LevelUpTime4 = customEntry.LevelUpTime4.ToSecondsTime(),
			DaggersFired = customEntry.DaggersFired,
			DaggersHit = customEntry.DaggersHit,
			SubmitDate = customEntry.SubmitDate,
			Time = customEntry.Time.ToSecondsTime(),
			CustomLeaderboardDagger = customEntry.CustomLeaderboard.DaggerFromStat(customEntry.Time)?.ToMainApi(),

			GemsCollectedData = GetInt32Arr(customEntryData?.GemsCollectedData),
			EnemiesKilledData = GetInt32Arr(customEntryData?.EnemiesKilledData),
			DaggersFiredData = GetInt32Arr(customEntryData?.DaggersFiredData),
			DaggersHitData = GetInt32Arr(customEntryData?.DaggersHitData),
			EnemiesAliveData = GetInt32Arr(customEntryData?.EnemiesAliveData),
			HomingStoredData = GetInt32Arr(customEntryData?.HomingStoredData),
			HomingEatenData = GetInt32Arr(customEntryData?.HomingEatenData),
			GemsDespawnedData = GetInt32Arr(customEntryData?.GemsDespawnedData),
			GemsEatenData = GetInt32Arr(customEntryData?.GemsEatenData),
			GemsTotalData = GetInt32Arr(customEntryData?.GemsTotalData),

			Skull1sAliveData = GetUInt16Arr(customEntryData?.Skull1sAliveData),
			Skull2sAliveData = GetUInt16Arr(customEntryData?.Skull2sAliveData),
			Skull3sAliveData = GetUInt16Arr(customEntryData?.Skull3sAliveData),
			SpiderlingsAliveData = GetUInt16Arr(customEntryData?.SpiderlingsAliveData),
			Skull4sAliveData = GetUInt16Arr(customEntryData?.Skull4sAliveData),
			Squid1sAliveData = GetUInt16Arr(customEntryData?.Squid1sAliveData),
			Squid2sAliveData = GetUInt16Arr(customEntryData?.Squid2sAliveData),
			Squid3sAliveData = GetUInt16Arr(customEntryData?.Squid3sAliveData),
			CentipedesAliveData = GetUInt16Arr(customEntryData?.CentipedesAliveData),
			GigapedesAliveData = GetUInt16Arr(customEntryData?.GigapedesAliveData),
			Spider1sAliveData = GetUInt16Arr(customEntryData?.Spider1sAliveData),
			Spider2sAliveData = GetUInt16Arr(customEntryData?.Spider2sAliveData),
			LeviathansAliveData = GetUInt16Arr(customEntryData?.LeviathansAliveData),
			OrbsAliveData = GetUInt16Arr(customEntryData?.OrbsAliveData),
			ThornsAliveData = GetUInt16Arr(customEntryData?.ThornsAliveData),
			GhostpedesAliveData = GetUInt16Arr(customEntryData?.GhostpedesAliveData),
			SpiderEggsAliveData = GetUInt16Arr(customEntryData?.SpiderEggsAliveData),

			Skull1sKilledData = GetUInt16Arr(customEntryData?.Skull1sKilledData),
			Skull2sKilledData = GetUInt16Arr(customEntryData?.Skull2sKilledData),
			Skull3sKilledData = GetUInt16Arr(customEntryData?.Skull3sKilledData),
			SpiderlingsKilledData = GetUInt16Arr(customEntryData?.SpiderlingsKilledData),
			Skull4sKilledData = GetUInt16Arr(customEntryData?.Skull4sKilledData),
			Squid1sKilledData = GetUInt16Arr(customEntryData?.Squid1sKilledData),
			Squid2sKilledData = GetUInt16Arr(customEntryData?.Squid2sKilledData),
			Squid3sKilledData = GetUInt16Arr(customEntryData?.Squid3sKilledData),
			CentipedesKilledData = GetUInt16Arr(customEntryData?.CentipedesKilledData),
			GigapedesKilledData = GetUInt16Arr(customEntryData?.GigapedesKilledData),
			Spider1sKilledData = GetUInt16Arr(customEntryData?.Spider1sKilledData),
			Spider2sKilledData = GetUInt16Arr(customEntryData?.Spider2sKilledData),
			LeviathansKilledData = GetUInt16Arr(customEntryData?.LeviathansKilledData),
			OrbsKilledData = GetUInt16Arr(customEntryData?.OrbsKilledData),
			ThornsKilledData = GetUInt16Arr(customEntryData?.ThornsKilledData),
			GhostpedesKilledData = GetUInt16Arr(customEntryData?.GhostpedesKilledData),
			SpiderEggsKilledData = GetUInt16Arr(customEntryData?.SpiderEggsKilledData),

			StartingLevel = startingLevel.ToMainApi(),
			HasReplay = hasReplay,
		};

		static int[]? GetInt32Arr(byte[]? bytes)
			=> bytes == null || bytes.Length == 0 ? null : IntegerArrayCompressor.ExtractData(bytes);

		static ushort[]? GetUInt16Arr(byte[]? bytes)
			=> bytes == null || bytes.Length == 0 ? null : Array.ConvertAll(IntegerArrayCompressor.ExtractData(bytes), i => (ushort)i);
	}
}
