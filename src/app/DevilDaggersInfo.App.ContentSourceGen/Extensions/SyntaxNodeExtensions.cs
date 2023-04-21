using Microsoft.CodeAnalysis;

namespace Warp.NET.SourceGen.Extensions;

internal static class SyntaxNodeExtensions
{
	public static string? GetFullTypeName(this SyntaxNode syntaxNode, Compilation compilation)
	{
		SemanticModel semanticModel = compilation.GetSemanticModel(syntaxNode.SyntaxTree);
		if (semanticModel.GetDeclaredSymbol(syntaxNode) is not INamedTypeSymbol symbol)
			return null;

		return symbol.ToString();
	}
}
