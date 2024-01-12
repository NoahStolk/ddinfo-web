using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

public class DdLeaderboardException : StatusCodeException
{
	public DdLeaderboardException()
	{
	}

	public DdLeaderboardException(string? message)
		: base(message)
	{
	}

	public DdLeaderboardException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
