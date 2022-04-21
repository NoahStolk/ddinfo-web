namespace DevilDaggersInfo.Core.Mod.Structs;

public struct Vertex
{
	public Vertex(Vector3 position, Vector3 normal, Vector2 texCoord)
	{
		Position = position;
		Normal = normal;
		TexCoord = texCoord;
	}

	public Vector3 Position { get; }
	public Vector3 Normal { get; }
	public Vector2 TexCoord { get; }
}
