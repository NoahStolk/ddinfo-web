namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Extensions;

public static class SyntaxNodeExtensions
{
	public static string? GetDisplayStringFromContext(this CSharpSyntaxNode syntaxNode, GeneratorSyntaxContext context)
		=> context.SemanticModel.GetTypeInfo(syntaxNode).Type?.ToDisplayString();
}
