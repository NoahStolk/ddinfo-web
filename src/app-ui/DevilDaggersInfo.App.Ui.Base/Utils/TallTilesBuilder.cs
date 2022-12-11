using Warp.NET.Content;

namespace DevilDaggersInfo.App.Ui.Base.Utils;

public static class TallTilesBuilder
{
	private const float _defaultBottomHeight = -2;
	private const float _tileUnit = 4;

	public static Mesh CreateTallTiles(Mesh pillarMesh, float heightMultiplier)
	{
		float newHeight = -heightMultiplier * _tileUnit;

		Vertex[] newVertices = new Vertex[pillarMesh.Vertices.Length];
		for (int i = 0; i < pillarMesh.Vertices.Length; i++)
		{
			Vertex vertex = pillarMesh.Vertices[i];

			Vector3 position;
			if (Math.Abs(vertex.Position.Y - _defaultBottomHeight) < 0.01f)
				position = vertex.Position with { Y = newHeight };
			else
				position = vertex.Position;

			newVertices[i] = new(position, vertex.Texture, vertex.Normal);
		}

		return new(newVertices, pillarMesh.Indices, TriangleRenderMode.Triangles);
	}
}
