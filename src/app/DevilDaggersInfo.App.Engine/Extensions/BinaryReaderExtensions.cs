using Warp.NET.Maths.Numerics;

namespace Warp.NET.Extensions;

public static class BinaryReaderExtensions
{
	public static Vector2 ReadVector2AsHalfPrecision(this BinaryReader br)
		=> new((float)br.ReadHalf(), (float)br.ReadHalf());

	public static Vector3 ReadVector3AsHalfPrecision(this BinaryReader br)
		=> new((float)br.ReadHalf(), (float)br.ReadHalf(), (float)br.ReadHalf());

	public static Vector2 ReadVector2(this BinaryReader br)
		=> new(br.ReadSingle(), br.ReadSingle());

	public static Vector2i<int> ReadVector2Int32(this BinaryReader br)
		=> new(br.ReadInt32(), br.ReadInt32());

	public static Vector2i<short> ReadVector2Int16(this BinaryReader br)
		=> new(br.ReadInt16(), br.ReadInt16());

	public static Vector2i<sbyte> ReadVector2SByte(this BinaryReader br)
		=> new(br.ReadSByte(), br.ReadSByte());

	public static Vector2i<uint> ReadVector2UInt32(this BinaryReader br)
		=> new(br.ReadUInt32(), br.ReadUInt32());

	public static Vector2i<ushort> ReadVector2UInt16(this BinaryReader br)
		=> new(br.ReadUInt16(), br.ReadUInt16());

	public static Vector2i<byte> ReadVector2Byte(this BinaryReader br)
		=> new(br.ReadByte(), br.ReadByte());

	public static Vector3 ReadVector3(this BinaryReader br)
		=> new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

	public static Vector3i<int> ReadVector3Int32(this BinaryReader br)
		=> new(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());

	public static Vector3i<short> ReadVector3Int16(this BinaryReader br)
		=> new(br.ReadInt16(), br.ReadInt16(), br.ReadInt16());

	public static Vector3i<sbyte> ReadVector3SByte(this BinaryReader br)
		=> new(br.ReadSByte(), br.ReadSByte(), br.ReadSByte());

	public static Vector3i<uint> ReadVector3UInt32(this BinaryReader br)
		=> new(br.ReadUInt32(), br.ReadUInt32(), br.ReadUInt32());

	public static Vector3i<ushort> ReadVector3UInt16(this BinaryReader br)
		=> new(br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16());

	public static Vector3i<byte> ReadVector3Byte(this BinaryReader br)
		=> new(br.ReadByte(), br.ReadByte(), br.ReadByte());

	public static Plane ReadPlane(this BinaryReader br)
		=> new(br.ReadVector3(), br.ReadSingle());

	public static List<T> ReadLengthPrefixedList<T>(this BinaryReader br, Func<BinaryReader, T> reader)
	{
		int length = br.ReadInt32();

		List<T> list = new();
		for (int i = 0; i < length; i++)
			list.Add(reader(br));

		return list;
	}
}
