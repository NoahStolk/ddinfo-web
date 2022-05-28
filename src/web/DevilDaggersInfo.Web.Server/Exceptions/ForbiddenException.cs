using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Exceptions;

[Serializable]
public class ForbiddenException : StatusCodeException
{
	public ForbiddenException()
	{
	}

	public ForbiddenException(string? message)
		: base(message)
	{
	}

	public ForbiddenException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected ForbiddenException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
}
