using DevilDaggersInfo.Core.Mod.Extensions;
using Warp.NET.Content;
using DdVertex = DevilDaggersInfo.Core.Mod.Structs.Vertex;

namespace DevilDaggersInfo.App.Core.AssetInterop;

public static class MeshConverter
{
	// TODO: Move duplicate code to Core.Mod.
	public static Mesh ToWarpMesh(byte[] ddMeshBuffer)
	{
		using MemoryStream ms = new(ddMeshBuffer);
		using BinaryReader br = new(ms);

		int indexCount = br.ReadInt32();
		int vertexCount = br.ReadInt32();
		_ = br.ReadUInt16();

		DdVertex[] ddVertices = new DdVertex[vertexCount];
		for (int i = 0; i < ddVertices.Length; i++)
			ddVertices[i] = br.ReadVertex();

		uint[] indices = new uint[indexCount];
		for (int i = 0; i < indices.Length; i++)
			indices[i] = br.ReadUInt32();

		Vertex[] warpVertices = new Vertex[vertexCount];
		for (int i = 0; i < ddVertices.Length; i++)
		{
			DdVertex ddVertex = ddVertices[i];
			warpVertices[i] = new(ddVertex.Position, ddVertex.TexCoord, ddVertex.Normal);
		}

		return new(warpVertices, indices);
	}
}
