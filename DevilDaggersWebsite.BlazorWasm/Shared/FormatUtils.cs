namespace DevilDaggersWebsite.BlazorWasm.Shared
{
	public static class FormatUtils
	{
		public static readonly string LeaderboardGlobalTimeFormat = "N4";
		public static readonly string LeaderboardIntFormat = "N0";
		public static readonly string LeaderboardIntAverageFormat = "N2";
		public static readonly string DateFormat = "yyyy-MM-dd";
		public static readonly string DateTimeUtcFormat = "yyyy-MM-dd HH:mm UTC";
		public static readonly string DateTimeFullFormat = "yyyy-MM-dd HH:mm:ss.fff zzz";
		public static readonly string TimeFormat = "0.0000";
		public static readonly string InGameSensFormat = "0.000";
		public static readonly string GammaFormat = "0.00";
		public static readonly string AccuracyFormat = "0.00%";

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
	}
}
