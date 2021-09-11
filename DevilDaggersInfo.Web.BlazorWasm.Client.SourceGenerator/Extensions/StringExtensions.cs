namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Extensions;

public static class StringExtensions
{
	public static string Indent(this string code, int count)
		=> code.Replace(Environment.NewLine, new string('\t', count));
}
