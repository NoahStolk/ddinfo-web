using DevilDaggersInfo.Types.Core.Assets;
using DevilDaggersInfo.Types.Core.Mods;

namespace DevilDaggersInfo.Core.Mod;

public class ModBinary
{
	private const long _fileIdentifier = 0x013A67723A78683A;
	private const int _fileHeaderSize = 12;

	private readonly ModBinaryReadComprehensiveness _readComprehensiveness;

	// TODO: Split class in three parts and remove ModBinaryReadComprehensiveness.
	public ModBinary(byte[] fileContents, ModBinaryReadComprehensiveness readComprehensiveness)
	{
		if (fileContents.Length < _fileHeaderSize)
			throw new InvalidModBinaryException($"Invalid binary; must be at least {_fileHeaderSize} bytes in length.");

		using MemoryStream ms = new(fileContents);
		using BinaryReader br = new(ms);
		ulong fileIdentifier = br.ReadUInt64();
		if (fileIdentifier != _fileIdentifier)
			throw new InvalidModBinaryException("Invalid binary; incorrect header values.");

		uint tocSize = br.ReadUInt32();
		if (tocSize > fileContents.Length - _fileHeaderSize)
			throw new InvalidModBinaryException("Invalid binary; TOC size is larger than the remaining amount of bytes.");

		// Read TOC into chunks.
		ModBinaryType? modBinaryType = null;
		List<ModBinaryChunk> chunks = new();
		while (br.BaseStream.Position < _fileHeaderSize + tocSize)
		{
			ushort type = br.ReadUInt16();
			if (type == 0)
			{
				// Break the loop when the end of the TOC is reached (which is 0x0000).
				break;
			}

			AssetType? assetType = type.GetAssetType();
			string name = br.ReadNullTerminatedString();
			int offset = br.ReadInt32();
			int size = br.ReadInt32();
			_ = br.ReadInt32();

			// Skip invalid chunks (present in default dd binary).
			if (size <= 0 || offset < _fileHeaderSize + tocSize)
				continue;

			// Skip unknown or obsolete types (such as 0x11, which is an outdated type for (fragment?) shaders).
			if (!assetType.HasValue)
				continue;

			chunks.Add(new(name, offset, size, assetType.Value));

			if (!modBinaryType.HasValue)
			{
				modBinaryType = assetType == AssetType.Audio ? ModBinaryType.Audio : ModBinaryType.Dd;
				if (readComprehensiveness == ModBinaryReadComprehensiveness.TypeOnly)
					break;
			}
			else if (!IsAssetTypeValid(modBinaryType.Value, assetType.Value))
			{
				throw new InvalidModBinaryException($"Asset type '{assetType}' is not compatible with binary type '{modBinaryType}'.");
			}
		}

		// Read assets.
		AssetMap = new();
		if (readComprehensiveness == ModBinaryReadComprehensiveness.All)
		{
			// If chunks point to the same asset position; the asset is re-used (TODO: test if this even works in DD -- if not, remove DistinctBy).
			foreach (ModBinaryChunk chunk in chunks.DistinctBy(c => c.Offset))
			{
				br.BaseStream.Seek(chunk.Offset, SeekOrigin.Begin);
				byte[] buffer = br.ReadBytes(chunk.Size);

				AssetMap[new(chunk.AssetType, chunk.Name)] = new(buffer);
			}
		}
		else if (readComprehensiveness == ModBinaryReadComprehensiveness.TocAndLoudness)
		{
			ModBinaryChunk? loudnessChunk = chunks.Find(c => c.IsLoudness());
			if (loudnessChunk != null)
			{
				br.BaseStream.Seek(loudnessChunk.Offset, SeekOrigin.Begin);
				byte[] buffer = br.ReadBytes(loudnessChunk.Size);

				AssetMap[new(loudnessChunk.AssetType, loudnessChunk.Name)] = new(buffer);
			}
		}

		ModBinaryType = modBinaryType ?? throw new InvalidModBinaryException("Could not automatically determine binary type (most likely because there are no chunks).");
		Chunks = chunks;

		_readComprehensiveness = readComprehensiveness;
	}

	public ModBinary(ModBinaryType modBinaryType)
	{
		if (modBinaryType is not (ModBinaryType.Audio or ModBinaryType.Dd))
			throw new NotSupportedException($"Creating mods of type '{modBinaryType}' is not supported.");

		ModBinaryType = modBinaryType;
		Chunks = new();
		AssetMap = new();

		_readComprehensiveness = ModBinaryReadComprehensiveness.All;
	}

	public ModBinaryType ModBinaryType { get; }
	public List<ModBinaryChunk> Chunks { get; }
	public Dictionary<AssetKey, AssetData> AssetMap { get; }

	public void AddAsset(string assetName, AssetType assetType, byte[] fileContents)
	{
		if (_readComprehensiveness != ModBinaryReadComprehensiveness.All)
			throw new InvalidOperationException("This mod binary has not been opened for full reading comprehensiveness. Cannot add assets.");

		if (!IsAssetTypeValid(ModBinaryType, assetType))
			throw new InvalidModBinaryException($"Asset type '{assetType}' is not compatible with binary type '{ModBinaryType}'.");

		AssetMap.Add(new(assetType, assetName), AssetConverter.Compile(assetType, fileContents));

		Chunks.Clear();

		const int tocEntrySizeWithoutName = 15;
		int offset = _fileHeaderSize + tocEntrySizeWithoutName * AssetMap.Count + AssetMap.Sum(kvp => Encoding.UTF8.GetBytes(kvp.Key.AssetName).Length) + 2;
		foreach (KeyValuePair<AssetKey, AssetData> kvp in AssetMap)
		{
			int size = kvp.Value.Buffer.Length;
			Chunks.Add(new(kvp.Key.AssetName, offset, size, kvp.Key.AssetType));

			offset += size;
		}

		// TODO: Loudness.
	}

	public void RemoveAsset(AssetKey assetKey)
		=> RemoveAsset(assetKey.AssetName, assetKey.AssetType);

	public void RemoveAsset(string assetName, AssetType assetType)
	{
		if (_readComprehensiveness != ModBinaryReadComprehensiveness.All)
			throw new InvalidOperationException("This mod binary has not been opened for full reading comprehensiveness. Cannot remove assets.");

		ModBinaryChunk? chunk = Chunks.Find(c => c.Name == assetName && c.AssetType == assetType);
		if (chunk == null)
			return;

		Chunks.Remove(chunk);
		AssetMap.Remove(new(assetType, assetName));
	}

	public void EnableAsset(AssetKey assetKey)
		=> ToggleAsset(assetKey.AssetName, assetKey.AssetType, (c) => c.Enable());

	public void EnableAsset(string assetName, AssetType assetType)
		=> ToggleAsset(assetName, assetType, (c) => c.Enable());

	public void DisableAsset(AssetKey assetKey)
		=> ToggleAsset(assetKey.AssetName, assetKey.AssetType, (c) => c.Disable());

	public void DisableAsset(string assetName, AssetType assetType)
		=> ToggleAsset(assetName, assetType, (c) => c.Disable());

	private void ToggleAsset(string assetName, AssetType assetType, Action<ModBinaryChunk> action)
	{
		if (_readComprehensiveness != ModBinaryReadComprehensiveness.All)
			throw new InvalidOperationException("This mod binary has not been opened for full reading comprehensiveness. Cannot rename assets.");

		ModBinaryChunk? chunk = Chunks.Find(c => c.Name == assetName && c.AssetType == assetType);
		if (chunk == null)
			return;

		action(chunk);

		AssetKey key = new(assetType, assetName);
		AssetData data = AssetMap[key];
		AssetMap.Remove(key);
		AssetMap[new(assetType, chunk.Name)] = data;
	}

	public static bool IsAssetTypeValid(ModBinaryType modBinaryType, AssetType assetType)
	{
		return assetType == AssetType.Audio && modBinaryType == ModBinaryType.Audio || assetType != AssetType.Audio && modBinaryType != ModBinaryType.Audio;
	}

	public byte[] ExtractAsset(AssetKey assetKey)
		=> ExtractAsset(assetKey.AssetName, assetKey.AssetType);

	public byte[] ExtractAsset(string assetName, AssetType assetType)
	{
		if (_readComprehensiveness != ModBinaryReadComprehensiveness.All)
			throw new InvalidOperationException("This mod binary has not been opened for full reading comprehensiveness. Cannot extract assets.");

		AssetKey key = new(assetType, assetName);
		if (!AssetMap.ContainsKey(key))
			throw new InvalidOperationException($"This mod binary does not contain an asset of type '{assetType}' with name '{assetName}'.");

		return AssetConverter.Extract(assetType, AssetMap[key]);
	}

	public byte[] Compile()
	{
		if (_readComprehensiveness != ModBinaryReadComprehensiveness.All)
			throw new InvalidOperationException("This mod binary has not been opened for full reading comprehensiveness. Cannot compile binary.");

		const int tocEntrySizeWithoutName = 15;
		int tocBufferSize = tocEntrySizeWithoutName * AssetMap.Count + Chunks.Sum(c => Encoding.UTF8.GetBytes(c.Name).Length) + sizeof(short);
		int offset = _fileHeaderSize + tocBufferSize;
		byte[]? tocBuffer;
		using (MemoryStream tocStream = new())
		{
			using BinaryWriter tocWriter = new(tocStream);
			foreach (KeyValuePair<AssetKey, AssetData> kvp in AssetMap)
			{
				AssetKey key = kvp.Key;
				AssetData assetData = kvp.Value;

				tocWriter.Write((ushort)key.AssetType);

				tocWriter.Write(Encoding.UTF8.GetBytes(key.AssetName));
				tocWriter.Write((byte)0);

				int size = assetData.Buffer.Length;

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

		List<AssetData> uniqueAssets = AssetMap.Select(ad => ad.Value).Distinct().ToList();
		byte[]? assetBuffer;
		using (MemoryStream assetStream = new())
		{
			using BinaryWriter assetWriter = new(assetStream);
			foreach (AssetData assetData in uniqueAssets)
				assetWriter.Write(assetData.Buffer);

			assetBuffer = assetStream.ToArray();
		}

		using MemoryStream ms = new();
		ms.Write(BitConverter.GetBytes(_fileIdentifier));
		ms.Write(BitConverter.GetBytes((uint)tocBuffer.Length));
		ms.Write(tocBuffer);
		ms.Write(assetBuffer);
		return ms.ToArray();
	}
}
