using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Core.Spawnset;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class EnumDisplayStringExtensions
{
	public static string ToDisplayString(this CustomLeaderboardsClient client) => client switch
	{
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => "DDCL",
		CustomLeaderboardsClient.DdstatsRust => "ddstats-rust",
		CustomLeaderboardsClient.DdinfoTools => "DDINFO",
		_ => throw new UnreachableException(),
	};

	public static string ToDisplayString(this CustomLeaderboardRankSorting rankSorting) => rankSorting switch
	{
		CustomLeaderboardRankSorting.TimeDesc => "Highest Time",
		CustomLeaderboardRankSorting.TimeAsc => "Lowest Time",
		CustomLeaderboardRankSorting.GemsCollectedDesc => "Most Gems",
		CustomLeaderboardRankSorting.GemsCollectedAsc => "Least Gems",
		CustomLeaderboardRankSorting.GemsDespawnedDesc => "Most Gems Despawned",
		CustomLeaderboardRankSorting.GemsDespawnedAsc => "Least Gems Despawned",
		CustomLeaderboardRankSorting.GemsEatenDesc => "Most Gems Eaten",
		CustomLeaderboardRankSorting.GemsEatenAsc => "Least Gems Eaten",
		CustomLeaderboardRankSorting.EnemiesKilledDesc => "Most Kills",
		CustomLeaderboardRankSorting.EnemiesKilledAsc => "Least Kills",
		CustomLeaderboardRankSorting.EnemiesAliveDesc => "Most Enemies Alive",
		CustomLeaderboardRankSorting.EnemiesAliveAsc => "Least Enemies Alive",
		CustomLeaderboardRankSorting.HomingStoredDesc => "Most Homing",
		CustomLeaderboardRankSorting.HomingStoredAsc => "Least Homing",
		CustomLeaderboardRankSorting.HomingEatenDesc => "Most Homing Eaten",
		CustomLeaderboardRankSorting.HomingEatenAsc	=> "Least Homing Eaten",
		_ => throw new UnreachableException(),
	};

	public static string ToDisplayString(this GameMode gameMode) => gameMode switch
	{
		GameMode.Survival => "Survival",
		GameMode.TimeAttack => "Time Attack",
		GameMode.Race => "Race",
		_ => throw new UnreachableException(),
	};

	public static GameMode ToCore(this Api.Main.Spawnsets.GameMode gameMode) => gameMode switch
	{
		Api.Main.Spawnsets.GameMode.Survival => GameMode.Survival,
		Api.Main.Spawnsets.GameMode.TimeAttack => GameMode.TimeAttack,
		Api.Main.Spawnsets.GameMode.Race => GameMode.Race,
		_ => throw new UnreachableException(),
	};
}
