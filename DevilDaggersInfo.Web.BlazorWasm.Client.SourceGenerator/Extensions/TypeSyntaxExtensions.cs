namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Extensions;

public static class TypeSyntaxExtensions
{
	public static string GetTypeStringForApiHttpClient(this TypeSyntax typeSyntax)
	{
		string typeString = typeSyntax.ToString();
		if (typeString == "ActionResult")
			return "Task";

		while (typeString.StartsWith("ActionResult<") || typeString.StartsWith("Task<"))
			typeString = typeString.RemoveOuterType();

		return typeString;
	}

	private static string RemoveOuterType(this string str)
	{
		int startTypeParameterTokenIndex = str.IndexOf('<');
		int endTypeParameterTokenIndex = str.LastIndexOf('>');

		if (startTypeParameterTokenIndex == -1 || endTypeParameterTokenIndex == -1)
			return str;

		return str.Substring(startTypeParameterTokenIndex + 1, endTypeParameterTokenIndex - startTypeParameterTokenIndex - 1);
	}
}
