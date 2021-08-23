namespace DevilDaggersInfo.Core.Mod;

public static class ModBinaryHandler
{
	public static readonly ulong Magic1 = MakeMagic(0x3AUL, 0x68UL, 0x78UL, 0x3AUL);
	public static readonly ulong Magic2 = MakeMagic(0x72UL, 0x67UL, 0x3AUL, 0x01UL);

	private static ulong MakeMagic(ulong a, ulong b, ulong c, ulong d)
		=> a | b << 8 | c << 16 | d << 24;

	public static ModBinary ReadChunks(string fileName, byte[] fileContents)
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

		uint magic1FromFile = BitConverter.ToUInt32(fileContents, 0);
		uint magic2FromFile = BitConverter.ToUInt32(fileContents, 4);
		if (magic1FromFile != Magic1 || magic2FromFile != Magic2)
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
			string name = ReadNullTerminatedString(tocBuffer, i + 2);

			i += name.Length + 1; // + 1 to include null terminator.
			uint offset = BitConverter.ToUInt32(tocBuffer, i + 2);
			uint size = BitConverter.ToUInt32(tocBuffer, i + 6);
			i += 14;
			AssetType assetType = type switch
			{
				0x01 => AssetType.Model,
				0x02 => AssetType.Texture,
				0x10 => AssetType.Shader,
				0x20 => AssetType.Audio,
				0x80 => AssetType.ModelBinding,
				_ => throw new InvalidModBinaryException($"Binary '{fileName}' contains an unknown asset type '{type}'. Valid types are {0x01}, {0x02}, {0x10}, {0x20}, and {0x80}."),
			};

			if (!(assetType == AssetType.Audio && name == "loudness"))
				chunks.Add(new(name, offset, size, assetType));
		}

		return new(modBinaryType, chunks);

		string ReadNullTerminatedString(byte[] buffer, int offset)
		{
			StringBuilder sb = new();
			for (int i = offset; i < buffer.Length; i++)
			{
				char c = (char)buffer[i];
				if (c == '\0')
					return sb.ToString();
				sb.Append(c);
			}

			throw new InvalidModBinaryException($"Null terminator not observed in buffer with length '{buffer.Length}' starting from offset '{offset}' in mod binary '{fileName}'.");
		}
	}
}
