namespace DevilDaggersInfo.Core.Replay.Numerics;

public readonly record struct Int16Vec3(short X, short Y, short Z) : ISpanFormattable
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
	{
		return $"{X}, {Y}, {Z}";
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		return $"{X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)}, {Z.ToString(format, formatProvider)}";
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		charsWritten = 0;

		if (destination.IsEmpty || destination.Length < 7)
			return false;

		bool formattedX = X.TryFormat(destination[charsWritten..], out int charsWrittenX, format, provider);
		charsWritten += charsWrittenX;
		if (!formattedX)
			return false;

		if (charsWritten + 2 >= destination.Length)
			return false;

		destination[charsWritten] = ',';
		charsWritten++;

		destination[charsWritten] = ' ';
		charsWritten++;

		bool formattedY = Y.TryFormat(destination[charsWritten..], out int charsWrittenY, format, provider);
		charsWritten += charsWrittenY;
		if (!formattedY)
			return false;

		if (charsWritten + 2 >= destination.Length)
			return false;

		destination[charsWritten] = ',';
		charsWritten++;

		destination[charsWritten] = ' ';
		charsWritten++;

		bool formattedZ = Z.TryFormat(destination[charsWritten..], out int charsWrittenZ, format, provider);
		charsWritten += charsWrittenZ;
		return formattedZ;
	}
}
