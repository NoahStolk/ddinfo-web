using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Exceptions;

[Serializable]
public class CustomLeaderboardValidationException : Exception
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
}
