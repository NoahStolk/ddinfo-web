using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

public class CustomEntryValidationException : StatusCodeException
{
	public CustomEntryValidationException()
	{
	}

	public CustomEntryValidationException(string? message)
		: base(message)
	{
	}

	public CustomEntryValidationException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
