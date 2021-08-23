namespace DevilDaggersInfo.Core.Mod;

public static class AssetWriter
{
	public static void WriteWav(string outputDirectory, ModBinaryChunk chunk, byte[] buffer)
		=> File.WriteAllBytes(Path.Combine(outputDirectory, $"{chunk.Name}.wav"), buffer);
}
