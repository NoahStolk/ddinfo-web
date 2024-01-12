using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

public class InvalidModArchiveException : StatusCodeException
{
	public InvalidModArchiveException()
	{
	}

	public InvalidModArchiveException(string? message)
		: base(message)
	{
	}

	public InvalidModArchiveException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
