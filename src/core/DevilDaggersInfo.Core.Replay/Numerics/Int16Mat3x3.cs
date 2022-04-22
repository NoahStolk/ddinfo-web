namespace DevilDaggersInfo.Core.Replay.Numerics;

public readonly record struct Int16Mat3x3(short M11, short M12, short M13, short M21, short M22, short M23, short M31, short M32, short M33)
{
	public static Int16Mat3x3 Identity { get; } = new(1, 0, 0, 0, 1, 0, 0, 0, 1);
}
