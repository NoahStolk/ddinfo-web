using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

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

	public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
