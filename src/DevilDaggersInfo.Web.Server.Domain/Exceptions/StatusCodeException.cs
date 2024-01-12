using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

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

	public abstract HttpStatusCode StatusCode { get; }
}
