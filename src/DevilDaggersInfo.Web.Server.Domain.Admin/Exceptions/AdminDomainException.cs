using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using System.Net;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;

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

	public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
