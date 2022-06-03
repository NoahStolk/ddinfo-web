using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Common.Exceptions;

[Serializable]
public class InvalidEnumConversionException : Exception
{
	public InvalidEnumConversionException()
	{
	}

	public InvalidEnumConversionException(Enum e, [CallerMemberName] string? caller = "")
		: base($"Invalid enum conversion in method {caller} for value {e}.")
	{
	}

	public InvalidEnumConversionException(string? message)
		: base(message)
	{
	}

	public InvalidEnumConversionException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected InvalidEnumConversionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
