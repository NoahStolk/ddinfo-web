// ReSharper disable InconsistentNaming
using DevilDaggersInfo.Core.Replay.Utils;

namespace DevilDaggersInfo.Core.Replay.Numerics;

public readonly record struct Int16Mat3x3(short M11, short M12, short M13, short M21, short M22, short M23, short M31, short M32, short M33) : ISpanFormattable
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
	{
		return $"<{M11}, {M12}, {M13}> <{M21}, {M22}, {M23}> <{M31}, {M32}, {M33}>";
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
