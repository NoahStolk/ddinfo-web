using DevilDaggersInfo.Web.BlazorWasm.Shared.InternalModels;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;

public static class GlobalCustomLeaderboardUtils
{
	public const int RankingMultiplier = 2;
	public const int LeviathanMultiplier = 10;
	public const int DevilMultiplier = 6;
	public const int GoldenMultiplier = 4;
	public const int SilverMultiplier = 2;
	public const int BronzeMultiplier = 1;
	public const int DefaultMultiplier = 0;

	public static int GetPoints(GlobalCustomLeaderboardEntryData data)
	{
		int points = 0;
		points += data.Rankings.ConvertAll(r => (r.TotalPlayers - (r.Rank - 1)) * RankingMultiplier).Sum();
		points += data.LeviathanCount * LeviathanMultiplier;
		points += data.DevilCount * DevilMultiplier;
		points += data.GoldenCount * GoldenMultiplier;
		points += data.SilverCount * SilverMultiplier;
		points += data.BronzeCount * BronzeMultiplier;
		points += data.DefaultCount * DefaultMultiplier;
		return points;
	}
}
