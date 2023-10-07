using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

[Serializable]
public class CustomEntryValidationException : StatusCodeException
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

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
