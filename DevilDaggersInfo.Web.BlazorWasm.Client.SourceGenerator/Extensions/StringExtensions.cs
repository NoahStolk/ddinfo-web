namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Extensions;

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

		foreach (string value in values)
		{
			if (str.StartsWith(value))
				return str.Substring(value.Length);
		}

		return str;
	}
}
