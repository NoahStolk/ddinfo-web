namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class StringExtensions
{
	public static string TrimAfter(this string s, int count, bool appendThreePeriods = false)
	{
		if (s.Length <= count)
			return s;

		string subString = s[..count];
		return appendThreePeriods ? $"{subString}..." : subString;
	}
}
