namespace DevilDaggersInfo.Core.Replay.Numerics;

public readonly record struct Matrix3x3(float M11, float M12, float M13, float M21, float M22, float M23, float M31, float M32, float M33)
{
	public static Matrix3x3 Identity { get; } = new(1, 0, 0, 0, 1, 0, 0, 0, 1);
}
