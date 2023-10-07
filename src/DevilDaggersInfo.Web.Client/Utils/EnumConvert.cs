using DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;
using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

namespace DevilDaggersInfo.Web.Client.Utils;

public static class EnumConvert
{
	public static GameMode GetGameMode(string str) => str switch
	{
		nameof(GameMode.TimeAttack) => GameMode.TimeAttack,
		nameof(GameMode.Race) => GameMode.Race,
		_ => GameMode.Survival,
	};

	public static CustomLeaderboardRankSorting GetRankSorting(string str) => str switch
	{
		nameof(CustomLeaderboardRankSorting.TimeAsc) => CustomLeaderboardRankSorting.TimeAsc,
		nameof(CustomLeaderboardRankSorting.GemsCollectedDesc) => CustomLeaderboardRankSorting.GemsCollectedDesc,
		nameof(CustomLeaderboardRankSorting.GemsCollectedAsc) => CustomLeaderboardRankSorting.GemsCollectedAsc,
		nameof(CustomLeaderboardRankSorting.GemsDespawnedDesc) => CustomLeaderboardRankSorting.GemsDespawnedDesc,
		nameof(CustomLeaderboardRankSorting.GemsDespawnedAsc) => CustomLeaderboardRankSorting.GemsDespawnedAsc,
		nameof(CustomLeaderboardRankSorting.GemsEatenDesc) => CustomLeaderboardRankSorting.GemsEatenDesc,
		nameof(CustomLeaderboardRankSorting.GemsEatenAsc) => CustomLeaderboardRankSorting.GemsEatenAsc,
		nameof(CustomLeaderboardRankSorting.EnemiesKilledDesc) => CustomLeaderboardRankSorting.EnemiesKilledDesc,
		nameof(CustomLeaderboardRankSorting.EnemiesKilledAsc) => CustomLeaderboardRankSorting.EnemiesKilledAsc,
		nameof(CustomLeaderboardRankSorting.EnemiesAliveDesc) => CustomLeaderboardRankSorting.EnemiesAliveDesc,
		nameof(CustomLeaderboardRankSorting.EnemiesAliveAsc) => CustomLeaderboardRankSorting.EnemiesAliveAsc,
		nameof(CustomLeaderboardRankSorting.HomingStoredDesc) => CustomLeaderboardRankSorting.HomingStoredDesc,
		nameof(CustomLeaderboardRankSorting.HomingStoredAsc) => CustomLeaderboardRankSorting.HomingStoredAsc,
		nameof(CustomLeaderboardRankSorting.HomingEatenDesc) => CustomLeaderboardRankSorting.HomingEatenDesc,
		nameof(CustomLeaderboardRankSorting.HomingEatenAsc) => CustomLeaderboardRankSorting.HomingEatenAsc,
		_ => CustomLeaderboardRankSorting.TimeDesc,
	};
}
