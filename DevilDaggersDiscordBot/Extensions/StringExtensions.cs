namespace DevilDaggersDiscordBot.Extensions
{
	public static class StringExtensions
	{
		public static string TrimAfter(this string s, int count, bool includeThreePeriods = false)
		{
			if (s.Length <= count)
				return s;

			string subString = s.Substring(0, count);
			return includeThreePeriods ? $"{subString}..." : subString;
		}
	}
}
