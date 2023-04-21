using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Warp.NET.SourceGen.Extensions;

internal static class GeneratorSyntaxContextExtensions
{
	public static TTypeDeclarationSyntax? GetTypeWithAttribute<TTypeDeclarationSyntax>(this GeneratorSyntaxContext context, string attributeFullTypeName)
		where TTypeDeclarationSyntax : TypeDeclarationSyntax
	{
		TTypeDeclarationSyntax declarationSyntax = (TTypeDeclarationSyntax)context.Node;

		foreach (AttributeListSyntax attributeListSyntax in declarationSyntax.AttributeLists)
		{
			foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
			{
				if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
					continue;

				INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
				string fullName = attributeContainingTypeSymbol.ToDisplayString();

				if (fullName == attributeFullTypeName)
					return declarationSyntax;
			}
		}

		return null;
	}
}
