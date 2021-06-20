namespace DevilDaggersDiscordBot.Extensions
{
	public static class StringExtensions
	{
		public static string TrimAfter(this string s, int count)
			=> s.Length > count ? s.Substring(0, count) : s;
	}
}
