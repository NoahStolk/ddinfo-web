using ToolsShared;

namespace LeaderboardJsonHighscoreStatsSpreader
{
	public static class Program
	{
		public static void Main()
		{
			HighscoreSpreadUtils.SpreadAllHighscoreStats(false, true);
		}
	}
}
