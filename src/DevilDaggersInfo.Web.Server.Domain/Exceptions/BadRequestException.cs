using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

public class BadRequestException : StatusCodeException
{
	public BadRequestException()
	{
	}

	public BadRequestException(string? message)
		: base(message)
	{
	}

	public BadRequestException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
