using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

[Serializable]
public abstract class StatusCodeException : Exception
{
	protected StatusCodeException()
	{
	}

	protected StatusCodeException(string? message)
		: base(message)
	{
	}

	protected StatusCodeException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected StatusCodeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public abstract HttpStatusCode StatusCode { get; }
}
