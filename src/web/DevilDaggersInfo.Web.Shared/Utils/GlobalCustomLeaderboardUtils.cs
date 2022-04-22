using DevilDaggersInfo.Web.Shared.InternalModels;

namespace DevilDaggersInfo.Web.Shared.Utils;

public static class GlobalCustomLeaderboardUtils
{
	public const int RankingMultiplier = 2;
	public const int LeviathanBonus = 10;
	public const int DevilBonus = 6;
	public const int GoldenBonus = 4;
	public const int SilverBonus = 2;
	public const int BronzeBonus = 1;
	public const int DefaultBonus = 0;

	public static int GetPoints(GlobalCustomLeaderboardEntryData data)
	{
		int points = 0;
		points += data.Rankings.ConvertAll(r => (r.TotalPlayers - (r.Rank - 1)) * RankingMultiplier).Sum();
		points += data.LeviathanCount * LeviathanBonus;
		points += data.DevilCount * DevilBonus;
		points += data.GoldenCount * GoldenBonus;
		points += data.SilverCount * SilverBonus;
		points += data.BronzeCount * BronzeBonus;
		points += data.DefaultCount * DefaultBonus;
		return points;
	}
}
