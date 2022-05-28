using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Exceptions;

[Serializable]
public class InvalidProfileRequestException : StatusCodeException
{
	public InvalidProfileRequestException()
	{
	}

	public InvalidProfileRequestException(string? message)
		: base(message)
	{
	}

	public InvalidProfileRequestException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected InvalidProfileRequestException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
