using System;
using System.Collections.Generic;
using System.Text;

namespace DevilDaggersWebsite.Dto
{
	public class ModData
	{
		public static readonly ulong Magic1 = MakeMagic(0x3AUL, 0x68UL, 0x78UL, 0x3AUL);
		public static readonly ulong Magic2 = MakeMagic(0x72UL, 0x67UL, 0x3AUL, 0x01UL);

		public ModData(string name, ModBinaryType modBinaryType, List<ModAssetData> modAssetData)
		{
			Name = name;
			ModBinaryType = modBinaryType;
			ModAssetData = modAssetData;
		}

		public string Name { get; }
		public ModBinaryType ModBinaryType { get; }
		public List<ModAssetData> ModAssetData { get; }

		private static ulong MakeMagic(ulong a, ulong b, ulong c, ulong d)
			=> a | b << 8 | c << 16 | d << 24;

		public static ModData CreateFromFile(string fileName, byte[] fileContents)
		{
			ModBinaryType modBinaryType;
			if (fileName.StartsWith("audio"))
				modBinaryType = ModBinaryType.Audio;
			else if (fileName.StartsWith("core"))
				modBinaryType = ModBinaryType.Core;
			else if (fileName.StartsWith("dd"))
				modBinaryType = ModBinaryType.Dd;
			else
				throw new($"File '{fileName}' must start with 'audio', 'core', or 'dd'.");

			uint magic1FromFile = BitConverter.ToUInt32(fileContents, 0);
			uint magic2FromFile = BitConverter.ToUInt32(fileContents, 4);
			if (magic1FromFile != Magic1 || magic2FromFile != Magic2)
				throw new($"File '{fileName}' is not a valid binary.");

			uint tocSize = BitConverter.ToUInt32(fileContents, 8);
			byte[] tocBuffer = new byte[tocSize];
			Buffer.BlockCopy(fileContents, 12, tocBuffer, 0, (int)tocSize);

			List<ModAssetData> chunks = new();
			int i = 0;
			while (i < tocBuffer.Length - 14)
			{
				byte type = tocBuffer[i];
				string name = ReadNullTerminatedString(tocBuffer, i + 2);
				i += name.Length + 15;
				ModAssetType assetType = type switch
				{
					0x01 => ModAssetType.Model,
					0x02 => ModAssetType.Texture,
					0x10 => ModAssetType.Shader,
					0x20 => ModAssetType.Audio,
					0x80 => ModAssetType.ModelBinding,
					_ => throw new NotSupportedException($"Byte '{type}' has no asset type."),
				};
				chunks.Add(new(name, assetType));
			}

			return new(fileName, modBinaryType, chunks);

			static string ReadNullTerminatedString(byte[] buffer, int offset)
			{
				StringBuilder sb = new();
				for (int i = offset; i < buffer.Length; i++)
				{
					char c = (char)buffer[i];
					if (c == '\0')
						return sb.ToString();
					sb.Append(c);
				}

				throw new($"Null terminator not observed in buffer with length {buffer.Length} starting from offset {offset}.");
			}
		}
	}
}
