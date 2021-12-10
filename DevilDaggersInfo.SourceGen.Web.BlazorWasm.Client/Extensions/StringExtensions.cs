namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Extensions;

public static class StringExtensions
{
	public static string ToUsingDirective(this string namespaceString)
		=> $"using {namespaceString};";
}
