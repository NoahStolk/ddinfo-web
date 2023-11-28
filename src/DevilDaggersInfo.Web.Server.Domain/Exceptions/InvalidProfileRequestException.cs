using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

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

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
