namespace DevilDaggersInfo.Core.Replay.Numerics;

public readonly record struct Int16Vec3(short X, short Y, short Z)
{
	public static Int16Vec3 Zero { get; } = new(0, 0, 0);

	public static Int16Vec3 operator +(Int16Vec3 a, Int16Vec3 b)
	{
		return new(
			(short)(a.X + b.X),
			(short)(a.Y + b.Y),
			(short)(a.Z + b.Z));
	}

	public override string ToString()
		=> $"{{ {X}, {Y}, {Z} }}";
}
