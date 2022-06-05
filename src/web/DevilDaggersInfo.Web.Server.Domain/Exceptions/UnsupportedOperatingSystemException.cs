using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Domain.Exceptions;

[Serializable]
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

	protected UnsupportedOperatingSystemException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
