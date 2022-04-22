namespace DevilDaggersInfo.Core.Replay.Numerics;

public readonly record struct Int16Vec3(short X, short Y, short Z)
{
	public static Int16Vec3 Zero { get; } = new(0, 0, 0);
}
