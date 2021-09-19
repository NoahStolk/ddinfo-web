namespace DevilDaggersInfo.Core.Mod;

public class ModBinary
{
	private const long _fileHeader = 0x3A68783A72673A01;

	public ModBinary(string fileName, byte[] fileContents)
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

		if (fileContents.Length <= 12)
			throw new InvalidModBinaryException($"Binary '{fileName}' is not a valid binary; file must be at least 13 bytes in length.");

		ulong fileHeader = BitConverter.ToUInt64(fileContents, 0);
		if (fileHeader != _fileHeader)
			throw new InvalidModBinaryException($"Binary '{fileName}' is not a valid binary; incorrect header values.");

		uint tocSize = BitConverter.ToUInt32(fileContents, 8);
		if (tocSize > fileContents.Length - 12)
			throw new InvalidModBinaryException($"Binary '{fileName}' is not a valid binary; TOC size is larger than the remaining amount of file bytes.");

		byte[] tocBuffer = new byte[tocSize];
		Buffer.BlockCopy(fileContents, 12, tocBuffer, 0, (int)tocSize);

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

			if (!(assetType == AssetType.Audio && name == "loudness"))
				chunks.Add(new(name, offset, size, assetType));
		}

		ModBinaryType = modBinaryType;
		Chunks = chunks;
	}

	public ModBinaryType ModBinaryType { get; }
	public List<ModBinaryChunk> Chunks { get; }

	public void ExtractAssets(string outputDirectory, byte[] fileContents)
	{
		foreach (ModBinaryChunk chunk in Chunks)
		{
			byte[] buffer = new byte[chunk.Size];
			Buffer.BlockCopy(fileContents, chunk.Offset, buffer, 0, buffer.Length);
			AssetWriter.WriteFile(outputDirectory, chunk, buffer);
		}
	}
}
