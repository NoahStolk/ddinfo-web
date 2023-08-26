using DevilDaggersInfo.Core.Replay.Utils;

namespace DevilDaggersInfo.Core.Replay.Numerics;

// ReSharper disable once InconsistentNaming
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

		return
			SpanWrite.TryWriteChar(destination, ref charsWritten, '<') &&
			SpanWrite.TryWrite(destination, ref charsWritten, M11, format, provider) &&
			SpanWrite.TryWriteString(destination, ref charsWritten, ", ") &&
			SpanWrite.TryWrite(destination, ref charsWritten, M12, format, provider) &&
			SpanWrite.TryWriteString(destination, ref charsWritten, ", ") &&
			SpanWrite.TryWrite(destination, ref charsWritten, M13, format, provider) &&
			SpanWrite.TryWriteString(destination, ref charsWritten, "> <") &&
			SpanWrite.TryWrite(destination, ref charsWritten, M21, format, provider) &&
			SpanWrite.TryWriteString(destination, ref charsWritten, ", ") &&
			SpanWrite.TryWrite(destination, ref charsWritten, M22, format, provider) &&
			SpanWrite.TryWriteString(destination, ref charsWritten, ", ") &&
			SpanWrite.TryWrite(destination, ref charsWritten, M23, format, provider) &&
			SpanWrite.TryWriteString(destination, ref charsWritten, "> <") &&
			SpanWrite.TryWrite(destination, ref charsWritten, M31, format, provider) &&
			SpanWrite.TryWriteString(destination, ref charsWritten, ", ") &&
			SpanWrite.TryWrite(destination, ref charsWritten, M32, format, provider) &&
			SpanWrite.TryWriteString(destination, ref charsWritten, ", ") &&
			SpanWrite.TryWrite(destination, ref charsWritten, M33, format, provider) &&
			SpanWrite.TryWriteChar(destination, ref charsWritten, '>');
	}
}
