using System;

namespace DevilDaggersWebsite.BlazorWasm.Shared
{
	public static class FormatUtils
	{
		public static readonly string LeaderboardTimeFormat = "0.0000";
		public static readonly string LeaderboardTimeLargeFormat = "###,###,###,##0.0000";
		public static readonly string LeaderboardIntFormat = "N0";
		public static readonly string LeaderboardIntAverageFormat = "###,###,###,##0.00";
		public static readonly string DateFormat = "yyyy-MM-dd";
		public static readonly string DateTimeUtcFormat = "yyyy-MM-dd HH:mm UTC";
		public static readonly string DateTimeFullFormat = "yyyy-MM-dd HH:mm:ss.fff zzz";
		public static readonly string SpawnTimeFormat = "0.0000";
		public static readonly string InGameSensFormat = "0.000";
		public static readonly string GammaFormat = "0.00";
		public static readonly string AccuracyFormat = "0.00%";

		// TODO: Improve performance.
		public static string FormatTimeInteger<T>(this T time, bool includeThousandSeparator = false)
			where T : struct // C# does not have a type constraint for integer types.
		{
			string timeStr = time.ToString() ?? "0";
			timeStr = new string('0', Math.Max(0, 5 - timeStr.Length)) + timeStr;
			timeStr = timeStr.Reverse().Insert(4, ".");

			if (includeThousandSeparator)
			{
				for (int i = 8; i < timeStr.Length; i += 4)
					timeStr = timeStr.Insert(i, ",");
			}

			return timeStr.Reverse();
		}

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

		private static string Reverse(this string s)
		{
			char[] charArray = s.ToCharArray();
			Array.Reverse(charArray);

			return new string(charArray);
		}
	}
}
