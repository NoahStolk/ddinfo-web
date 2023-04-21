using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Warp.NET.SourceGen.Extensions;

internal static class MemberDeclarationSyntaxExtensions
{
	public static AttributeSyntax? GetAttributeFromMember(this MemberDeclarationSyntax member, string fullAttributeName)
	{
		foreach (AttributeListSyntax attributeList in member.AttributeLists)
		{
			foreach (AttributeSyntax attribute in attributeList.Attributes)
			{
				string name = attribute.Name.ToString();
				if (name == fullAttributeName || $"{name}Attribute" == fullAttributeName)
					return attribute;
			}
		}

		return null;
	}
}
