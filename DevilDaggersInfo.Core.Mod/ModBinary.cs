namespace DevilDaggersInfo.Core.Mod;

public class ModBinary
{
	public ModBinary(ModBinaryType modBinaryType, List<ModBinaryChunk> chunks)
	{
		ModBinaryType = modBinaryType;
		Chunks = chunks;
	}

	public ModBinaryType ModBinaryType { get; }
	public List<ModBinaryChunk> Chunks { get; }

	public void ExtractAssets(string outputDirectory, byte[] fileContents)
	{
		foreach (ModBinaryChunk chunk in Chunks)
		{
			byte[] buffer = new byte[chunk.Size];
			Buffer.BlockCopy(fileContents, chunk.Offset, buffer, 0, buffer.Length);
			AssetWriter.WriteFile(outputDirectory, chunk, buffer);
		}
	}
}
