using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

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

	public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
}
