namespace DevilDaggersInfo.Core.Mod;

public class ModBinaryChunk
{
	public ModBinaryChunk(string name, uint offset, uint size, AssetType assetType)
	{
		Name = name;
		Offset = offset;
		Size = size;
		AssetType = assetType;
	}

	public string Name { get; }
	public uint Offset { get; }
	public uint Size { get; }
	public AssetType AssetType { get; }
}
