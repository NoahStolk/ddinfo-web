using DevilDaggersInfo.Core.Asset;

namespace DevilDaggersInfo.Core.Mod;

public record ModBinaryChunk(string Name, int Offset, int Size, AssetType AssetType)
{
	public bool IsLoudness() => AssetType == AssetType.Audio && Name == "loudness";
}
