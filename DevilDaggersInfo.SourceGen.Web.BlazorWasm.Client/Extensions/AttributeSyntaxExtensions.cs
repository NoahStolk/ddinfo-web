namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Extensions;

public static class AttributeSyntaxExtensions
{
	public static string GetRouteAttributeStringValue(this AttributeSyntax attribute)
	{
		SeparatedSyntaxList<AttributeArgumentSyntax>? arguments = attribute.ArgumentList?.Arguments;
		if (!arguments.HasValue || arguments.Value.Count == 0)
			return string.Empty;

		return arguments.Value[0].Expression?.ToString().Trim('"') ?? string.Empty;
	}
}
