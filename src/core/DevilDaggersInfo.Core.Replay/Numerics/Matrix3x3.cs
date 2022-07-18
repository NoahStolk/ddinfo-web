namespace DevilDaggersInfo.Core.Replay.Numerics;

public readonly record struct Matrix3x3(float M11, float M12, float M13, float M21, float M22, float M23, float M31, float M32, float M33)
{
	public static Matrix3x3 Identity { get; } = new(1, 0, 0, 0, 1, 0, 0, 0, 1);

	public override string ToString()
		=> $"<{M11:0.00}, {M12:0.00}, {M13:0.00}> <{M21:0.00}, {M22:0.00}, {M23:0.00}> <{M31:0.00}, {M32:0.00}, {M33:0.00}>";
}
