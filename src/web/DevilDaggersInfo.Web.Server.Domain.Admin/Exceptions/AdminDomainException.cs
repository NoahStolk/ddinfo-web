using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using System.Net;
using System.Runtime.Serialization;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;

[Serializable]
public class AdminDomainException : StatusCodeException
{
	public AdminDomainException()
	{
	}

	public AdminDomainException(string? message)
		: base(message)
	{
	}

	public AdminDomainException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}

	protected AdminDomainException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
