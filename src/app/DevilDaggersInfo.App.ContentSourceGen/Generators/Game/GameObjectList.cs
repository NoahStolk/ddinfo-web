using Warp.NET.SourceGen.Extensions;

namespace Warp.NET.SourceGen.Generators.Game;

public record GameObjectList
{
	public GameObjectList(string fullTypeName)
	{
		FullTypeName = fullTypeName;

		int separatorIndex = FullTypeName.LastIndexOf('.') + 1;
		string typeName = separatorIndex == -1 ? FullTypeName : FullTypeName.Substring(separatorIndex, FullTypeName.Length - separatorIndex);

		PropertyName = $"{typeName}List";
		FieldName = $"_{typeName.FirstCharToLowerCase()}List";
		VariableName = typeName.FirstCharToLowerCase();
	}

	public string FullTypeName { get; }

	public string PropertyName { get; }

	public string FieldName { get; }

	public string VariableName { get; }
}
