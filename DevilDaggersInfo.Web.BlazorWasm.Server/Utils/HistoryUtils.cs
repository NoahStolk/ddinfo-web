using System;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Utils
{
	public static class HistoryUtils
	{
		public static DateTime HistoryJsonFileNameToDateTime(string dateString)
		{
			int year = int.Parse(dateString.Substring(0, 4));
			int month = int.Parse(dateString.Substring(4, 2));
			int day = int.Parse(dateString.Substring(6, 2));
			int hour = int.Parse(dateString.Substring(8, 2));
			int minute = int.Parse(dateString.Substring(10, 2));

			return new(year, month, day, hour, minute, 0);
		}

		public static string DateTimeToHistoryJsonFileName(DateTime dateTime)
			=> $"{dateTime.Year:0000}{dateTime.Month:00}{dateTime.Day:00}{dateTime.Hour:00}{dateTime.Minute:00}";
	}
}
