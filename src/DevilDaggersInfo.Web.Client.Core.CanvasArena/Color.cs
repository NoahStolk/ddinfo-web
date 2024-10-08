namespace DevilDaggersInfo.Web.Client.Core.CanvasArena;

public readonly record struct Color(byte R, byte G, byte B)
{
	public int ToInt()
	{
		return (R << 24) + (G << 16) + (B << 8);
	}

	public static Color GetColorFromHeight(float tileHeight)
	{
		float h = tileHeight * 3 + 12;
		float s = (tileHeight + 1.5f) * 0.25f;
		float v = (tileHeight + 2) * 0.2f;
		return FromHsv(h, s, v);
	}

	public static Color FromHsv(float hue, float saturation, float value)
	{
		saturation = Math.Clamp(saturation, 0, 1);
		value = Math.Clamp(value, 0, 1);

		int hi = (int)MathF.Floor(hue / 60) % 6;
		float f = hue / 60 - MathF.Floor(hue / 60);

		value *= 255;
		byte v = (byte)value;
		byte p = (byte)(value * (1 - saturation));
		byte q = (byte)(value * (1 - f * saturation));
		byte t = (byte)(value * (1 - (1 - f) * saturation));

		return hi switch
		{
			0 => new Color(v, t, p),
			1 => new Color(q, v, p),
			2 => new Color(p, v, t),
			3 => new Color(p, q, v),
			4 => new Color(t, p, v),
			_ => new Color(v, p, q),
		};
	}
}
