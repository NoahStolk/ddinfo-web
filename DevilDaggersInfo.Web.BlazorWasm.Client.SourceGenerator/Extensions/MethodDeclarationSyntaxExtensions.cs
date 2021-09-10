namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Extensions;

public static class MethodDeclarationSyntaxExtensions
{
	public static string? GetAttributeValueFromMethod(this MethodDeclarationSyntax method, GeneratorSyntaxContext context, string attributeName, int argumentIndex = 0)
	{
		foreach (AttributeListSyntax attributeList in method.AttributeLists)
		{
			foreach (AttributeSyntax attribute in attributeList.Attributes)
			{
				if (attribute.Name.GetDisplayStringFromContext(context) != attributeName)
					continue;

				SeparatedSyntaxList<AttributeArgumentSyntax>? arguments = attribute.ArgumentList?.Arguments;
				if (!arguments.HasValue || arguments.Value.Count < argumentIndex + 1)
					continue;

				ExpressionSyntax? expression = arguments.Value[argumentIndex].Expression;
				if (expression == null)
					continue;

				return expression.GetDisplayStringFromContext(context);
			}
		}

		return null;
	}
}
