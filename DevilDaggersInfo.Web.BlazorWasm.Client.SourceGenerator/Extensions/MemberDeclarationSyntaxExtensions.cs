namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Extensions;

public static class MemberDeclarationSyntaxExtensions
{
	public static AttributeSyntax? GetAttributeFromMember(this MemberDeclarationSyntax member, GeneratorSyntaxContext context, string attributeName)
	{
		foreach (AttributeListSyntax attributeList in member.AttributeLists)
		{
			foreach (AttributeSyntax attribute in attributeList.Attributes)
			{
				if (attribute.Name.GetDisplayStringFromContext(context) == attributeName)
					return attribute;
			}
		}

		return null;
	}
}
