namespace DevilDaggersInfo.SourceGen;

public static class StringExtensions
{
	public static string Indent(this string code, int count)
	{
		string indentation = new('\t', count);
		return code.Insert(0, indentation).Replace(Environment.NewLine, Environment.NewLine + indentation);
	}

	public static string TrimStart(this string str, params string[] values)
	{
		if (values.Length == 0)
			return str;

		string? sub = Array.Find(values, v => str.StartsWith(v));
		return sub == null ? str : str.Substring(sub.Length);
	}
}
