using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Exceptions;

[Serializable]
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

	protected InvalidModArchiveException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
