namespace DevilDaggersInfo.Core.Mod.FileHandling;

internal sealed class ObjFileHandler : IFileHandler
{
	private static readonly Lazy<ObjFileHandler> _lazy = new(() => new());

	private ObjFileHandler()
	{
	}

	public static ObjFileHandler Instance => _lazy.Value;

	public int HeaderSize => 10;

	public byte[] Compile(byte[] buffer)
	{
		throw new NotImplementedException();
	}

	public byte[] Extract(byte[] buffer)
	{
		using MemoryStream ms = new(buffer);
		using BinaryReader br = new(ms);

		int indexCount = br.ReadInt32();
		int vertexCount = br.ReadInt32();

		_ = br.ReadUInt16();

		Vertex[] vertices = new Vertex[vertexCount];
		int[] indices = new int[indexCount];

		for (int i = 0; i < vertices.Length; i++)
			vertices[i] = br.ReadVertex();

		for (int i = 0; i < indices.Length; i++)
			indices[i] = br.ReadInt32();

		StringBuilder sb = new("# Vertex Attributes\n");

		StringBuilder v = new();
		StringBuilder vt = new();
		StringBuilder vn = new();
		for (int i = 0; i < vertexCount; i++)
		{
			v.Append("v ").Append(vertices[i].Position.X).Append(' ').Append(vertices[i].Position.Y).Append(' ').Append(vertices[i].Position.Z).AppendLine();
			vt.Append("vt ").Append(vertices[i].TexCoord.X).Append(' ').Append(vertices[i].TexCoord.Y).AppendLine();
			vn.Append("vn ").Append(vertices[i].Normal.X).Append(' ').Append(vertices[i].Normal.Y).Append(' ').Append(vertices[i].Normal.Z).AppendLine();
		}

		sb.Append(v);
		sb.Append(vt);
		sb.Append(vn);

		sb.AppendLine("\n# Triangles");
		for (int i = 0; i < indexCount / 3; i++)
		{
			VertexReference vertex1 = new(indices[i * 3] + 1);
			VertexReference vertex2 = new(indices[i * 3 + 1] + 1);
			VertexReference vertex3 = new(indices[i * 3 + 2] + 1);
			sb.Append("f ").Append(vertex1).Append(' ').Append(vertex2).Append(' ').Append(vertex3).AppendLine();
		}

		return Encoding.Default.GetBytes(sb.ToString());
	}
}
