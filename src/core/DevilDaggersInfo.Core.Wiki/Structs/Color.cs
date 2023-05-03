using System.Numerics;

namespace DevilDaggersInfo.Core.Wiki.Structs;

public readonly record struct Color
{
	public Color(byte r, byte g, byte b)
	{
		R = r;
		G = g;
		B = b;

		HexCode = $"#{R:X2}{G:X2}{B:X2}";
	}

	public byte R { get; }
	public byte G { get; }
	public byte B { get; }

	public string HexCode { get; }

	public static implicit operator Vector4(Color c)
	{
		return new(c.R / 255f, c.G / 255f, c.B / 255f, 1);
	}
}
