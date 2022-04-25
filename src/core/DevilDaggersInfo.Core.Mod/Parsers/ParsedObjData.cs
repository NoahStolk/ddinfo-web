namespace DevilDaggersInfo.Core.Mod.Parsers;

public class ParsedObjData
{
	public List<Vector3> Positions { get; } = new();
	public List<Vector2> TexCoords { get; } = new();
	public List<Vector3> Normals { get; } = new();
	public List<VertexReference> Vertices { get; } = new();
}
