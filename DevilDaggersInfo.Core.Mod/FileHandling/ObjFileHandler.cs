namespace DevilDaggersInfo.Core.Mod.FileHandling;

public sealed class ObjFileHandler : IFileHandler
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
		int indexCount = BitConverter.ToInt32(buffer, 0);
		int vertexCount = BitConverter.ToInt32(buffer, 4);

		Vertex[] vertices = new Vertex[vertexCount];
		int[] indices = new int[indexCount];

		for (int i = 0; i < vertices.Length; i++)
			vertices[i] = Vertex.CreateFromBuffer(buffer, HeaderSize, i);

		for (int i = 0; i < indices.Length; i++)
			indices[i] = BitConverter.ToInt32(buffer, HeaderSize + vertices.Length * Vertex.ByteCount + i * sizeof(int));

		StringBuilder sb = new("# Vertex Attributes\n");

		StringBuilder v = new();
		StringBuilder vt = new();
		StringBuilder vn = new();
		for (int i = 0; i < vertexCount; ++i)
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

	private struct Vertex
	{
		public const int ByteCount = 32;

		public Vertex(Vector3 position, Vector2 texCoord, Vector3 normal)
		{
			Position = position;
			TexCoord = texCoord;
			Normal = normal;
		}

		public Vector3 Position { get; private set; }
		public Vector2 TexCoord { get; private set; }
		public Vector3 Normal { get; private set; }

		public byte[] ToByteArray()
		{
			// TexCoord and Normal are swapped in the binary format for some reason.
			byte[] bytes = new byte[32];
			Buffer.BlockCopy(BitConverter.GetBytes(Position.X), 0, bytes, 0, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(Position.Y), 0, bytes, 4, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(Position.Z), 0, bytes, 8, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(Normal.X), 0, bytes, 12, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(Normal.Y), 0, bytes, 16, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(Normal.Z), 0, bytes, 20, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(TexCoord.X), 0, bytes, 24, sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(TexCoord.Y), 0, bytes, 28, sizeof(float));
			return bytes;
		}

		public static Vertex CreateFromBuffer(byte[] buffer, int offset, int vertexIndex)
		{
			Vector3 position = new(
				x: BitConverter.ToSingle(buffer, offset + vertexIndex * ByteCount),
				y: BitConverter.ToSingle(buffer, offset + vertexIndex * ByteCount + 4),
				z: BitConverter.ToSingle(buffer, offset + vertexIndex * ByteCount + 8));
			Vector2 texCoord = new(
				x: BitConverter.ToSingle(buffer, offset + vertexIndex * ByteCount + 24),
				y: BitConverter.ToSingle(buffer, offset + vertexIndex * ByteCount + 28));
			Vector3 normal = new(
				x: BitConverter.ToSingle(buffer, offset + vertexIndex * ByteCount + 12),
				y: BitConverter.ToSingle(buffer, offset + vertexIndex * ByteCount + 16),
				z: BitConverter.ToSingle(buffer, offset + vertexIndex * ByteCount + 20));
			return new(position, texCoord, normal);
		}

		public void RoundValues(int decimals)
		{
			Position = new Vector3(
				(float)Math.Round((decimal)Position.X, decimals, MidpointRounding.AwayFromZero),
				(float)Math.Round((decimal)Position.Y, decimals, MidpointRounding.AwayFromZero),
				(float)Math.Round((decimal)Position.Z, decimals, MidpointRounding.AwayFromZero));

			TexCoord = new Vector2(
				(float)Math.Round((decimal)TexCoord.X, decimals, MidpointRounding.AwayFromZero),
				(float)Math.Round((decimal)TexCoord.Y, decimals, MidpointRounding.AwayFromZero));

			Normal = new Vector3(
				(float)Math.Round((decimal)Normal.X, decimals, MidpointRounding.AwayFromZero),
				(float)Math.Round((decimal)Normal.Y, decimals, MidpointRounding.AwayFromZero),
				(float)Math.Round((decimal)Normal.Z, decimals, MidpointRounding.AwayFromZero));
		}
	}

	private struct VertexReference
	{
		public VertexReference(int positionReference, int texCoordReference, int normalReference)
		{
			PositionReference = positionReference;
			TexCoordReference = texCoordReference;
			NormalReference = normalReference;
		}

		public VertexReference(int unifiedReference)
		{
			PositionReference = unifiedReference;
			TexCoordReference = unifiedReference;
			NormalReference = unifiedReference;
		}

		public int PositionReference { get; }
		public int TexCoordReference { get; }
		public int NormalReference { get; }

		public override string ToString()
			=> $"{PositionReference}/{TexCoordReference}/{NormalReference}";
	}
}
