namespace DevilDaggersInfo.Web.Shared.Utils;

public static class FormatUtils
{
	public static string FormatDaggersInt32(int hit, int fired, bool isHistory = false)
		=> FormatDaggersUInt64((ulong)hit, (ulong)fired, isHistory);

	public static string FormatDaggersUInt64(ulong hit, ulong fired, bool isHistory = false)
	{
		if (isHistory)
		{
			if (hit == 0)
				return "No data";
			if (fired == 10000)
				return "Exact values not known";
		}

		return $"{hit:N0} / {fired:N0}";
	}

	public static string FormatDateTimeAsTimeAgo(DateTime? dateTime)
		=> dateTime.HasValue ? FormatDateTimeAsTimeAgo(dateTime.Value) : "Never";

	public static string FormatDateTimeAsTimeAgo(DateTime dateTime)
	{
		TimeSpan difference = DateTime.UtcNow - dateTime;

		if (difference.TotalMinutes < 1)
			return "Less than 1 minute ago";

		if (difference.TotalHours < 1)
		{
			int minutes = (int)difference.TotalMinutes;
			return $"{minutes} minute{S(minutes)} ago";
		}

		if (difference.TotalDays < 1)
		{
			int hours = (int)difference.TotalHours;
			return $"{hours} hour{S(hours)} ago";
		}

		int days = (int)difference.TotalDays;
		return $"{days} day{S(days)} ago";

		static string S(int n) => n == 1 ? string.Empty : "s";
	}
}
