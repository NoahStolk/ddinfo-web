using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

[Serializable]
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

	protected DdLeaderboardException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
