namespace DevilDaggersWebsite.BlazorWasm.Shared
{
	public static class FormatUtils
	{
		public const string LeaderboardGlobalTimeFormat = "N4";
		public const string LeaderboardIntFormat = "N0";
		public const string LeaderboardIntAverageFormat = "N2";
		public const string DateFormat = "yyyy-MM-dd";
		public const string DateTimeUtcFormat = "yyyy-MM-dd HH:mm UTC";
		public const string DateTimeFullFormat = "yyyy-MM-dd HH:mm:ss.fff zzz";
		public const string TimeFormat = "0.0000";
		public const string InGameSensFormat = "0.000";
		public const string GammaFormat = "0.00";
		public const string AccuracyFormat = "0.00%";

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
