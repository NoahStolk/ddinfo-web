using DevilDaggersInfo.Types.Core.Assets;

namespace DevilDaggersInfo.Core.Mod;

public class ModBinary
{
	private readonly ModBinaryReadFilter _readFilter;

	public ModBinary(byte[] fileContents, ModBinaryReadFilter readFilter)
	{
		_readFilter = readFilter;

		Toc = ModBinaryToc.FromBytes(fileContents);

		using MemoryStream ms = new(fileContents);
		using BinaryReader br = new(ms);

		AssetMap = new();

		// If chunks point to the same asset position; the asset is re-used (TODO: test if this even works in DD -- if not, remove DistinctBy).
		foreach (ModBinaryChunk chunk in Toc.Chunks.DistinctBy(c => c.Offset))
		{
			if (!_readFilter.ShouldRead(new(chunk.AssetType, chunk.Name)))
				continue;

			br.BaseStream.Seek(chunk.Offset, SeekOrigin.Begin);
			byte[] buffer = br.ReadBytes(chunk.Size);

			AssetMap[new(chunk.AssetType, chunk.Name)] = new(buffer);
		}
	}

	public ModBinaryToc Toc { get; }
	public Dictionary<AssetKey, AssetData> AssetMap { get; }

	public byte[] ExtractAsset(AssetKey assetKey)
		=> ExtractAsset(assetKey.AssetName, assetKey.AssetType);

	public byte[] ExtractAsset(string assetName, AssetType assetType)
	{
		if (_readFilter.ShouldRead(new(assetType, assetName)))
			throw new InvalidOperationException("This asset has not been read. It was not included in the filter, so it cannot be extracted.");

		AssetKey key = new(assetType, assetName);
		if (!AssetMap.ContainsKey(key))
			throw new InvalidOperationException($"Mod binary does not contain an asset of type '{assetType}' with name '{assetName}'.");

		return AssetConverter.Extract(assetType, AssetMap[key]);
	}
}
