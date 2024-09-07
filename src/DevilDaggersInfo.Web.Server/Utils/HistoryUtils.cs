namespace DevilDaggersInfo.Web.Server.Utils;

public static class HistoryUtils
{
	public static DateTime HistoryFileNameToDateTime(string dateString)
	{
		int year = int.Parse(dateString[..4]);
		int month = int.Parse(dateString[4..6]);
		int day = int.Parse(dateString[6..8]);
		int hour = int.Parse(dateString[8..10]);
		int minute = int.Parse(dateString[10..12]);

		return new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc);
	}
}
