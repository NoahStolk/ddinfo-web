using System;

namespace LeaderboardJsonCreator
{
	public static class Utils
	{
		public static DateTime HistoryJsonFileNameToDateTime(string dateString)
		{
			int year = int.Parse(dateString.Substring(0, 4));
			int month = int.Parse(dateString.Substring(4, 2));
			int day = int.Parse(dateString.Substring(6, 2));
			int hour = int.Parse(dateString.Substring(8, 2));
			int minute = int.Parse(dateString.Substring(10, 2));

			if (dateString.Length == 14)
			{
				int second = int.Parse(dateString.Substring(12, 2));
				return new DateTime(year, month, day, hour, minute, second);
			}

			return new DateTime(year, month, day, hour, minute, 0);
		}
	}
}