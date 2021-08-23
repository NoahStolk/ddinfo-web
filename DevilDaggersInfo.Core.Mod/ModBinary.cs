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
}
