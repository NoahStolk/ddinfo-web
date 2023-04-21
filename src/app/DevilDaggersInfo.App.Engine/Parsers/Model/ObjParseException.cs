using System;
using System.Runtime.Serialization;

namespace Warp.NET.Parsers.Model;

[Serializable]
public class ObjParseException : Exception
{
	public ObjParseException()
	{
	}

	public ObjParseException(string? message)
		: base(message)
	{
	}

	public ObjParseException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected ObjParseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
