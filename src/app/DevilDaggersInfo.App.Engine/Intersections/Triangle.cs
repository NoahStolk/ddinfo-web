namespace Warp.NET.Intersections;

public readonly record struct Triangle
{
	public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		P1 = p1;
		P2 = p2;
		P3 = p3;

		Normal = GetNormal(P1, P2, P3);
		Center = GetCenter(P1, P2, P3);
		Area = GetArea(P1, P2, P3);
	}

	public Vector3 P1 { get; }
	public Vector3 P2 { get; }
	public Vector3 P3 { get; }

	public Vector3 Normal { get; }
	public Vector3 Center { get; }
	public float Area { get; }

	/// <summary>
	/// <see href="https://wickedengine.net/2020/04/26/capsule-collision-detection/">Source</see>.
	/// </summary>
	public Vector3 CollideSphere(Vector3 position, float radius)
	{
		float dist = Vector3.Dot(position - P1, Normal); // Signed distance between sphere and plane.
		if (dist < -radius || dist > radius)
			return default;

		Vector3 point0 = position - Normal * dist; // Projected sphere center on triangle plane.

		// Now determine whether point0 is inside all triangle edges:
		Vector3 c0 = Vector3.Cross(point0 - P1, P2 - P1);
		Vector3 c1 = Vector3.Cross(point0 - P2, P3 - P2);
		Vector3 c2 = Vector3.Cross(point0 - P3, P1 - P3);
		if (Vector3.Dot(c0, Normal) <= 0 && Vector3.Dot(c1, Normal) <= 0 && Vector3.Dot(c2, Normal) <= 0)
		{
			float distance = (point0 - position).Length();
			return Normal * (radius - distance);
		}

		float radiusSq = radius * radius;

		// Edge 1:
		Vector3 point1 = ClosestPointOnLineSegment(P1, P2, position);
		Vector3 v1 = position - point1;
		float distSq1 = Vector3.Dot(v1, v1);
		bool intersects1 = distSq1 < radiusSq;

		// Edge 2:
		Vector3 point2 = ClosestPointOnLineSegment(P2, P3, position);
		Vector3 v2 = position - point2;
		float distSq2 = Vector3.Dot(v2, v2);
		bool intersects2 = distSq2 < radiusSq;

		// Edge 3:
		Vector3 point3 = ClosestPointOnLineSegment(P3, P1, position);
		Vector3 v3 = position - point3;
		float distSq3 = Vector3.Dot(v3, v3);
		bool intersects3 = distSq3 < radiusSq;

		// Find best edge
		if (intersects1 || intersects2 || intersects3)
		{
			Vector3 d = position - point1;
			float bestDistSq = Vector3.Dot(d, d);
			Vector3 bestPoint = point1;

			d = position - point2;
			float distSq = Vector3.Dot(d, d);
			if (distSq < bestDistSq)
			{
				bestDistSq = distSq;
				bestPoint = point2;
			}

			d = position - point3;
			distSq = Vector3.Dot(d, d);
			if (distSq < bestDistSq)
			{
				bestPoint = point3;
			}

			float distance = (bestPoint - position).Length();
			return Normal * (radius - distance);
		}

		return default;

		static Vector3 ClosestPointOnLineSegment(Vector3 a, Vector3 b, Vector3 point)
		{
			Vector3 ab = b - a;
			float t = Vector3.Dot(point - a, ab) / Vector3.Dot(ab, ab);
			return a + Math.Clamp(t, 0, 1) * ab;
		}
	}

	/// <summary>
	/// <see href="https://math.stackexchange.com/a/100766">Source</see>.
	/// </summary>
	public Vector3 GetClosestPointOnPlane(Vector3 position)
	{
		float t =
			Normal.X * Center.X - Normal.X * position.X +
			Normal.Y * Center.Y - Normal.Y * position.Y +
			Normal.Z * Center.Z - Normal.Z * position.Z;

		return new(
			position.X + t * Normal.X,
			position.Y + t * Normal.Y,
			position.Z + t * Normal.Z);
	}

	private static Vector3 GetNormal(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		return Vector3.Normalize(Vector3.Cross(p2 - p1, p3 - p1));
	}

	private static Vector3 GetCenter(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		return (p1 + p2 + p3) / 3;
	}

	private static float GetArea(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float a = (p1 - p2).Length();
		float b = (p2 - p3).Length();
		float c = (p3 - p1).Length();
		float s = (a + b + c) / 2;
		return MathF.Sqrt(s * (s - a) * (s - b) * (s - c));
	}
}
