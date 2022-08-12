namespace DevilDaggersInfo.Razor.CustomLeaderboard.Utils;

public static class PageUtils
{
	public static int GetTotalPages(int pageSize, int totalResults)
	{
		return (int)Math.Ceiling(totalResults / (double)pageSize);
	}
}
