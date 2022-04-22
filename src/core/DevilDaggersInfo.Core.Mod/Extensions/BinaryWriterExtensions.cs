namespace DevilDaggersInfo.Core.Mod.Extensions;

public static class BinaryWriterExtensions
{
	public static void WriteVertex(this BinaryWriter bw, Vertex vertex)
	{
		bw.Write(vertex.Position.X);
		bw.Write(vertex.Position.Y);
		bw.Write(vertex.Position.Z);
		bw.Write(vertex.Normal.X);
		bw.Write(vertex.Normal.Y);
		bw.Write(vertex.Normal.Z);
		bw.Write(vertex.TexCoord.X);
		bw.Write(vertex.TexCoord.Y);
	}
}
