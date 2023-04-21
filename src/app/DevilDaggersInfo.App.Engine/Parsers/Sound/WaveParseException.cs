using System;
using System.Runtime.Serialization;

namespace Warp.NET.Parsers.Sound;

[Serializable]
public class WaveParseException : Exception
{
	public WaveParseException()
	{
	}

	public WaveParseException(string? message)
		: base(message)
	{
	}

	public WaveParseException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected WaveParseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
