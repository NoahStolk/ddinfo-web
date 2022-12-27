// ReSharper disable InconsistentNaming
namespace DevilDaggersInfo.Core.Replay.Numerics;

public readonly record struct Int16Mat3x3(short M11, short M12, short M13, short M21, short M22, short M23, short M31, short M32, short M33)
{
	public static Int16Mat3x3 Identity { get; } = new(1, 0, 0, 0, 1, 0, 0, 0, 1);

	// TODO: Untested.
	public static Int16Mat3x3 FromMatrix4x4(Matrix4x4 matrix4x4)
	{
		return new(
			(short)(matrix4x4.M11 * short.MaxValue),
			(short)(matrix4x4.M12 * short.MaxValue),
			(short)(matrix4x4.M13 * short.MaxValue),
			(short)(matrix4x4.M21 * short.MaxValue),
			(short)(matrix4x4.M22 * short.MaxValue),
			(short)(matrix4x4.M23 * short.MaxValue),
			(short)(matrix4x4.M31 * short.MaxValue),
			(short)(matrix4x4.M32 * short.MaxValue),
			(short)(matrix4x4.M33 * short.MaxValue));
	}

	public override string ToString()
		=> $"<{M11}, {M12}, {M13}> <{M21}, {M22}, {M23}> <{M31}, {M32}, {M33}>";
}
