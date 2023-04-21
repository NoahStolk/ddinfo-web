using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Warp.NET.SourceGen.Extensions;

internal static class CompilationExtensions
{
	public static List<T> GetTypeDataFromName<T>(this Compilation compilation, IEnumerable<TypeDeclarationSyntax> types, Func<string, T> ctor, CancellationToken cancellationToken)
	{
		List<T> items = new();
		foreach (TypeDeclarationSyntax tds in types)
		{
			cancellationToken.ThrowIfCancellationRequested();

			string? fullTypeName = tds.GetFullTypeName(compilation);
			if (fullTypeName != null)
				items.Add(ctor(fullTypeName));
		}

		return items;
	}
}
