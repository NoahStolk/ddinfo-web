namespace DevilDaggersInfo.Core.Extensions;

public static class StringExtensions
{
	public static string TrimStart(this string str, params string[] values)
	{
		if (values.Length == 0)
			return str;

		string? sub = Array.Find(values, v => str.StartsWith(v));
		return sub == null ? str : str[sub.Length..];
	}
}
