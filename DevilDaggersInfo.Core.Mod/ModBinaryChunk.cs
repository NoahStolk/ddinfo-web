namespace DevilDaggersInfo.Core.Mod;

public class ModBinaryChunk
{
	public ModBinaryChunk(string name, int offset, int size, AssetType assetType)
	{
		Name = name;
		Offset = offset;
		Size = size;
		AssetType = assetType;
	}

	public string Name { get; }
	public int Offset { get; }
	public int Size { get; }
	public AssetType AssetType { get; }
}
