using DevilDaggersInfo.App.Engine.Maths.Numerics;

namespace DevilDaggersInfo.App.Engine.Extensions;

public static class BinaryWriterExtensions
{
	public static void WriteAsHalfPrecision(this BinaryWriter binaryWriter, Vector2 vector)
	{
		binaryWriter.Write((Half)vector.X);
		binaryWriter.Write((Half)vector.Y);
	}

	public static void WriteAsHalfPrecision(this BinaryWriter binaryWriter, Vector3 vector)
	{
		binaryWriter.Write((Half)vector.X);
		binaryWriter.Write((Half)vector.Y);
		binaryWriter.Write((Half)vector.Z);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector2 vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector2i<int> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector2i<short> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector2i<sbyte> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector2i<uint> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector2i<ushort> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector2i<byte> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector3 vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
		binaryWriter.Write(vector.Z);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector3i<int> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
		binaryWriter.Write(vector.Z);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector3i<short> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
		binaryWriter.Write(vector.Z);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector3i<sbyte> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
		binaryWriter.Write(vector.Z);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector3i<uint> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
		binaryWriter.Write(vector.Z);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector3i<ushort> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
		binaryWriter.Write(vector.Z);
	}

	public static void Write(this BinaryWriter binaryWriter, Vector3i<byte> vector)
	{
		binaryWriter.Write(vector.X);
		binaryWriter.Write(vector.Y);
		binaryWriter.Write(vector.Z);
	}

	public static void Write(this BinaryWriter binaryWriter, Plane plane)
	{
		binaryWriter.Write(plane.Normal);
		binaryWriter.Write(plane.D);
	}

	public static void WriteLengthPrefixedList<T>(this BinaryWriter bw, List<T> list, Action<BinaryWriter, T> writer)
	{
		bw.Write(list.Count);

		for (int i = 0; i < list.Count; i++)
			writer(bw, list[i]);
	}
}
