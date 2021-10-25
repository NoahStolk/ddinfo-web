namespace DevilDaggersInfo.SourceGen.Web.BlazorWasm.Client.Extensions;

public static class SyntaxNodeExtensions
{
	public static string? GetDisplayStringFromContext(this CSharpSyntaxNode syntaxNode, GeneratorSyntaxContext context)
		=> context.SemanticModel.GetTypeInfo(syntaxNode).Type?.ToDisplayString();
}
