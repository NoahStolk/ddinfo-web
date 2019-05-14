using System;

namespace DevilDaggersUtilities.Website
{
	public static class LeaderboardHistoryUtils
	{
		public static string HistoryJsonFileNameToDateString(string dateString)
		{
			string year = dateString.Substring(0, 4);
			string month = dateString.Substring(4, 2);
			string day = dateString.Substring(6, 2);
			string hour = dateString.Substring(8, 2);
			string minute = dateString.Substring(10, 2);

			return $"{year}-{month}-{day} {hour}:{minute}";
		}

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