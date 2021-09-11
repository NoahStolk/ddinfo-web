namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Extensions;

public static class TypeSyntaxExtensions
{
	public static string GetTypeStringWithoutActionResult(this TypeSyntax typeSyntax)
	{
		string typeString = typeSyntax.ToString();

		if (!typeString.StartsWith("ActionResult<"))
			return typeString;

		string trimStart = typeString.TrimStart("ActionResult<");
		return trimStart.Substring(0, trimStart.Length - 1);
	}
}
