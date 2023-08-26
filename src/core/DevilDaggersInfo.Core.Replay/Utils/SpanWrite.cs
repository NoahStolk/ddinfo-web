namespace DevilDaggersInfo.Core.Replay.Utils;

public static class SpanWrite
{
	public static bool TryWriteChar(Span<char> destination, ref int charsWritten, char value)
	{
		if (destination.IsEmpty)
			return false;

		if (charsWritten + 1 >= destination.Length)
			return false;

		destination[charsWritten++] = value;
		return true;
	}

	public static bool TryWriteString(Span<char> destination, ref int charsWritten, string value)
	{
		if (destination.IsEmpty)
			return false;

		if (charsWritten + value.Length >= destination.Length)
			return false;

		value.AsSpan().CopyTo(destination[charsWritten..]);
		charsWritten += value.Length;
		return true;
	}

	public static bool TryWrite<T>(Span<char> destination, ref int charsWritten, T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : ISpanFormattable
	{
		if (destination.IsEmpty)
			return false;

		if (!value.TryFormat(destination[charsWritten..], out int charsWrittenValue, format, provider))
			return false;

		charsWritten += charsWrittenValue;
		return true;
	}
}
