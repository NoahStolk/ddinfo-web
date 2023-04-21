using DevilDaggersInfo.App.Engine.Maths.Numerics;

namespace DevilDaggersInfo.App.Engine.Extensions;

public static class GenericVectorExtensions
{
	public static Vector2 ToVector2(this Vector2i<sbyte> vector)
		=> new(vector.X, vector.Y);

	public static Vector2 ToVector2(this Vector2i<byte> vector)
		=> new(vector.X, vector.Y);

	public static Vector2 ToVector2(this Vector2i<short> vector)
		=> new(vector.X, vector.Y);

	public static Vector2 ToVector2(this Vector2i<ushort> vector)
		=> new(vector.X, vector.Y);

	public static Vector2 ToVector2(this Vector2i<int> vector)
		=> new(vector.X, vector.Y);

	public static Vector2 ToVector2(this Vector2i<uint> vector)
		=> new(vector.X, vector.Y);

	public static Vector3 ToVector3(this Vector3i<sbyte> vector)
		=> new(vector.X, vector.Y, vector.Z);

	public static Vector3 ToVector3(this Vector3i<byte> vector)
		=> new(vector.X, vector.Y, vector.Z);

	public static Vector3 ToVector3(this Vector3i<short> vector)
		=> new(vector.X, vector.Y, vector.Z);

	public static Vector3 ToVector3(this Vector3i<ushort> vector)
		=> new(vector.X, vector.Y, vector.Z);

	public static Vector3 ToVector3(this Vector3i<int> vector)
		=> new(vector.X, vector.Y, vector.Z);

	public static Vector3 ToVector3(this Vector3i<uint> vector)
		=> new(vector.X, vector.Y, vector.Z);
}
