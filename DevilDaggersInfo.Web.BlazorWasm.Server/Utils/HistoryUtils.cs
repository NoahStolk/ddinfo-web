namespace DevilDaggersInfo.Web.BlazorWasm.Server.Utils;

public static class HistoryUtils
{
	public static DateTime HistoryJsonFileNameToDateTime(string dateString)
	{
		int year = int.Parse(dateString[..4]);
		int month = int.Parse(dateString[4..6]);
		int day = int.Parse(dateString[6..8]);
		int hour = int.Parse(dateString[8..10]);
		int minute = int.Parse(dateString[10..12]);

		return new(year, month, day, hour, minute, 0);
	}

	public static string DateTimeToHistoryJsonFileName(DateTime dateTime)
		=> $"{dateTime.Year:0000}{dateTime.Month:00}{dateTime.Day:00}{dateTime.Hour:00}{dateTime.Minute:00}";
}
