namespace DevilDaggersInfo.Core.Replay.Numerics;

public readonly record struct Matrix3x3(float M11, float M12, float M13, float M21, float M22, float M23, float M31, float M32, float M33) : ISpanFormattable
{
	public static Matrix3x3 Identity { get; } = new(1, 0, 0, 0, 1, 0, 0, 0, 1);

	public override string ToString()
	{
		return $"<{M11:0.00}, {M12:0.00}, {M13:0.00}> <{M21:0.00}, {M22:0.00}, {M23:0.00}> <{M31:0.00}, {M32:0.00}, {M33:0.00}>";
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		return $"<{M11.ToString(format, formatProvider)}, {M12.ToString(format, formatProvider)}, {M13.ToString(format, formatProvider)}> <{M21.ToString(format, formatProvider)}, {M22.ToString(format, formatProvider)}, {M23.ToString(format, formatProvider)}> <{M31.ToString(format, formatProvider)}, {M32.ToString(format, formatProvider)}, {M33.ToString(format, formatProvider)}>";
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		charsWritten = 0;

		if (destination.IsEmpty || destination.Length < 35)
			return false;

		bool formattedM11 = M11.TryFormat(destination[charsWritten..], out int charsWrittenM11, format, provider);
		charsWritten += charsWrittenM11;
		if (!formattedM11)
			return false;

		if (charsWritten + 2 >= destination.Length)
			return false;

		destination[charsWritten] = ',';
		charsWritten++;

		destination[charsWritten] = ' ';
		charsWritten++;

		bool formattedM12 = M12.TryFormat(destination[charsWritten..], out int charsWrittenM12, format, provider);
		charsWritten += charsWrittenM12;
		if (!formattedM12)
			return false;

		if (charsWritten + 2 >= destination.Length)
			return false;

		destination[charsWritten] = ',';
		charsWritten++;

		destination[charsWritten] = ' ';
		charsWritten++;

		bool formattedM13 = M13.TryFormat(destination[charsWritten..], out int charsWrittenM13, format, provider);
		charsWritten += charsWrittenM13;
		if (!formattedM13)
			return false;

		if (charsWritten + 4 >= destination.Length)
			return false;

		destination[charsWritten] = '>';
		charsWritten++;

		destination[charsWritten] = ' ';
		charsWritten++;

		destination[charsWritten] = '<';
		charsWritten++;

		bool formattedM21 = M21.TryFormat(destination[charsWritten..], out int charsWrittenM21, format, provider);
		charsWritten += charsWrittenM21;
		if (!formattedM21)
			return false;

		if (charsWritten + 2 >= destination.Length)
			return false;

		destination[charsWritten] = ',';
		charsWritten++;

		destination[charsWritten] = ' ';
		charsWritten++;

		bool formattedM22 = M22.TryFormat(destination[charsWritten..], out int charsWrittenM22, format, provider);
		charsWritten += charsWrittenM22;
		if (!formattedM22)
			return false;

		if (charsWritten + 2 >= destination.Length)
			return false;

		destination[charsWritten] = ',';
		charsWritten++;

		destination[charsWritten] = ' ';
		charsWritten++;

		bool formattedM23 = M23.TryFormat(destination[charsWritten..], out int charsWrittenM23, format, provider);
		charsWritten += charsWrittenM23;
		if (!formattedM23)
			return false;

		if (charsWritten + 4 >= destination.Length)
			return false;

		destination[charsWritten] = '>';
		charsWritten++;

		destination[charsWritten] = ' ';
		charsWritten++;

		destination[charsWritten] = '<';
		charsWritten++;

		bool formattedM31 = M31.TryFormat(destination[charsWritten..], out int charsWrittenM31, format, provider);
		charsWritten += charsWrittenM31;
		if (!formattedM31)
			return false;

		if (charsWritten + 2 >= destination.Length)
			return false;

		destination[charsWritten] = ',';
		charsWritten++;

		destination[charsWritten] = ' ';
		charsWritten++;

		bool formattedM32 = M32.TryFormat(destination[charsWritten..], out int charsWrittenM32, format, provider);
		charsWritten += charsWrittenM32;
		if (!formattedM32)
			return false;

		if (charsWritten + 2 >= destination.Length)
			return false;

		destination[charsWritten] = ',';
		charsWritten++;

		destination[charsWritten] = ' ';
		charsWritten++;

		bool formattedM33 = M33.TryFormat(destination[charsWritten..], out int charsWrittenM33, format, provider);
		charsWritten += charsWrittenM33;
		if (!formattedM33)
			return false;

		if (charsWritten + 4 >= destination.Length)
			return false;

		destination[charsWritten] = '>';
		charsWritten++;

		return true;
	}
}
