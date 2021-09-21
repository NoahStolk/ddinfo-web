namespace DevilDaggersInfo.Core.Mod;

public class ModBinary
{
	private const long _fileIdentifier = 0x013A67723A78683A;
	private const int _fileHeaderSize = 12;

	public ModBinary(string fileName, byte[] fileContents, bool readAssets)
	{
		ModBinaryType modBinaryType;
		if (fileName.StartsWith("audio"))
			modBinaryType = ModBinaryType.Audio;
		else if (fileName.StartsWith("core"))
			modBinaryType = ModBinaryType.Core;
		else if (fileName.StartsWith("dd"))
			modBinaryType = ModBinaryType.Dd;
		else
			throw new InvalidModBinaryException($"Binary '{fileName}' must start with 'audio', 'core', or 'dd'.");

		if (fileContents.Length <= _fileHeaderSize)
			throw new InvalidModBinaryException($"Binary '{fileName}' is not a valid binary; file must be at least 13 bytes in length.");

		ulong fileIdentifier = BitConverter.ToUInt64(fileContents, 0);
		if (fileIdentifier != _fileIdentifier)
			throw new InvalidModBinaryException($"Binary '{fileName}' is not a valid binary; incorrect header values.");

		uint tocSize = BitConverter.ToUInt32(fileContents, 8);
		if (tocSize > fileContents.Length - _fileHeaderSize)
			throw new InvalidModBinaryException($"Binary '{fileName}' is not a valid binary; TOC size is larger than the remaining amount of file bytes.");

		byte[] tocBuffer = new byte[tocSize];
		Buffer.BlockCopy(fileContents, _fileHeaderSize, tocBuffer, 0, (int)tocSize);

		List<ModBinaryChunk> chunks = new();
		int i = 0;
		while (i < tocBuffer.Length - 14)
		{
			byte type = tocBuffer[i];
			string name = tocBuffer.ReadNullTerminatedString(i + 2);

			i += name.Length + 1; // + 1 to include null terminator.
			int offset = BitConverter.ToInt32(tocBuffer, i + 2);
			int size = BitConverter.ToInt32(tocBuffer, i + 6);
			i += 14;
			AssetType assetType = (AssetType)type;

			// Skip unknown or obsolete types (such as 0x11, which is an outdated type for (fragment?) shaders).
			if (!Enum.IsDefined(assetType))
				continue;

			// Skip loudness.
			if (assetType == AssetType.Audio && name == "loudness")
				continue;

			chunks.Add(new(name, offset, size, assetType));
		}

		if (readAssets)
		{
			AssetMap = new();
			foreach (ModBinaryChunk chunk in chunks)
			{
				byte[] buffer = new byte[chunk.Size];
				Buffer.BlockCopy(fileContents, chunk.Offset, buffer, 0, buffer.Length);

				// TODO: If in TOC the chunks point to the same asset position; the asset is re-used (TODO: test if this works in DD).
				AssetMap[chunk] = new(buffer);
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
			File.WriteAllBytes(Path.Combine(outputDirectory, kvp.Key.Name + kvp.Key.AssetType.GetFileExtension()), AssetConverter.Extract(kvp.Key, kvp.Value.Buffer));
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

		int tocSize = 15 * AssetMap.Count + Chunks.Sum(c => Encoding.Default.GetBytes(c.Name).Length) + 2;
		byte[]? tocBuffer = null;
		using (MemoryStream tocStream = new())
		{
			using BinaryWriter bw = new(tocStream);
			foreach (KeyValuePair<ModBinaryChunk, AssetData> kvp in AssetMap)
			{
				ModBinaryChunk chunk = kvp.Key;
				AssetData assetData = kvp.Value;

				bw.Write((byte)chunk.AssetType);
				bw.Write((byte)0);

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
