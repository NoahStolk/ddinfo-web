namespace DevilDaggersInfo.App.ContentSourceGen.Generators.GameObject;

public class GameObject
{
	public GameObject(string fullTypeName, List<GameObjectInterpolationState> states, List<GameObjectChild> children)
	{
		FullTypeName = fullTypeName;
		States = states;
		Children = children;

		int separatorIndex = FullTypeName.LastIndexOf('.');
		if (separatorIndex == -1)
			throw new InvalidOperationException($"Invalid full type name, at least one '.' character was expected: {fullTypeName}");

		Namespace = FullTypeName.Substring(0, separatorIndex);
		TypeName = FullTypeName.Substring(separatorIndex + 1, FullTypeName.Length - (separatorIndex + 1));
	}

	public string FullTypeName { get; }
	public List<GameObjectInterpolationState> States { get; }
	public List<GameObjectChild> Children { get; }

	public string Namespace { get; }
	public string TypeName { get; }
}
