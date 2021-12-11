namespace DevilDaggersInfo.Core.Extensions;

public static class StringExtensions
{
	public static string ReplaceLastOccurrenceOf(this string source, string find, string replace)
	{
		int position = source.LastIndexOf(find);
		if (position == -1)
			return source;

		return source.Remove(position, find.Length).Insert(position, replace);
	}

	public static string TrimStart(this string str, params string[] values)
	{
		if (values.Length == 0)
			return str;

		string? sub = Array.Find(values, v => str.StartsWith(v));
		return sub == null ? str : str[sub.Length..];
	}
}
