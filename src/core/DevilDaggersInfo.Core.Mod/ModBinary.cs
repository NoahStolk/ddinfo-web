namespace DevilDaggersInfo.Core.Mod;

public class ModBinary
{
	public const long FileIdentifier = 0x013A67723A78683A;

	private const int _fileHeaderSize = 12;

	private readonly ModBinaryReadComprehensiveness _readComprehensiveness;

	public ModBinary(byte[] fileContents, ModBinaryReadComprehensiveness readComprehensiveness)
	{
		if (fileContents.Length < _fileHeaderSize)
			throw new InvalidModBinaryException($"Invalid binary; must be at least {_fileHeaderSize} bytes in length.");

		using MemoryStream ms = new(fileContents);
		using BinaryReader br = new(ms);
		ulong fileIdentifier = br.ReadUInt64();
		if (fileIdentifier != FileIdentifier)
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
			AssetType? assetType = type.GetAssetType();

			// Skip unknown or obsolete types (such as 0x11, which is an outdated type for (fragment?) shaders).
			// This also breaks the while loop when the end of the TOC is reached (which is 0x0000).
			if (!assetType.HasValue)
				continue;

			string name = br.ReadNullTerminatedString();

			int offset = br.ReadInt32();
			int size = br.ReadInt32();
			_ = br.ReadInt32();

			chunks.Add(new(name, offset, size, assetType.Value));

			if (!modBinaryType.HasValue)
				modBinaryType = assetType == AssetType.Audio ? ModBinaryType.Audio : ModBinaryType.Dd;
			else
				ValidateAssetType(modBinaryType.Value, assetType.Value);
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
		ValidateAssetType(ModBinaryType, assetType);

		AssetMap.Add(new(assetType, assetName), AssetConverter.Compile(assetType, fileContents));

		Chunks.Clear();

		const int tocEntrySizeWithoutName = 15;
		int offset = _fileHeaderSize + tocEntrySizeWithoutName * AssetMap.Count + AssetMap.Sum(kvp => Encoding.Default.GetBytes(kvp.Key.AssetName).Length) + 2;
		foreach (KeyValuePair<AssetKey, AssetData> kvp in AssetMap)
		{
			int size = kvp.Value.Buffer.Length;
			Chunks.Add(new(kvp.Key.AssetName, offset, size, kvp.Key.AssetType));

			offset += size;
		}

		// TODO: Loudness.
	}

	private static void ValidateAssetType(ModBinaryType modBinaryType, AssetType assetType)
	{
		if (assetType == AssetType.Audio && modBinaryType != ModBinaryType.Audio)
			throw new InvalidModCompilationException($"Cannot add an audio asset to a mod of type '{modBinaryType}'.");
		if (assetType != AssetType.Audio && modBinaryType == ModBinaryType.Audio)
			throw new InvalidModCompilationException("Cannot add a non-audio asset to an audio mod.");
	}

	public void ExtractAssets(string outputDirectory)
	{
		if (_readComprehensiveness != ModBinaryReadComprehensiveness.All)
			throw new InvalidOperationException("This mod binary has not been opened for full reading comprehensiveness. Cannot extract assets from mod binary.");

		foreach (KeyValuePair<AssetKey, AssetData> kvp in AssetMap)
			File.WriteAllBytes(Path.Combine(outputDirectory, kvp.Key.AssetName + kvp.Key.AssetType.GetFileExtension()), AssetConverter.Extract(kvp.Key.AssetType, kvp.Value));
	}

	public byte[] ExtractAsset(string assetName, AssetType assetType)
	{
		if (_readComprehensiveness != ModBinaryReadComprehensiveness.All)
			throw new InvalidOperationException("This mod binary has not been opened for full reading comprehensiveness. Cannot extract assets from mod binary.");

		AssetKey key = new(assetType, assetName);
		if (!AssetMap.ContainsKey(key))
			throw new InvalidOperationException($"This mod binary does not contain an asset of type '{assetType}' with name '{assetName}'.");

		return AssetConverter.Extract(assetType, AssetMap[key]);
	}

	public byte[] Compile()
	{
		if (_readComprehensiveness != ModBinaryReadComprehensiveness.All)
			throw new InvalidOperationException("This mod binary has not been opened for full reading comprehensiveness. Cannot compile mod binary.");

		const int tocEntrySizeWithoutName = 15;
		int tocBufferSize = tocEntrySizeWithoutName * AssetMap.Count + Chunks.Sum(c => Encoding.Default.GetBytes(c.Name).Length) + sizeof(short);
		int offset = _fileHeaderSize + tocBufferSize;
		byte[]? tocBuffer = null;
		using (MemoryStream tocStream = new())
		{
			using BinaryWriter bw = new(tocStream);
			foreach (KeyValuePair<AssetKey, AssetData> kvp in AssetMap)
			{
				AssetKey key = kvp.Key;
				AssetData assetData = kvp.Value;

				bw.Write((ushort)key.AssetType);

				bw.Write(Encoding.Default.GetBytes(key.AssetName));
				bw.Write((byte)0);

				int size = assetData.Buffer.Length;

				bw.Write(offset);
				bw.Write(size);
				bw.Write(0);

				offset += size;
			}

			bw.Write((short)0);

			tocBuffer = tocStream.ToArray();
		}

		if (tocBuffer.Length != tocBufferSize)
			throw new InvalidOperationException($"Invalid TOC buffer size: {tocBuffer.Length}. Expected length was {tocBufferSize}.");

		List<AssetData> uniqueAssets = AssetMap.Select(ad => ad.Value).Distinct().ToList();
		byte[]? assetBuffer = null;
		using (MemoryStream assetStream = new())
		{
			using BinaryWriter bw = new(assetStream);
			foreach (AssetData assetData in uniqueAssets)
				bw.Write(assetData.Buffer);

			assetBuffer = assetStream.ToArray();
		}

		using MemoryStream ms = new();
		ms.Write(BitConverter.GetBytes(FileIdentifier));
		ms.Write(BitConverter.GetBytes((uint)tocBuffer.Length));
		ms.Write(tocBuffer);
		ms.Write(assetBuffer);
		return ms.ToArray();
	}
}
