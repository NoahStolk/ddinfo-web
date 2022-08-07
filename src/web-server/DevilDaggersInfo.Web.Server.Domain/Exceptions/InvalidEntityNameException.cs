using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

[Serializable]
public class InvalidEntityNameException : StatusCodeException
{
	public InvalidEntityNameException()
	{
	}

	public InvalidEntityNameException(string? message)
		: base(message)
	{
	}

	public InvalidEntityNameException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected InvalidEntityNameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
