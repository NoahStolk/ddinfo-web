using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

public class CustomLeaderboardValidationException : StatusCodeException
{
	public CustomLeaderboardValidationException()
	{
	}

	public CustomLeaderboardValidationException(string? message)
		: base(message)
	{
	}

	public CustomLeaderboardValidationException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
