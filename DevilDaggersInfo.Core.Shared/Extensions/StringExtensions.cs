namespace DevilDaggersInfo.Core.Shared.Extensions;

public static class StringExtensions
{
	public static string TrimStart(this string str, params string[] values)
	{
		if (values.Length == 0)
			return str;

		foreach (string value in values)
		{
			if (str.StartsWith(value))
				return str[value.Length..];
		}

		return str;
	}
}
