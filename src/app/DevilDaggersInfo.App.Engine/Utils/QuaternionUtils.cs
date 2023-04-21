namespace Warp.NET.Utils;

public static class QuaternionUtils
{
	public static Quaternion CreateFromRotationBetween(Vector3 a, Vector3 b)
	{
		float dot = Vector3.Dot(a, b);
		if (dot > 0.999999f)
			return Quaternion.Identity;

		Vector3 cross;
		if (dot < -0.999999f)
		{
			cross = Vector3.Cross(Vector3.UnitX, a);
			if (cross.Length() < 0.000001f)
				cross = Vector3.Cross(Vector3.UnitY, a);
			cross = Vector3.Normalize(cross);

			return Quaternion.CreateFromAxisAngle(cross, MathF.PI);
		}

		cross = Vector3.Cross(a, b);
		Quaternion q = new(cross, 1 + dot);
		return Quaternion.Normalize(q);
	}
}
