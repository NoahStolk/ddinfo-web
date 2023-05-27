using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

[Serializable]
public class UnauthorizedException : StatusCodeException
{
	public UnauthorizedException()
	{
	}

	public UnauthorizedException(string? message)
		: base(message)
	{
	}

	public UnauthorizedException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected UnauthorizedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
