namespace Warp.NET.Utils;

public static class VectorUtils
{
	public static float GetAngle(Vector2 a, Vector2 b)
	{
		float dot = a.X * b.X + a.Y * b.Y;
		float det = a.X * b.Y - a.Y * b.X;
		return MathF.Atan2(det, dot);
	}

	public static Vector2 Clamp(Vector2 vector, float min, float max)
		=> new(Math.Clamp(vector.X, min, max), Math.Clamp(vector.Y, min, max));

	public static Vector3 Clamp(Vector3 vector, float min, float max)
		=> new(Math.Clamp(vector.X, min, max), Math.Clamp(vector.Y, min, max), Math.Clamp(vector.Z, min, max));

	public static Vector4 Clamp(Vector4 vector, float min, float max)
		=> new(Math.Clamp(vector.X, min, max), Math.Clamp(vector.Y, min, max), Math.Clamp(vector.Z, min, max), Math.Clamp(vector.W, min, max));

	public static Vector3 Reflect(Vector3 direction, Vector3 normal)
	{
		float dn = 2 * Vector3.Dot(direction, normal);
		return direction - normal * dn;
	}

	public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
	{
		float sqrMagnitude = vector.X * vector.X + vector.Y * vector.Y;
		if (sqrMagnitude <= maxLength * maxLength)
			return vector;

		float magnitude = MathF.Sqrt(sqrMagnitude);
		float normalizedX = vector.X / magnitude;
		float normalizedY = vector.Y / magnitude;
		return new(normalizedX * maxLength, normalizedY * maxLength);
	}

	public static Vector3 ClampMagnitude(Vector3 vector, float maxLength)
	{
		float sqrMagnitude = vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z;
		if (sqrMagnitude <= maxLength * maxLength)
			return vector;

		float magnitude = MathF.Sqrt(sqrMagnitude);
		float normalizedX = vector.X / magnitude;
		float normalizedY = vector.Y / magnitude;
		float normalizedZ = vector.Z / magnitude;
		return new(normalizedX * maxLength, normalizedY * maxLength, normalizedZ * maxLength);
	}
}
