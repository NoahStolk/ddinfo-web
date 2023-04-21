using DevilDaggersInfo.App.Engine.Maths.Numerics;
using DevilDaggersInfo.App.Engine.Utils;

namespace DevilDaggersInfo.App.Engine.Extensions;

public static class VectorExtensions
{
	public static Vector2 Sum(this IEnumerable<Vector2> source)
	{
		float x = 0, y = 0;
		foreach (Vector2 vector in source)
		{
			x += vector.X;
			y += vector.Y;
		}

		return new(x, y);
	}

	public static Vector3 Sum(this IEnumerable<Vector3> source)
	{
		float x = 0, y = 0, z = 0;
		foreach (Vector3 vector in source)
		{
			x += vector.X;
			y += vector.Y;
			z += vector.Z;
		}

		return new(x, y, z);
	}

	public static Vector4 Sum(this IEnumerable<Vector4> source)
	{
		float x = 0, y = 0, z = 0, w = 0;
		foreach (Vector4 vector in source)
		{
			x += vector.X;
			y += vector.Y;
			z += vector.Z;
			w += vector.W;
		}

		return new(x, y, z, w);
	}

	public static Vector2 Round(this Vector2 vector, int digits)
		=> new(MathF.Round(vector.X, digits), MathF.Round(vector.Y, digits));

	public static Vector3 Round(this Vector3 vector, int digits)
		=> new(MathF.Round(vector.X, digits), MathF.Round(vector.Y, digits), MathF.Round(vector.Z, digits));

	public static Vector4 Round(this Vector4 vector, int digits)
		=> new(MathF.Round(vector.X, digits), MathF.Round(vector.Y, digits), MathF.Round(vector.Z, digits), MathF.Round(vector.W, digits));

	public static Vector2i<int> FloorToVector2Int32(this Vector2 vector)
		=> new((int)MathF.Floor(vector.X), (int)MathF.Floor(vector.Y));

	public static Vector3i<int> FloorToVector3Int32(this Vector3 vector)
		=> new((int)MathF.Floor(vector.X), (int)MathF.Floor(vector.Y), (int)MathF.Floor(vector.Z));

	public static Vector2i<int> RoundToVector2Int32(this Vector2 vector)
		=> new((int)MathF.Round(vector.X), (int)MathF.Round(vector.Y));

	public static Vector3i<int> RoundToVector3Int32(this Vector3 vector)
		=> new((int)MathF.Round(vector.X), (int)MathF.Round(vector.Y), (int)MathF.Round(vector.Z));

	public static Vector2i<int> CeilingToVector2Int32(this Vector2 vector)
		=> new((int)MathF.Ceiling(vector.X), (int)MathF.Ceiling(vector.Y));

	public static Vector3i<int> CeilingToVector3Int32(this Vector3 vector)
		=> new((int)MathF.Ceiling(vector.X), (int)MathF.Ceiling(vector.Y), (int)MathF.Ceiling(vector.Z));

	public static Vector2i<short> FloorToVector2Int16(this Vector2 vector)
		=> new((short)MathF.Floor(vector.X), (short)MathF.Floor(vector.Y));

	public static Vector3i<short> FloorToVector3Int16(this Vector3 vector)
		=> new((short)MathF.Floor(vector.X), (short)MathF.Floor(vector.Y), (short)MathF.Floor(vector.Z));

	public static Vector2i<short> RoundToVector2Int16(this Vector2 vector)
		=> new((short)MathF.Round(vector.X), (short)MathF.Round(vector.Y));

	public static Vector3i<short> RoundToVector3Int16(this Vector3 vector)
		=> new((short)MathF.Round(vector.X), (short)MathF.Round(vector.Y), (short)MathF.Round(vector.Z));

	public static Vector2i<short> CeilingToVector2Int16(this Vector2 vector)
		=> new((short)MathF.Ceiling(vector.X), (short)MathF.Ceiling(vector.Y));

	public static Vector3i<short> CeilingToVector3Int16(this Vector3 vector)
		=> new((short)MathF.Ceiling(vector.X), (short)MathF.Ceiling(vector.Y), (short)MathF.Ceiling(vector.Z));

	public static Vector2i<sbyte> FloorToVector2SByte(this Vector2 vector)
		=> new((sbyte)MathF.Floor(vector.X), (sbyte)MathF.Floor(vector.Y));

	public static Vector3i<sbyte> FloorToVector3SByte(this Vector3 vector)
		=> new((sbyte)MathF.Floor(vector.X), (sbyte)MathF.Floor(vector.Y), (sbyte)MathF.Floor(vector.Z));

	public static Vector2i<sbyte> RoundToVector2SByte(this Vector2 vector)
		=> new((sbyte)MathF.Round(vector.X), (sbyte)MathF.Round(vector.Y));

	public static Vector3i<sbyte> RoundToVector3SByte(this Vector3 vector)
		=> new((sbyte)MathF.Round(vector.X), (sbyte)MathF.Round(vector.Y), (sbyte)MathF.Round(vector.Z));

	public static Vector2i<sbyte> CeilingToVector2SByte(this Vector2 vector)
		=> new((sbyte)MathF.Ceiling(vector.X), (sbyte)MathF.Ceiling(vector.Y));

	public static Vector3i<sbyte> CeilingToVector3SByte(this Vector3 vector)
		=> new((sbyte)MathF.Ceiling(vector.X), (sbyte)MathF.Ceiling(vector.Y), (sbyte)MathF.Ceiling(vector.Z));

	public static string ToString(this Vector2 vector, int digits)
		=> $"{{{FormatUtils.FormatAxis(nameof(Vector2.X), vector.X, digits)} {FormatUtils.FormatAxis(nameof(Vector2.Y), vector.Y, digits)}}}";

	public static string ToString(this Vector3 vector, int digits)
		=> $"{{{FormatUtils.FormatAxis(nameof(Vector3.X), vector.X, digits)} {FormatUtils.FormatAxis(nameof(Vector3.Y), vector.Y, digits)} {FormatUtils.FormatAxis(nameof(Vector3.Z), vector.Z, digits)}}}";

	public static string ToString(this Vector4 vector, int digits)
		=> $"{{{FormatUtils.FormatAxis(nameof(Vector4.X), vector.X, digits)} {FormatUtils.FormatAxis(nameof(Vector4.Y), vector.Y, digits)} {FormatUtils.FormatAxis(nameof(Vector4.Z), vector.Z, digits)} {FormatUtils.FormatAxis(nameof(Vector4.W), vector.W, digits)}}}";
}
