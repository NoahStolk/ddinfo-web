using System.Numerics;

namespace DevilDaggersInfo.Core.Wiki.Structs;

public readonly record struct Color(byte R, byte G, byte B)
{
	public string HexCode => $"#{R:X2}{G:X2}{B:X2}";

	public static implicit operator Vector4(Color c)
	{
		return new(c.R / 255f, c.G / 255f, c.B / 255f, 1);
	}
}
