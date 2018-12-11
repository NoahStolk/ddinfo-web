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

		public static string[] deaths = { "FALLEN", "SWARMED", "IMPALED", "GORED", "INFESTED", "OPENED", "PURGED", "DESECRATED", "SACRIFICED", "EVISCERATED", "ANNIHILATED", "INTOXICATED", "ENVENMONATED", "INCARNATED", "DISCARNATED", "BARBED" };

		public static int ToDeathType(this string deathName)
		{
			int i = 0;
			foreach (string death in deaths)
			{
				if (death == deathName)
					return i;
				i++;
			}

			return -1;
		}
	}
}