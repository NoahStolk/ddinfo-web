using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Exceptions;

[Serializable]
public class InvalidProfileRequestException : Exception
{
	public InvalidProfileRequestException()
	{
	}

	public InvalidProfileRequestException(string? message)
		: base(message)
	{
	}

	public InvalidProfileRequestException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected InvalidProfileRequestException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.BadRequest;

	public bool ShouldLog { get; init; } = true;
}
