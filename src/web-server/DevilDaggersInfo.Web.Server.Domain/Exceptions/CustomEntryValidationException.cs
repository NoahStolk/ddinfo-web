using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

// TODO: Remove when DDCL 1.8.3 is removed.
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
