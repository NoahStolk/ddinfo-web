namespace DevilDaggersInfo.Core.Mod;

public class ModBinary
{
	private const long _fileIdentifier = 0x013A67723A78683A;
	private const int _fileHeaderSize = 12;

	public ModBinary(string fileName, byte[] fileContents, BinaryReadComprehensiveness readComprehensiveness)
	{
		ModBinaryType modBinaryType = BinaryFileNameUtils.GetBinaryTypeBasedOnFileName(fileName);

		if (fileContents.Length <= _fileHeaderSize)
			throw new InvalidModBinaryException($"Binary '{fileName}' is not a valid binary; file must be at least 13 bytes in length.");

		using MemoryStream ms = new(fileContents);
		using BinaryReader br = new(ms);
		ulong fileIdentifier = br.ReadUInt64();
		if (fileIdentifier != _fileIdentifier)
			throw new InvalidModBinaryException($"Binary '{fileName}' is not a valid binary; incorrect header values.");

		uint tocSize = br.ReadUInt32();
		if (tocSize > fileContents.Length - _fileHeaderSize)
			throw new InvalidModBinaryException($"Binary '{fileName}' is not a valid binary; TOC size is larger than the remaining amount of file bytes.");

		List<ModBinaryChunk> chunks = new();
		while (true)
		{
			ushort type = br.ReadUInt16();
			string name = br.ReadNullTerminatedString();

			int offset = br.ReadInt32();
			int size = br.ReadInt32();
			_ = br.ReadInt32();

			if (br.BaseStream.Position >= _fileHeaderSize + tocSize)
				break;

			AssetType assetType = (AssetType)type;

			// Skip unknown or obsolete types (such as 0x11, which is an outdated type for (fragment?) shaders).
			if (!Enum.IsDefined(assetType))
				continue;

			chunks.Add(new(name, offset, size, assetType));
		}

		if (readComprehensiveness == BinaryReadComprehensiveness.All)
		{
			AssetMap = new();

			// If chunks point to the same asset position; the asset is re-used (TODO: test if this even works in DD -- if not, remove DistinctBy).
			foreach (ModBinaryChunk chunk in chunks.DistinctBy(c => c.Offset))
			{
				br.BaseStream.Seek(chunk.Offset, SeekOrigin.Begin);
				byte[] buffer = br.ReadBytes(chunk.Size);

				AssetMap[chunk] = new(buffer);
			}
		}
		else if (readComprehensiveness == BinaryReadComprehensiveness.TocAndLoudness)
		{
			ModBinaryChunk? loudnessChunk = chunks.Find(c => c.IsLoudness());
			if (loudnessChunk != null)
			{
				br.BaseStream.Seek(loudnessChunk.Offset, SeekOrigin.Begin);
				byte[] buffer = br.ReadBytes(loudnessChunk.Size);

				AssetMap = new()
				{
					[loudnessChunk] = new(buffer),
				};
			}
		}

		ModBinaryType = modBinaryType;
		Chunks = chunks;
	}

	public ModBinaryType ModBinaryType { get; }
	public List<ModBinaryChunk> Chunks { get; }
	public Dictionary<ModBinaryChunk, AssetData>? AssetMap { get; }

	public void ExtractAssets(string outputDirectory)
	{
		if (AssetMap == null)
		{
			// TODO: Log warning.
			return;
		}

		foreach (KeyValuePair<ModBinaryChunk, AssetData> kvp in AssetMap)
			File.WriteAllBytes(Path.Combine(outputDirectory, kvp.Key.Name + kvp.Key.AssetType.GetFileExtension()), AssetConverter.Extract(kvp.Key.AssetType, kvp.Value));
	}

	public byte[] ToBytes()
	{
		if (AssetMap == null)
		{
			// TODO: Log warning.
			return Array.Empty<byte>();
		}

		List<AssetData> uniqueAssets = AssetMap.Select(ad => ad.Value).Distinct().ToList();
		byte[]? assetBuffer = null;
		Dictionary<AssetData, int> assetDataOffsets = new();
		using (MemoryStream assetStream = new())
		{
			using BinaryWriter bw = new(assetStream);
			foreach (AssetData assetData in uniqueAssets)
			{
				assetDataOffsets.Add(assetData, (int)bw.BaseStream.Position);
				bw.Write(assetData.Buffer);
			}

			assetBuffer = assetStream.ToArray();
		}

		const int tocEntrySizeWithoutName = 15;
		int tocSize = tocEntrySizeWithoutName * AssetMap.Count + Chunks.Sum(c => Encoding.Default.GetBytes(c.Name).Length) + 2;
		byte[]? tocBuffer = null;
		using (MemoryStream tocStream = new())
		{
			using BinaryWriter bw = new(tocStream);
			foreach (KeyValuePair<ModBinaryChunk, AssetData> kvp in AssetMap)
			{
				ModBinaryChunk chunk = kvp.Key;
				AssetData assetData = kvp.Value;

				bw.Write((ushort)chunk.AssetType);

				bw.Write(Encoding.Default.GetBytes(chunk.Name));
				bw.Write((byte)0);

				bw.Write(_fileHeaderSize + tocSize + assetDataOffsets[assetData]);
				bw.Write(assetData.Buffer.Length);
				bw.Write(0);
			}

			bw.Write((short)0);

			tocBuffer = tocStream.ToArray();
		}

		if (tocBuffer.Length != tocSize)
			throw new($"Invalid TOC buffer size: {tocBuffer.Length}. Expected length was {tocSize}.");

		using MemoryStream ms = new();
		ms.Write(BitConverter.GetBytes(_fileIdentifier));
		ms.Write(BitConverter.GetBytes((uint)tocBuffer.Length));
		ms.Write(tocBuffer);
		ms.Write(assetBuffer);
		return ms.ToArray();
	}
}
