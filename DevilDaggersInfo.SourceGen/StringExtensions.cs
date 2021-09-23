namespace DevilDaggersInfo.SourceGen;

public static class StringExtensions
{
	public static string Indent(this string code, int count)
	{
		string indentation = new('\t', count);
		return code.Insert(0, indentation).Replace(Environment.NewLine, Environment.NewLine + indentation);
	}
}
