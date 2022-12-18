using DevilDaggersInfo.Types.Core.Assets;
using DevilDaggersInfo.Types.Core.Mods;

namespace DevilDaggersInfo.Core.Mod;

public sealed class ModBinaryToc
{
	private ModBinaryToc(ModBinaryType type, IReadOnlyList<ModBinaryChunk> chunks)
	{
		Type = type;
		Chunks = chunks;
	}

	public ModBinaryType Type { get; }

	public IReadOnlyList<ModBinaryChunk> Chunks { get; }

	/// <summary>
	/// Reads the mod binary header and TOC from the byte array.
	/// </summary>
	public static ModBinaryToc FromBytes(byte[] contents)
	{
		using MemoryStream ms = new(contents);
		return FromStream(ms);
	}

	/// <summary>
	/// Reads the mod binary header and TOC from the stream.
	/// </summary>
	public static ModBinaryToc FromStream(Stream stream)
	{
		stream.Position = 0;

		using BinaryReader br = new(stream);
		uint tocSize = GetSize(stream.Length, br);

		ModBinaryType? modBinaryType = null;
		List<ModBinaryChunk> chunks = new();
		while (br.BaseStream.Position < ModBinaryConstants.HeaderSize + tocSize)
		{
			ushort type = br.ReadUInt16();
			if (type == 0)
				break; // Break the loop when the end of the TOC is reached (which is 0x0000).

			ModBinaryChunk? chunk = ReadChunk(tocSize, type, br);
			if (chunk == null)
				continue; // Skip invalid chunks.

			chunks.Add(chunk);

			if (!modBinaryType.HasValue)
				modBinaryType = chunk.AssetType == AssetType.Audio ? ModBinaryType.Audio : ModBinaryType.Dd;
			else if (!modBinaryType.Value.IsAssetTypeValid(chunk.AssetType))
				throw new InvalidModBinaryException($"Asset type '{chunk.AssetType}' is not compatible with binary type '{modBinaryType}'.");
		}

		if (!modBinaryType.HasValue)
			throw new InvalidModBinaryException("Mod binary type could not be determined, probably because there are no assets.");

		return new(modBinaryType.Value, chunks);
	}

	public static ModBinaryType DetermineType(byte[] contents)
	{
		using MemoryStream ms = new(contents);
		using BinaryReader br = new(ms);
		uint tocSize = GetSize(ms.Length, br);

		while (br.BaseStream.Position < ModBinaryConstants.HeaderSize + tocSize)
		{
			ushort type = br.ReadUInt16();
			if (type == 0)
				break;

			ModBinaryChunk? chunk = ReadChunk(tocSize, type, br);
			if (chunk != null)
				return chunk.AssetType == AssetType.Audio ? ModBinaryType.Audio : ModBinaryType.Dd;
		}

		throw new InvalidModBinaryException("Mod binary type could not be determined, probably because there are no assets.");
	}

	private static uint GetSize(long totalStreamLength, BinaryReader br)
	{
		if (totalStreamLength < ModBinaryConstants.HeaderSize)
			throw new InvalidModBinaryException($"Invalid binary; must be at least {ModBinaryConstants.HeaderSize} bytes in length.");

		ulong fileIdentifier = br.ReadUInt64();
		if (fileIdentifier != ModBinaryConstants.Identifier)
			throw new InvalidModBinaryException("Invalid binary; incorrect header values.");

		uint tocSize = br.ReadUInt32();
		if (tocSize > totalStreamLength - ModBinaryConstants.HeaderSize)
			throw new InvalidModBinaryException("Invalid binary; TOC size is larger than the remaining amount of bytes.");

		return tocSize;
	}

	private static ModBinaryChunk? ReadChunk(uint tocSize, ushort type, BinaryReader br)
	{
		// Read everything first.
		AssetType? assetType = type.GetAssetType();
		string name = br.ReadNullTerminatedString();
		int offset = br.ReadInt32();
		int size = br.ReadInt32();
		_ = br.ReadInt32();

		// Skip invalid chunks (present in default dd binary).
		if (size <= 0 || offset < ModBinaryConstants.HeaderSize + tocSize)
			return null;

		// Skip unknown or obsolete types (such as 0x11, which is an outdated type for (fragment?) shaders).
		if (!assetType.HasValue)
			return null;

		return new(name, offset, size, assetType.Value);
	}
}
