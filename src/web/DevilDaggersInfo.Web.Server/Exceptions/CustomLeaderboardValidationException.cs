using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Exceptions;

[Serializable]
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

	protected CustomLeaderboardValidationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
