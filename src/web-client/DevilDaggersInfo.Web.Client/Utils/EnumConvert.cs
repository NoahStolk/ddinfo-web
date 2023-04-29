using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Api.Main.Spawnsets;

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
		_ => CustomLeaderboardRankSorting.TimeDesc,
	};
}
