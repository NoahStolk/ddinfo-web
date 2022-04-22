using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Exceptions;

[Serializable]
public class CustomEntryValidationException : Exception
{
	public CustomEntryValidationException()
	{
	}

	public CustomEntryValidationException(string? message)
		: base(message)
	{
	}

	public CustomEntryValidationException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected CustomEntryValidationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
