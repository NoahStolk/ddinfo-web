using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

public class UnsupportedOperatingSystemException : StatusCodeException
{
	public UnsupportedOperatingSystemException()
	{
	}

	public UnsupportedOperatingSystemException(string? message)
		: base(message)
	{
	}

	public UnsupportedOperatingSystemException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
