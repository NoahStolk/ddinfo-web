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

	public string Name { get; private set; }
	public int Offset { get; }
	public int Size { get; }
	public AssetType AssetType { get; }

	public bool IsLoudness() => AssetType == AssetType.Audio && Name == "loudness";

	public string Enable() => Name = Name.ToLower();

	public string Disable() => Name = Name.ToUpper();
}
