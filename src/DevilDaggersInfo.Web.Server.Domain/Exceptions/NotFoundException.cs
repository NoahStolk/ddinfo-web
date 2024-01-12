using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

public class NotFoundException : StatusCodeException
{
	public NotFoundException()
	{
	}

	public NotFoundException(string? message)
		: base(message)
	{
	}

	public NotFoundException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}
