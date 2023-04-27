using DevilDaggersInfo.Core.Asset;
using DevilDaggersInfo.Types.Core.Mods;

namespace DevilDaggersInfo.Core.Mod;

/// <summary>
/// Exposes a way of building and compiling a mod binary.
/// </summary>
public class ModBinaryBuilder
{
	private const int _tocEntrySizeWithoutName = 15;

	private readonly List<ModBinaryChunk> _chunks;
	private readonly Dictionary<AssetKey, AssetData> _assetMap;

	public ModBinaryBuilder(ModBinaryType modBinaryType)
	{
		if (modBinaryType is not (ModBinaryType.Audio or ModBinaryType.Dd))
			throw new NotSupportedException($"Creating mods of type '{modBinaryType}' is not supported.");

		Type = modBinaryType;
		_chunks = new();
		_assetMap = new();
	}

	public ModBinaryType Type { get; }

	public IReadOnlyList<ModBinaryChunk> Chunks => _chunks;

	public IReadOnlyDictionary<AssetKey, AssetData> AssetMap => _assetMap;

	public void AddAsset(string assetName, AssetType assetType, byte[] fileContents)
	{
		if (!Type.IsAssetTypeValid(assetType))
			throw new InvalidModBinaryException($"Asset type '{assetType}' is not compatible with binary type '{Type}'.");

		_assetMap.Add(new(assetType, assetName), AssetConverter.Compile(assetType, fileContents));

		_chunks.Clear();

		int offset = ModBinaryConstants.HeaderSize + _tocEntrySizeWithoutName * _assetMap.Count + _assetMap.Sum(kvp => Encoding.UTF8.GetBytes(kvp.Key.AssetName).Length) + sizeof(short);
		foreach (KeyValuePair<AssetKey, AssetData> kvp in _assetMap)
		{
			int size = kvp.Value.Buffer.Length;
			_chunks.Add(new(kvp.Key.AssetName, offset, size, kvp.Key.AssetType));

			offset += size;
		}

		// TODO: Loudness.
	}

	public byte[] Compile()
	{
		int tocBufferSize = _tocEntrySizeWithoutName * _assetMap.Count + _chunks.Sum(c => Encoding.UTF8.GetBytes(c.Name).Length) + sizeof(short);
		int offset = ModBinaryConstants.HeaderSize + tocBufferSize;
		byte[]? tocBuffer;
		using (MemoryStream tocStream = new())
		{
			using BinaryWriter tocWriter = new(tocStream);
			foreach ((AssetKey key, AssetData data) in _assetMap)
			{
				tocWriter.Write((ushort)key.AssetType);

				tocWriter.Write(Encoding.UTF8.GetBytes(key.AssetName));
				tocWriter.Write((byte)0);

				int size = data.Buffer.Length;

				tocWriter.Write(offset);
				tocWriter.Write(size);
				tocWriter.Write(0);

				offset += size;
			}

			tocWriter.Write((short)0);

			tocBuffer = tocStream.ToArray();
		}

		if (tocBuffer.Length != tocBufferSize)
			throw new InvalidOperationException($"Invalid TOC buffer size: {tocBuffer.Length}. Expected length was {tocBufferSize}.");

		List<AssetData> uniqueAssets = _assetMap.Select(ad => ad.Value).Distinct().ToList();
		byte[]? assetBuffer;
		using (MemoryStream assetStream = new())
		{
			using BinaryWriter assetWriter = new(assetStream);
			foreach (AssetData assetData in uniqueAssets)
				assetWriter.Write(assetData.Buffer);

			assetBuffer = assetStream.ToArray();
		}

		using MemoryStream ms = new();
		ms.Write(BitConverter.GetBytes(ModBinaryConstants.Identifier));
		ms.Write(BitConverter.GetBytes((uint)tocBuffer.Length));
		ms.Write(tocBuffer);
		ms.Write(assetBuffer);
		return ms.ToArray();
	}
}
