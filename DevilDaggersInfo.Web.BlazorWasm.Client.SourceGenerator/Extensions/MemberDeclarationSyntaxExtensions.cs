namespace DevilDaggersInfo.Web.BlazorWasm.Client.SourceGenerator.Extensions;

public static class MemberDeclarationSyntaxExtensions
{
	public static AttributeSyntax? GetAttributeFromMember(this MemberDeclarationSyntax member, string attributeName)
	{
		foreach (AttributeListSyntax attributeList in member.AttributeLists)
		{
			foreach (AttributeSyntax attribute in attributeList.Attributes)
			{
				if (attribute.Name.ToString() == attributeName)
					return attribute;
			}
		}

		return null;
	}
}
