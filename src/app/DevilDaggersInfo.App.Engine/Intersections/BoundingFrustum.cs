namespace Warp.NET.Intersections;

public class BoundingFrustum
{
	private Matrix4x4 _viewProjection;
	private readonly Vector3[] _corners = new Vector3[CornerCount];
	private readonly Plane[] _planes = new Plane[PlaneCount];

	public const int PlaneCount = 6;
	public const int CornerCount = 8;

	public BoundingFrustum(Matrix4x4 viewProjection)
	{
		_viewProjection = viewProjection;
		CreatePlanes();
		CreateCorners();
	}

	public Matrix4x4 Matrix
	{
		get => _viewProjection;
		set
		{
			_viewProjection = value;
			CreatePlanes();
			CreateCorners();
		}
	}

	public Plane Near => _planes[0];
	public Plane Far => _planes[1];
	public Plane Left => _planes[2];
	public Plane Right => _planes[3];
	public Plane Top => _planes[4];
	public Plane Bottom => _planes[5];

	public bool Contains(Vector3 position)
	{
		for (int i = 0; i < PlaneCount; i++)
		{
			if (ClassifyPoint(position, ref _planes[i]) > 0)
				return false;
		}

		return true;

		static float ClassifyPoint(Vector3 position, ref Plane plane)
			=> position.X * plane.Normal.X + position.Y * plane.Normal.Y + position.Z * plane.Normal.Z + plane.D;
	}

	private void CreatePlanes()
	{
		_planes[0] = new(-_viewProjection.M13, -_viewProjection.M23, -_viewProjection.M33, -_viewProjection.M43);
		_planes[1] = new(_viewProjection.M13 - _viewProjection.M14, _viewProjection.M23 - _viewProjection.M24, _viewProjection.M33 - _viewProjection.M34, _viewProjection.M43 - _viewProjection.M44);
		_planes[2] = new(-_viewProjection.M14 - _viewProjection.M11, -_viewProjection.M24 - _viewProjection.M21, -_viewProjection.M34 - _viewProjection.M31, -_viewProjection.M44 - _viewProjection.M41);
		_planes[3] = new(_viewProjection.M11 - _viewProjection.M14, _viewProjection.M21 - _viewProjection.M24, _viewProjection.M31 - _viewProjection.M34, _viewProjection.M41 - _viewProjection.M44);
		_planes[4] = new(_viewProjection.M12 - _viewProjection.M14, _viewProjection.M22 - _viewProjection.M24, _viewProjection.M32 - _viewProjection.M34, _viewProjection.M42 - _viewProjection.M44);
		_planes[5] = new(-_viewProjection.M14 - _viewProjection.M12, -_viewProjection.M24 - _viewProjection.M22, -_viewProjection.M34 - _viewProjection.M32, -_viewProjection.M44 - _viewProjection.M42);

		NormalizePlane(ref _planes[0]);
		NormalizePlane(ref _planes[1]);
		NormalizePlane(ref _planes[2]);
		NormalizePlane(ref _planes[3]);
		NormalizePlane(ref _planes[4]);
		NormalizePlane(ref _planes[5]);
	}

	private void CreateCorners()
	{
		IntersectionPoint(ref _planes[0], ref _planes[2], ref _planes[4], out _corners[0]);
		IntersectionPoint(ref _planes[0], ref _planes[3], ref _planes[4], out _corners[1]);
		IntersectionPoint(ref _planes[0], ref _planes[3], ref _planes[5], out _corners[2]);
		IntersectionPoint(ref _planes[0], ref _planes[2], ref _planes[5], out _corners[3]);
		IntersectionPoint(ref _planes[1], ref _planes[2], ref _planes[4], out _corners[4]);
		IntersectionPoint(ref _planes[1], ref _planes[3], ref _planes[4], out _corners[5]);
		IntersectionPoint(ref _planes[1], ref _planes[3], ref _planes[5], out _corners[6]);
		IntersectionPoint(ref _planes[1], ref _planes[2], ref _planes[5], out _corners[7]);
	}

	private static void IntersectionPoint(ref Plane a, ref Plane b, ref Plane c, out Vector3 result)
	{
		Vector3 cross = Vector3.Cross(b.Normal, c.Normal);

		float f = Vector3.Dot(a.Normal, cross);
		f *= -1.0f;

		cross = Vector3.Cross(b.Normal, c.Normal);
		Vector3 v1 = Vector3.Multiply(cross, a.D);

		cross = Vector3.Cross(c.Normal, a.Normal);
		Vector3 v2 = Vector3.Multiply(cross, b.D);

		cross = Vector3.Cross(a.Normal, b.Normal);
		Vector3 v3 = Vector3.Multiply(cross, c.D);

		result.X = (v1.X + v2.X + v3.X) / f;
		result.Y = (v1.Y + v2.Y + v3.Y) / f;
		result.Z = (v1.Z + v2.Z + v3.Z) / f;
	}

	private static void NormalizePlane(ref Plane p)
	{
		float factor = 1f / p.Normal.Length();
		p.Normal.X *= factor;
		p.Normal.Y *= factor;
		p.Normal.Z *= factor;
		p.D *= factor;
	}
}
