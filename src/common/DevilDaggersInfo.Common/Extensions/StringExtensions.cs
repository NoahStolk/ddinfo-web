namespace DevilDaggersInfo.Common.Extensions;

public static class StringExtensions
{
	// TODO: Move to web server redirects.
	public static string TrimStart(this string str, params string[] values)
	{
		if (values.Length == 0)
			return str;

		string? sub = Array.Find(values, str.StartsWith);
		return sub == null ? str : str[sub.Length..];
	}
}
